using System.Collections;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private struct Dash
    {
        public float DashForceMultiplier { get; private set; }
        public float MaxDashDistance { get; private set; }
        public float DashingTime { get; private set; }

        private readonly Rigidbody2D _rigidbody2D;

        private PlayerControl _playerControl;

        public readonly Vector2 Direction
        {
            get
            {
                Vector2 dashDirection = (Vector2)Camera.main.ScreenToWorldPoint(_playerControl.Dreamworld.MousePosition.ReadValue<Vector2>()) - _rigidbody2D.position;
                dashDirection.Normalize();
                return dashDirection;
            }
        }

        public Dash(Rigidbody2D rigidbody2D, float dashForceMultiplier = 10f, float maxDashDistance = 5f, float dashingTime = 0.25f)
        {
            _playerControl = new PlayerControl();
            _rigidbody2D = rigidbody2D;
            DashForceMultiplier = dashForceMultiplier;
            MaxDashDistance = maxDashDistance;
            DashingTime = dashingTime;
            _playerControl.Enable();
        }
    }

    [SerializeField] private GameObject _cursorAfterImagePrefab;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _cursorSpriteRenderer;
    [SerializeField] private Dash _dash;

    private CameraFollow _cameraFollow;
    private PlayerControl _playerControl;

    public Transform CursorTransform;

    // Declare a pool of lastClickLocation gameobjects
    private ObjectPool<GameObject> _cursorAfterImagePrefabPool;

    public NoteTracker _NoteTracker;

    [SerializeField] private bool canDash;
    
    
    private void Start()
    {
        _dash = new Dash(_rigidbody2D);
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();

        // Initialize the _cursorAfterImagePrefabPool pool with 10 objects
        _cursorAfterImagePrefabPool = new ObjectPool<GameObject>(10,() => {
            GameObject obj = Instantiate(_cursorAfterImagePrefab);
            obj.SetActive(false);
            return obj;
        }, item => { item.SetActive(false); });
        _playerControl = new PlayerControl();
        _playerControl.Dreamworld.Dash.performed += (context =>
        {
            if(canDash){
            HandleClick();
            canDash = false;
            }
            
        });

        _playerControl.Dreamworld.Enable();
    }

    public void Initialize()
    {
        _NoteTracker.offBeatTrigger += () => { canDash = true; };
    }

    private IEnumerator VanishClickAfterImage(GameObject cursorPrefab)
    {
        SpriteRenderer _spriteRenderer = cursorPrefab.GetComponent<SpriteRenderer>();
        float alpha = _spriteRenderer.color.a;
        Vector3 rgb = new Vector3(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b);

        while (alpha >= 0)
        {
            _spriteRenderer.color = new Vector4(rgb.x, rgb.y, rgb.z, alpha);
            alpha -= 0.02f;

            yield return 0;
        }

        // return object to the pool
        _cursorAfterImagePrefabPool.ReturnToPool(cursorPrefab);
    }

    public async void HandleClick()
    {
        
        _cameraFollow.UpdateSpeed(CameraFollow.SmoothSpeedType.Dashing);
        float dashDistance = Mathf.Min(_dash.MaxDashDistance, Vector2.Distance(CursorTransform.position, _rigidbody2D.position));
        float dashForce = _dash.DashForceMultiplier * dashDistance;

        if (!_NoteTracker.onBeat)
        {
            _trailRenderer.startColor = Color.red;
            _trailRenderer.endColor = Color.red;
            _cursorSpriteRenderer.color = Color.red;

            StartCoroutine(ResetTrailColor());
            return;
        }

        // Get object from the pool
        GameObject cursorPrefab = await _cursorAfterImagePrefabPool.GetFromPoolAsync();

        if (cursorPrefab != null)
        {
            cursorPrefab.transform.position = CursorTransform.position;
        }

        if (dashDistance == _dash.MaxDashDistance)
        {
            Vector2 actualDashDirection = _dash.Direction * dashDistance;
            cursorPrefab.transform.position = _rigidbody2D.position + actualDashDirection;
        }

        cursorPrefab.SetActive(true);

        StartCoroutine(VanishClickAfterImage(cursorPrefab));

        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(_dash.Direction * dashForce, ForceMode2D.Impulse);


        _trailRenderer.startColor = Color.yellow;
        _trailRenderer.endColor = Color.yellow;
        _cursorSpriteRenderer.color = Color.white;

        StartCoroutine(ResetGravity());
    }

    private IEnumerator ResetTrailColor()
    {
        yield return new WaitForSeconds(0.5f);

        _trailRenderer.startColor = Color.white;
        _trailRenderer.endColor = Color.white;
    }

    private IEnumerator ResetGravity()
    {
        yield return new WaitForSeconds(_dash.DashingTime);

        _rigidbody2D.gravityScale = 1f;
        _trailRenderer.startColor = Color.white;
        _trailRenderer.endColor = Color.white;
    }
}