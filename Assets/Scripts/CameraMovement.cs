using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    private float velocity;

	void Update () {
        transform.Translate(velocity * Time.deltaTime, 0, 0);
	}
}
