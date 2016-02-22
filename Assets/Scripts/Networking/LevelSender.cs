using UnityEngine;
using SerializableObjects;
using System.Collections;

public class LevelSender : MonoBehaviour {

    [SerializeField]
    private LayerMask obstacleLayerMask;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (1 << other.gameObject.layer == obstacleLayerMask.value)
        {
            SendObjectOverNetwork(other.gameObject);
        }
    }

    void SendObjectOverNetwork (GameObject obj)
    {
        if (obj.tag == "Platform")
        {
            PhotonNetwork.RaiseEvent((int)NetworkEventCode.SpawnPlatform, (Vector2)obj.transform.position, true, null);
        }
        else if (obj.tag == "Terrain")
        {
            TerrainRenderer renderer = obj.GetComponent<TerrainRenderer>();
            if (renderer.ColliderConfigured)
            {
                BoxCollider2D bc = renderer.GetComponent<BoxCollider2D>();
                //1 tile per unit, so we can use size directly
                TerrainBlock block = new TerrainBlock(obj.transform.position, (int)bc.size.x, (int)bc.size.y);
                PhotonNetwork.RaiseEvent((int)NetworkEventCode.SpawnTerrain, block, true, null);
            }
        }
        else if (obj.tag == "Obstacle")
        {
            PhotonNetwork.RaiseEvent((int)NetworkEventCode.SpawnObstacle, (Vector2)obj.transform.position, true, null);
        }
    }
}
