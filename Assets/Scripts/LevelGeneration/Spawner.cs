using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField]
    private float minPlatformY;
    [SerializeField]
    private float maxPlatformY;
    [SerializeField]
    private float minSpawnWaitTime;
    [SerializeField]
    private float maxSpawnWaitTime;
    [SerializeField]
    private int minTerrainSegmentWidth;
    [SerializeField]
    private int maxTerrainSegmentWidth;
    [SerializeField]
    private int minTerrainSegmentHeight;
    [SerializeField]
    private int maxTerrainSegmentHeight;
    [SerializeField]
    private int minContiguousTerrainSegments;
    [SerializeField]
    private int maxContiguousTerrainSegments;

    [SerializeField]
    private StackPool platformPool;
    [SerializeField]
    private StackPool terrainPool;

    // Move to a constant eventually since this is the camera/player speed
    private float velocity = 5f;
    private float smallPlatformWidth = 4.26f;
    private float cameraLowerEdge;

    void Awake()
    {
        Camera camera = GetComponentInParent<Camera>();
        cameraLowerEdge = camera.transform.position.x - camera.orthographicSize;
    }

	// Use this for initialization
	void Start () {
        SpawnNext();
    }

    void SpawnNext()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            SpawnPlatform();
        } else
        {
            SpawnTerrain();
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = platformPool.Pop();
        platform.transform.position = new Vector2(transform.position.x, Random.Range(minPlatformY, maxPlatformY));
        platform.SetActive(true);

        Invoke("SpawnNext", smallPlatformWidth / velocity + Random.Range(minSpawnWaitTime, maxSpawnWaitTime));
    }

    void SpawnTerrain()
    {
        int numSegments = Random.Range(minContiguousTerrainSegments, maxContiguousTerrainSegments);
        int totalWidth = 0; // measured in # tiles

        for (int x = 0; x < numSegments; x++)
        {
            int segmentWidth = Random.Range(minTerrainSegmentWidth, maxTerrainSegmentWidth);
            int segmentHeight = Random.Range(minTerrainSegmentHeight, maxTerrainSegmentHeight);

            TerrainRenderer terrainRenderer = terrainPool.Pop().GetComponent<TerrainRenderer>();
            terrainRenderer.transform.position = new Vector2(transform.position.x + totalWidth, cameraLowerEdge);
            terrainRenderer.gameObject.SetActive(true);
            terrainRenderer.Generate(segmentWidth, segmentHeight);

            totalWidth += segmentWidth;
        }

        Invoke("SpawnNext", (totalWidth * TerrainRenderer.TerrainTileUnit) / velocity + Random.Range(minSpawnWaitTime, maxSpawnWaitTime));
    }
}
