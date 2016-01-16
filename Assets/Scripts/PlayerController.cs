using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetButton("Jump"))
        {
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
        }
    }

}