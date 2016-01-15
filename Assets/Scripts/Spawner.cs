using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY;
    [SerializeField]
    private StackPool stackPool;

    // Move to a constant eventually since this is the camera/player speed
    private float velocity = 5f;
    private float smallPlatformWidth = 4.26f;

	// Use this for initialization
	void Start () {
        SpawnPlatform();
    }

    void SpawnPlatform()
    {
        GameObject platform = stackPool.Pop();
        platform.transform.position = new Vector2(transform.position.x, Random.Range(minY, maxY));
        platform.SetActive(true);

        Invoke("SpawnPlatform", smallPlatformWidth / velocity + Random.Range(0, .8f));
    }
}
