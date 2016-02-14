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
        if(eventCode == 0)
        {
            SpawnPlatform((Vector2)content);
        } else if (eventCode == 1)
        {
            TerrainBlock block = (TerrainBlock)content;
            SpawnTerrain(block.Position, block.Width, block.Height);
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
}
