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

    [SerializeField]
    private int minObstacleY;

    [SerializeField]
    private int maxObstacleY;

    private float cameraLowerEdge;

    void Awake()
    {
        Camera camera = GetComponentInParent<Camera>();
        cameraLowerEdge = camera.transform.position.x - camera.orthographicSize;
    }

	// Use this for initialization
	void Start () {
        if (!GameSettings.Instance.MultiplayerMode)
        {
            SpawnNext();
        }
    }

    public void SpawnNext()
    {
        int rand = Random.Range(0, 2);

        if(rand == 0)
        {
            SpawnPlatform();
        }
        else
        {
            SpawnTerrain();
        }
    }

    void SpawnPlatform()
    {
        float platformX = transform.position.x;
        float platformY = Random.Range(minPlatformY, maxPlatformY);

        // Spawn platform
        base.SpawnPlatform(platformX, platformY);

        // Spawn obstacle in random location on top of platform
        if (Random.Range(0, 2) == 0)
        {
            float obstacleX = Random.Range(platformX - (GameSettings.SmallPlatformWidth / 2), platformX + (GameSettings.SmallPlatformWidth / 2));
            float obstacleY = platformY + 1;

            base.SpawnObstacle(obstacleX, obstacleY);
        }

        // Schedule next spawn
        Invoke("SpawnNext", GameSettings.SmallPlatformWidth / GameSettings.Instance.GameSpeed + Random.Range(minSpawnWaitTime, maxSpawnWaitTime));
    }

    void SpawnTerrain()
    {
        // Spawn terrain
        int numSegments = Random.Range(minContiguousTerrainSegments, maxContiguousTerrainSegments);
        int totalWidth = 0; // measured in # tiles

        int currentSequenceWidth = 0;
        int heightOfCurrentSequence = 0;
        TerrainRenderer firstSegment = null;

        for (int x = 0; x < numSegments; x++)
        {
            int segmentWidth = Random.Range(minTerrainSegmentWidth, maxTerrainSegmentWidth);
            int segmentHeight = Random.Range(minTerrainSegmentHeight, maxTerrainSegmentHeight);

            float terrainSegmentX = transform.position.x + totalWidth;
            float terrainSegmentY = cameraLowerEdge;

            TerrainRenderer currentSegment = base.SpawnTerrain(terrainSegmentX, terrainSegmentY, segmentWidth, segmentHeight);

            if (x == 0)
            {
                firstSegment = currentSegment;
            }
            else if (segmentHeight != heightOfCurrentSequence)
            {
                firstSegment.ConfigureCollider(currentSequenceWidth, heightOfCurrentSequence);
                firstSegment = currentSegment;
                currentSequenceWidth = 0;
            }

            if (x == numSegments - 1)
            {
                if (segmentHeight == heightOfCurrentSequence)
                {
                    firstSegment.ConfigureCollider(currentSequenceWidth + segmentWidth, heightOfCurrentSequence);
                } else
                {
                    currentSegment.ConfigureCollider(segmentWidth, segmentHeight);
                }
            }

            currentSequenceWidth += segmentWidth;
            heightOfCurrentSequence = segmentHeight;

            // Spawn obstacle in random location on top of terrain segment
            if (Random.Range(0, 2) == 0)
            {
                float obstacleX = Random.Range(terrainSegmentX - (segmentWidth / 2), terrainSegmentX + (segmentWidth / 2));
                float obstacleY = terrainSegmentY + segmentHeight + 0.5f;

                base.SpawnObstacle(obstacleX, obstacleY);
            }

            totalWidth += segmentWidth;
        }

        // Schedule next spawn
        Invoke("SpawnNext", (totalWidth * TerrainRenderer.TerrainTileUnit) / GameSettings.Instance.GameSpeed
            + Random.Range(minSpawnWaitTime, maxSpawnWaitTime));
    }

    void SpawnObstacle()
    {
        base.SpawnObstacle(transform.position.x, Random.Range(minObstacleY, maxObstacleY));
    }
}
