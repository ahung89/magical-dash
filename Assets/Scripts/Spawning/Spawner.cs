using UnityEngine;

public class Spawner : SpawnerBase
{
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
        base.SpawnPlatform(transform.position.x, Random.Range(minPlatformY, maxPlatformY));

        Invoke("SpawnNext", GameSettings.SmallPlatformWidth / GameSettings.GameSpeed + Random.Range(minSpawnWaitTime, maxSpawnWaitTime));
    }

    void SpawnTerrain()
    {
        int numSegments = Random.Range(minContiguousTerrainSegments, maxContiguousTerrainSegments);
        int totalWidth = 0; // measured in # tiles

        for (int x = 0; x < numSegments; x++)
        {
            int segmentWidth = Random.Range(minTerrainSegmentWidth, maxTerrainSegmentWidth);
            int segmentHeight = Random.Range(minTerrainSegmentHeight, maxTerrainSegmentHeight);

            base.SpawnTerrain(transform.position.x + totalWidth, cameraLowerEdge, segmentWidth, segmentHeight);

            totalWidth += segmentWidth;
        }

        Invoke("SpawnNext", (totalWidth * TerrainRenderer.TerrainTileUnit) / GameSettings.GameSpeed
            + Random.Range(minSpawnWaitTime, maxSpawnWaitTime));
    }
}
