using UnityEngine;
using SerializableObjects;

public class NetworkSpawner : SpawnerBase
{
    [SerializeField]
    private float distanceOffset;
    [SerializeField]
    private int startingTerrainHeight;

    void Start () {
        PhotonNetwork.OnEventCall += this.OnEvent;

        if (GameSettings.Instance.MultiplayerMode)
        {
            Vector2 lowerLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
            // Camera width = orthographicSize * aspect * 2. Add 4 to account for helper's spawner offset.
            int width = (int)(Camera.main.orthographicSize * Camera.main.aspect * 2 + distanceOffset + 4);
            TerrainRenderer terrain = base.SpawnTerrain(lowerLeftCorner.x, lowerLeftCorner.y, width , startingTerrainHeight);
            terrain.ConfigureCollider(width, startingTerrainHeight);
        }
    }

    void OnEvent(byte eventCode, object content, int senderId)
    {
        if(eventCode == (int) NetworkEventCode.SpawnPlatform)
        {
            SpawnPlatform((Vector2)content);
        }
        else if (eventCode == (int) NetworkEventCode.SpawnTerrain)
        {
            TerrainBlock block = (TerrainBlock)content;
            SpawnTerrain(block.Position, block.Width, block.Height);
        }
        else if (eventCode == (int) NetworkEventCode.SpawnObstacle)
        {
            SpawnObstacle((Vector2)content);
        }
    }

    void SpawnPlatform(Vector2 pos)
    {
        base.SpawnPlatform(pos.x + distanceOffset, pos.y);
    }

    void SpawnTerrain(Vector2 pos, int width, int height)
    {
        TerrainRenderer terrain = base.SpawnTerrain(pos.x + distanceOffset, pos.y, width, height);
        terrain.ConfigureCollider(width, height);
    }

    void SpawnObstacle(Vector2 pos)
    {
        base.SpawnObstacle(pos.x + distanceOffset, pos.y);
    }
}
