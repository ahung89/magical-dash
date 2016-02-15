using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlatformDetector;
    public float PlatformDetectionRadius = 0.2f;
    public LayerMask PlatformMask;

    public bool _isGrounded = false;
    public bool _isJumping = false;
    public bool _isHoldingJumpButton = false;

    private float _jumpStartTime = 0f;
    private float _constantForwardVelocity;

    private float _halfColliderWidth;
    private float _halfColliderHeight;

    public float MaxHoldTime;
    public float JumpVelocity;
    public float Deceleration;

    private Rigidbody2D rb2d;
    private BoxCollider2D bc;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ResetPlayer();

        bc = GetComponent<BoxCollider2D>();
        _halfColliderWidth = bc.size.x / 2;
        _halfColliderHeight = bc.size.y / 2;
    }

    void Update()
    {
        _constantForwardVelocity = GameSettings.Instance.GameSpeed;

        _isGrounded = IsGrounded();

        _isHoldingJumpButton = IsHoldingJumpTrigger();
        bool pressedJumpThisFrame = IsHoldingJumpTriggerThisFrame();

        _isJumping = rb2d.velocity.y > 0;

        // Basic jump
        if (_isGrounded)
        {
            if (pressedJumpThisFrame && !_isJumping)
            {
                _isJumping = true;
                _jumpStartTime = Time.realtimeSinceStartup;
                rb2d.velocity = new Vector2(rb2d.velocity.x, JumpVelocity);
            }
        }
    }

    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(_constantForwardVelocity, rb2d.velocity.y);

        float jumpTime = Time.realtimeSinceStartup - _jumpStartTime;

        if ((!_isGrounded && (!_isJumping || !_isHoldingJumpButton)) || (_isJumping && jumpTime > MaxHoldTime))
        {
            float newVelocityY = rb2d.velocity.y - Deceleration;
            rb2d.velocity = new Vector2(_constantForwardVelocity, newVelocityY);
        }
    }

    void OnBecameInvisible()
    {
        ResetPlayer();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Obstacle")
        {
            ResetPlayer();
        }

        if (col.gameObject.tag == "Platform" && !_isGrounded)
        {
            rb2d.velocity = Vector2.zero;
            _isJumping = false;
        }
    }

    void ResetPlayer()
    {
        // Reset the player's velocity
        rb2d.velocity = new Vector2(_constantForwardVelocity, 0);

        // Move character to the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2 + 100, Camera.main.nearClipPlane);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        rb2d.position = worldCenter;
    }

    bool IsGrounded()
    {
        Vector2 center = transform.TransformPoint(bc.offset);

        Vector2 lowerLeftCorner = new Vector2(center.x - _halfColliderWidth, center.y - _halfColliderHeight);
        Vector2 lowerRightCorner = new Vector2(center.x + _halfColliderWidth, center.x + _halfColliderHeight);

        return Physics2D.Raycast(lowerLeftCorner, Vector2.down, PlatformDetectionRadius, PlatformMask).transform != null
            || Physics2D.Raycast(lowerRightCorner, Vector2.down, PlatformDetectionRadius, PlatformMask).transform != null;
    }

    bool IsHoldingJumpTrigger()
    {
        bool isHoldingKeyboardTrigger = false;
        bool isHoldingTouchTrigger = false;

        // Detect: Keyboard
        isHoldingKeyboardTrigger = Input.GetButton("Jump");

        // Detect: Touch
        if (Input.touchCount > 0)
        {
            Touch touchInput = Input.GetTouch(0);

            if(touchInput.phase == TouchPhase.Stationary)
            {
                isHoldingTouchTrigger = true;
            }
        }

        return isHoldingKeyboardTrigger || isHoldingTouchTrigger;
    }

    bool IsHoldingJumpTriggerThisFrame()
    {
        bool isHoldingKeyboardTrigger = false;
        bool isHoldingTouchTrigger = false;

        // Detect: Keyboard
        isHoldingKeyboardTrigger = Input.GetButtonDown("Jump");

        // Detect: Touch
        if (Input.touchCount > 0)
        {
            Touch touchInput = Input.GetTouch(0);

            if (touchInput.phase == TouchPhase.Began)
            {
                isHoldingTouchTrigger = true;
            }
        }

        return isHoldingKeyboardTrigger || isHoldingTouchTrigger;
    }
}