using UnityEngine;
using SerializableObjects;
using System.Collections.Generic;

public class Destroyer : MonoBehaviour {
    [SerializeField]
    private List<StackPool> pools;
    [SerializeField]
    private LayerMask obstacleLayerMask;

    private Dictionary<string, StackPool> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<string, StackPool>();
        for(int x = 0; x < pools.Count; x++)
        {
            StackPool pool = pools[x];
            poolDictionary.Add(pool.PooledObject.tag, pool);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == obstacleLayerMask.value)
        {
            StackPool pool = poolDictionary[other.gameObject.tag];
            other.gameObject.SetActive(false);
            pool.Push(other.gameObject);
            SendObjectOverNetwork(other.gameObject);
        }
    }

    void SendObjectOverNetwork(GameObject obj)
    {
        if (obj.tag == "Platform")
        {
            PhotonNetwork.RaiseEvent((int) NetworkEventCode.SpawnPlatform, (Vector2) obj.transform.position, true, null);
        }
        else if (obj.tag == "Terrain")
        {
            TerrainRenderer renderer = obj.GetComponent<TerrainRenderer>();
            TerrainBlock block = new TerrainBlock(obj.transform.position, renderer.WidthInTiles, renderer.HeightInTiles);
            PhotonNetwork.RaiseEvent((int) NetworkEventCode.SpawnTerrain, block, true, null);
        }
        else if (obj.tag == "Obstacle")
        {
            PhotonNetwork.RaiseEvent((int) NetworkEventCode.SpawnObstacle, (Vector2)obj.transform.position, true, null);
        }
    }
}
