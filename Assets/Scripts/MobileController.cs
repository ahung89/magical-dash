using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class MobileController : MonoBehaviour {

    public Text DetectionDisplay;

    public bool _isSwiping = false;

	void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touchInput = Input.GetTouch(0);

            // Detect: Tap
            if(touchInput.phase == TouchPhase.Ended)
            {
                if(_isSwiping)
                {
                    _isSwiping = false;
                    return;
                }

                DetectionDisplay.text = String.Format("Detected tap at {0}, {1}", touchInput.position.x, touchInput.position.y);
                return;
            }

            // Detect: Swipe
            if (touchInput.phase == TouchPhase.Moved)
            {
                _isSwiping = true;
                DetectionDisplay.text = "Detected swipe";
                return;
            }
        }
	}
}



/*
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlatformDetector;
    public float PlatformDetectionRadius = 0.2f;
    public LayerMask PlatformMask;

    public float BaseJumpForce = 5f;

    public float AddedLongJumpForce = 0.5f;
    public float MaxLongJumpForce = 3f;

    public float ConstantForwardVelocity = 10f;

    private bool _isGrounded = false;
    private bool _isJumping = false;

    private float _remainingLongJumpForce = 0f;

    void FixedUpdate()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(new Vector2(ConstantForwardVelocity - rigidBody.velocity.x, 0));

        _isGrounded = Physics2D.OverlapCircle(PlatformDetector.position, PlatformDetectionRadius, PlatformMask);

        // Basic jump
        if (_isGrounded)
        {
            _isJumping = false;
            _remainingLongJumpForce = 0f;

            if (Input.GetButton("Jump"))
            {
                rigidBody.AddForce(new Vector2(0, BaseJumpForce), ForceMode2D.Impulse);
                _isJumping = true;
                _remainingLongJumpForce = MaxLongJumpForce;
            }
        }

        // Long jump (if user is holding down "jump" during the ascent of a jump)
        if (_isJumping && rigidBody.velocity.y > 0 && _remainingLongJumpForce > 0)
        {
            if (Input.GetButton("Jump"))
            {
                rigidBody.AddForce(new Vector2(0, AddedLongJumpForce), ForceMode2D.Impulse);
                _remainingLongJumpForce -= AddedLongJumpForce;
            }
        }
    }
}

    */