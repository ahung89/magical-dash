using UnityEngine;

public class CameraMovement : MonoBehaviour {

    void Update () {
        transform.Translate(GameSettings.Instance.GameSpeed * Time.deltaTime, 0, 0);
	}
}
