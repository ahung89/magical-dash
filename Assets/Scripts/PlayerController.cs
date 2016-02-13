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

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidBody.velocity = new Vector2(ConstantForwardVelocity, rigidBody.velocity.y);
        _isHoldingJumpButton = Input.GetButton("Jump");

        // Basic jump
        if (_isGrounded)
        {
            _isJumping = false;

            if (_isHoldingJumpButton)
            {
                _isJumping = true;
                _jumpStartTime = Time.realtimeSinceStartup;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, JumpVelocity);
            }
        }
    }

    void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(PlatformDetector.position, PlatformDetectionRadius, PlatformMask);
        float jumpTime = Time.realtimeSinceStartup - _jumpStartTime;

        if ((!_isGrounded && !_isJumping) || !_isHoldingJumpButton || (_isJumping && jumpTime > MaxHoldTime))
        {

            float newVelocityY = rigidBody.velocity.y - Deceleration;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, newVelocityY);
        }
    }

    void OnBecameInvisible()
    {
        ResetPlayer();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.gameObject.tag == "Obstacle")
        {
            ResetPlayer();
        }
    }

    void ResetPlayer()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        // Reset the player's velocity
        rigidBody.velocity = new Vector2(0, 0);

        // Move character to the center of the screen
        Vector3 screenCenter = new Vector3(Screen.width / 2 + 20, Screen.height / 2, Camera.main.nearClipPlane);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        rigidBody.position = worldCenter;
    }
}