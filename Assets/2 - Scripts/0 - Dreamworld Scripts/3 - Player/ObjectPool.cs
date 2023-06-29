using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// An interface for an object pool.
/// </summary>
/// <typeparam name="T">The type of objects in the pool.</typeparam>
public interface IObjectPool<T> where T : class
{
    Task<T> GetFromPoolAsync(CancellationToken cancellationToken = default, TimeSpan? timeout = null);
    void ReturnToPool(T item);
    int PoolSize { get; }
    void ClearPool();
}

/// <summary>
/// An object pooling system that uses multithreading and handles concurrent threads accessing the pool.
/// </summary>
/// <typeparam name="T">The type of objects in the pool.</typeparam>
public class ObjectPool<T> : IObjectPool<T> where T : class, new()
{
    private readonly object _lockObject = new();
    private readonly ConcurrentQueue<T> _pool = new();
    private readonly Func<T> _factoryMethod;
    private readonly Action<T> _resetMethod;
    private SemaphoreSlim _semaphore;

    private int _maxSize;
    private int _currentSize;

    /// <summary>
    /// Initializes a new instance of the ObjectPool class.
    /// </summary>
    /// <param name="maxSize">The maximum size of the pool. Can be set to int.MaxValue. Must be greater than zero.</param>
    public ObjectPool(int maxSize)
    {
        if (maxSize < 1)
            throw new ArgumentOutOfRangeException(nameof(maxSize), "Max size must be greater than 0.");

        _maxSize = maxSize;
        _semaphore = new SemaphoreSlim(maxSize, maxSize);

        _factoryMethod = () => new T();
        _resetMethod = null;
    }

    /// <summary>
    /// Initializes a new instance of the ObjectPool class with a custom factory method and reset method.
    /// </summary>
    /// <param name="maxSize">The maximum size of the pool. Can be set to int.MaxValue. Must be greater than zero.</param>
    /// <param name="factoryMethod">The method used to create objects when the pool is empty.</param>
    /// <param name="resetMethod">The method used to reset objects before they are returned to the pool.</param>
    public ObjectPool(int maxSize, Func<T> factoryMethod, Action<T> resetMethod = null) : this(maxSize)
    {
        _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));
        _resetMethod = resetMethod;
    }

    public async Task<T> GetFromPoolAsync(CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Wait for a slot to become available in the semaphore.
        await _semaphore.WaitAsync(timeout ?? TimeSpan.FromSeconds(5), cancellationToken);

        // Try to dequeue an item from the pool.
        if (_pool.TryDequeue(out T item))
        {
            return item;
        }

        // If the pool is empty, create a new item.
        if (Interlocked.Increment(ref _currentSize) > _maxSize)
        {
            // Release the semaphore slot if the maximum pool size is exceeded.
            _semaphore.Release();
            throw new InvalidOperationException("Pool has reached its maximum size.");
        }

        // Create a new item and return it.
        item = _factoryMethod();
        return item;
    }

    public void ReturnToPool(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        _pool.Enqueue(item);
        _semaphore.Release();
    }

    public void ClearPool()
    {
        while (_pool.TryDequeue(out T item))
        {
            Interlocked.Decrement(ref _currentSize);
        }

        _maxSize = int.MaxValue;
        _currentSize = 0;

        _semaphore.Dispose();
        _semaphore = new SemaphoreSlim(_maxSize, _maxSize);
    }

    public int PoolSize => _currentSize;

    /// <summary>
    /// Resizes the pool to the specified maximum size.
    /// </summary>
    /// <param name="maxSize">The new maximum size of the pool.</param>
    public virtual void Resize(int maxSize)
    {
        if (maxSize < 1)
            throw new ArgumentOutOfRangeException(nameof(maxSize), "Max size must be greater than 0.");

        if (maxSize == _maxSize)
            return;

        if (maxSize < _currentSize)
            throw new ArgumentOutOfRangeException(nameof(maxSize), "Max size must be greater than or equal to the current pool size.");

        int difference = maxSize - _maxSize;

        if (difference > 0)
        {
            int availableSlots = _maxSize - _currentSize;
            int slotsToFill = Math.Min(difference, availableSlots);
            _semaphore.Release(slotsToFill);

            lock (_lockObject)
            {
                _maxSize = maxSize;

                while (slotsToFill > 0 && _currentSize < _maxSize)
                {
                    T item = _factoryMethod();
                    _pool.Enqueue(item);
                    Interlocked.Increment(ref _currentSize);
                    slotsToFill--;
                }
            }
        }
        else
        {
            lock (_lockObject)
            {
                _maxSize = maxSize;

                while (_currentSize > _maxSize && _pool.TryDequeue(out T item))
                {
                    _resetMethod?.Invoke(item);
                    Interlocked.Decrement(ref _currentSize);
                }

                int excessSlots = _currentSize - _maxSize;

                while (excessSlots > 0)
                {
                    _semaphore.Wait();
                    if (_pool.TryDequeue(out T item))
                    {
                        _resetMethod?.Invoke(item);
                        Interlocked.Decrement(ref _currentSize);
                        excessSlots--;
                    }
                }
            }
        }
    }
}