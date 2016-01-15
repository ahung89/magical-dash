using UnityEngine;

public class Camera : MonoBehaviour {
    [SerializeField]
    private float velocity;

	void Update () {
        transform.Translate(velocity * Time.deltaTime, 0, 0);
	}
}
