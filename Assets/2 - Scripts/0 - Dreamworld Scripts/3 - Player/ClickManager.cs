using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private Dash _dash;
    [SerializeField] private float dashForceMultiplier;
    [SerializeField] private float maxDashDistanceMultiplier;
    [SerializeField] private PlayerAnimationController _playerAnim;

    [SerializeField] private ParticleSystem ps;
        
    private CameraFollow _cameraFollow;
    private PlayerControl _playerControl;

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
        _playerControl = new PlayerControl();
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, DashOnPerformed);
        _playerControl.Dreamworld.Enable();
        
        DreamworldEventManager.Instance.RegisterVoidEventResponse(DreamworldVoidEventEnum.COUNTDOWN_FINISH, EnableDash);
    }

    private void OnDestroy()
    {
        if(DreamworldEventManager.Instance != null)
        {
            DreamworldEventManager.Instance.DeregisterVoidEventResponse(DreamworldVoidEventEnum.INPUT_DASH, DashOnPerformed);
        }
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
            CursorTransform.gameObject.SetActive(true);
            _playerControl.Dreamworld.Enable();
        }
        else
        {
            CursorTransform.gameObject.SetActive(false);
            _playerControl.Dreamworld.Disable();
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

        Vector2 mousePos = _playerControl.Dreamworld.MousePosition.ReadValue<Vector2>();
        Vector3 dashLocation = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));

        float dashDistance = Mathf.Min(_dash.MaxDashDistance, dashScale * Vector2.Distance(dashLocation, _rigidbody2D.position));
        float dashForce = dashForceMultiplier * maxDashDistanceMultiplier * dashScale;

        /// Left out intentionally for August 2nd, 2023 (-ish) build. May return to it eventually?
        /// If not, delete entire comment block below and this commented section as well.
        /// See comment for details on where it's going wrong:
        /// <see cref="https://discord.com/channels/738438082147254283/738438438365298738/1134927541564608522"/>

        // float dashForce = 5f * dashDistance;
        // Adjusts the player's dash distance based on how far away the cursor is from the player
        // if (Vector3.Distance(dashLocation, _currentPosition) <= maxDashDistanceMultiplier)
        //{
        //    dashForce = dashForceMultiplier * Vector3.Distance(dashLocation, _currentPosition);
        //} else
        //{
        //    //Restricts the dash distance to the max radius when necessary
        //    dashForce = dashForceMultiplier * maxDashDistanceMultiplier;
        //}

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

        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(_dash.Direction * dashForce, ForceMode2D.Impulse);

        _trailRenderer.startColor = Color.yellow;
        _trailRenderer.endColor = Color.yellow;

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