using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public ObstacleMovementBehavior MovementBehavior { get; set; }

    void OnBecameVisible()
    {
        if (MovementBehavior == ObstacleMovementBehavior.Falling)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
