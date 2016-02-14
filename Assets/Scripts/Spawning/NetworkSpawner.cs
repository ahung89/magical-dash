using UnityEngine;
using SerializableObjects;

public class NetworkSpawner : SpawnerBase
{
    [SerializeField]
    private float distanceOffset;

    void Awake () {
        PhotonNetwork.OnEventCall += this.OnEvent;
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
        base.SpawnTerrain(pos.x + distanceOffset, pos.y, width, height);
    }

    void SpawnObstacle(Vector2 pos)
    {
        base.SpawnObstacle(pos.x + distanceOffset, pos.y);
    }
}
