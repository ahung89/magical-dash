using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlatformDetector;
    public float PlatformDetectionRadius = 0.2f;
    public LayerMask PlatformMask;

    public float ConstantForwardVelocity = 10f;

    private bool _isGrounded = false;
    private bool _isJumping = false;
    private bool _isHoldingJumpButton = false;

    private float _jumpStartTime = 0f;

    public float MaxHoldTime;
    public float JumpVelocity ;
    public float Deceleration;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ResetPlayer();
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(PlatformDetector.position, PlatformDetectionRadius, PlatformMask);

        _isHoldingJumpButton = Input.GetButton("Jump");
        bool pressedJumpThisFrame = Input.GetButtonDown("Jump");

        // Basic jump
        if (_isGrounded)
        {
            _isJumping = false;

            if (pressedJumpThisFrame)
            {
                _isJumping = true;
                _jumpStartTime = Time.realtimeSinceStartup;
                rb2d.velocity = new Vector2(rb2d.velocity.x, JumpVelocity);
            }
        }
    }

    void FixedUpdate()
    {
        if (_isGrounded)
        {
            rb2d.velocity = new Vector2(ConstantForwardVelocity, rb2d.velocity.y);
        }

        float jumpTime = Time.realtimeSinceStartup - _jumpStartTime;

        if ((!_isGrounded && !_isJumping) || !_isHoldingJumpButton || (_isJumping && jumpTime > MaxHoldTime))
        {

            float newVelocityY = rb2d.velocity.y - Deceleration;
            rb2d.velocity = new Vector2(ConstantForwardVelocity, newVelocityY);
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
        rb2d.velocity = new Vector2(ConstantForwardVelocity, 0);

        // Move character to the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2 + 100, Camera.main.nearClipPlane);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        rb2d.position = worldCenter;
    }
}