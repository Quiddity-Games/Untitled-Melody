using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ClickManager : MonoBehaviour
{
    private struct Dash
    {
        public float DashForceMultiplier { get; private set; }
        public float MaxDashDistance { get; private set; }
        public float DashingTime { get; private set; }

        private readonly Rigidbody2D _rigidbody2D;

        public readonly Vector2 Direction
        {
            get
            {
                Vector2 dashDirection = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - _rigidbody2D.position;
                dashDirection.Normalize();
                return dashDirection;
            }
        }

        public Dash(Rigidbody2D rigidbody2D, float dashForceMultiplier = 10f, float maxDashDistance = 5f, float dashingTime = 0.25f)
        {
            _rigidbody2D = rigidbody2D;
            DashForceMultiplier = dashForceMultiplier;
            MaxDashDistance = maxDashDistance;
            DashingTime = dashingTime;
        }
    }

    [SerializeField] private GameObject _cursorAfterImagePrefab;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Dash _dash;
    [SerializeField] private float dashForceMultiplier;
    [SerializeField] private float maxDashDistanceMultiplier;
    [SerializeField] private PlayerAnimationController _playerAnim;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private ParticleSystem ps;
        
    private CameraFollow _cameraFollow;

    public Transform CursorTransform;

    // Declare a pool of lastClickLocation gameobjects
    private ObjectPool<GameObject> _cursorAfterImagePrefabPool;

    public NoteTracker _NoteTracker;

    [SerializeField] private bool canDash;

    [SerializeField] private bool dashEnabled = false;

    private void Awake()
    {
        _NoteTracker.offBeatTrigger += () => { canDash = true; };
        _NoteTracker.HitCallback += HandleDash;

    }

    private void OnEnable()
    {
        DreamworldEventManager.OnCountdownFinish += () => dashEnabled = true;
        DreamworldEventManager.OnDash += DashOnPerformed;
        PauseMenuManager.OnPaused += ToggleControls;
        DreamworldEventManager.OnGameEnd += () => ToggleControls(true);
    }

    private void OnDestroy()
    {
        DreamworldEventManager.OnCountdownFinish -= () => dashEnabled = true;
        DreamworldEventManager.OnDash -= DashOnPerformed;
        PauseMenuManager.OnPaused -= ToggleControls;
        DreamworldEventManager.OnGameEnd -= () => ToggleControls(true);
    }

    public void Start()
    {
        _dash = new Dash(_rigidbody2D);
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();

        // Initialize the _cursorAfterImagePrefabPool pool with 10 objects
        _cursorAfterImagePrefabPool = new ObjectPool<GameObject>(10,() => {
            GameObject obj = Instantiate(_cursorAfterImagePrefab);
            obj.SetActive(false);
            return obj;
        }, item => { item.SetActive(false); });
        //_playerControl = new PlayerControl();
        //DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, DashOnPerformed);
        //_playerControl.Dreamworld.Enable();
        
        //DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.COUNTDOWN_FINISH, EnableDash);
    }

    private void DashOnPerformed()
    {
        if(canDash && dashEnabled){
            HandleClick();
            canDash = false;
        }
    }
    
    public void ToggleControls(bool value)
    {
        if(value)
        {
            CursorTransform.gameObject.SetActive(false);
            _rigidbody2D.Sleep();
        }
        else
        {
            CursorTransform.gameObject.SetActive(true);
            _rigidbody2D.WakeUp();
        }
    }
    
    public void EnableDash()
    {
        dashEnabled = true;
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

    private async void HandleDash(NoteTracker.HitInfo hitInfo)
    {
        float dashScale = 1f;

        switch (hitInfo.rating)
        {
            case NoteTracker.BeatRating.PERFECT:
                dashScale = 1f;
                break;
            case NoteTracker.BeatRating.GREAT:
                dashScale = 0.8f;
                break;
            case NoteTracker.BeatRating.GOOD:
                dashScale = 0.7f;
                break;
            case NoteTracker.BeatRating.MISS:
                dashScale = 0.5f;
                break;
        }

        _cameraFollow.UpdateSpeed(CameraFollow.SmoothSpeedType.Dashing);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 dashLocation = Camera.main.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y));

        float dashDistance = Mathf.Min(_dash.MaxDashDistance, dashScale * Vector2.Distance(dashLocation, _rigidbody2D.position));

        float dashForce = (dashScale + 0.2f) * Vector2.Distance(dashLocation, _playerTransform.position);

        // Get object from the pool
        GameObject cursorPrefab = await _cursorAfterImagePrefabPool.GetFromPoolAsync();

        if (cursorPrefab != null)
        {
            cursorPrefab.transform.position = dashLocation;
        }

        ps.gameObject.transform.position = dashLocation;
        ps.Emit(20);

        if (dashDistance == _dash.MaxDashDistance)
        {
            Vector2 actualDashDirection = _dash.Direction * dashDistance;
            cursorPrefab.transform.position = _rigidbody2D.position + actualDashDirection;

            if (actualDashDirection.x > 0)
                _playerAnim.SetDashRating(hitInfo, true);
            else if (actualDashDirection.x < 0)
                _playerAnim.SetDashRating(hitInfo, false);
        }

        cursorPrefab.SetActive(true);

        StartCoroutine(VanishClickAfterImage(cursorPrefab));

        Vector3 velocity = _rigidbody2D.velocity;
        Vector3.SmoothDamp(_rigidbody2D.position, dashLocation, ref velocity, 0.1f, 10f, Time.fixedDeltaTime);
        _rigidbody2D.velocity = velocity;

        _trailRenderer.startColor = Color.yellow;
        _trailRenderer.endColor = Color.yellow;

        _rigidbody2D.AddForce(_dash.Direction * dashForce, ForceMode2D.Impulse);

        StartCoroutine(ResetGravity());
    }

    public void HandleClick()
    {
        if (!_NoteTracker.onBeat)
        {
            _trailRenderer.startColor = Color.red;
            _trailRenderer.endColor = Color.red;
            _NoteTracker.OnHit();
            StartCoroutine(ResetTrailColor());
            return;
        }

        _NoteTracker.OnHit();
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
        
        yield return new WaitForSeconds(0.1f);

    }
}