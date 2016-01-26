using UnityEngine;
using SerializableObjects;

public class NetworkSpawner : MonoBehaviour {
    [SerializeField]
    private StackPool platformPool;
    [SerializeField]
    private StackPool terrainPool;
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
        GameObject platform = platformPool.Pop();
        platform.transform.position = new Vector2(pos.x + distanceOffset, pos.y);
        platform.SetActive(true);
    }

    void SpawnTerrain(Vector2 pos, int width, int height)
    {
        TerrainRenderer terrainRenderer = terrainPool.Pop().GetComponent<TerrainRenderer>();
        terrainRenderer.transform.position = new Vector2(pos.x + distanceOffset, pos.y);
        terrainRenderer.gameObject.SetActive(true);
        terrainRenderer.Generate(width, height);
    }
}
