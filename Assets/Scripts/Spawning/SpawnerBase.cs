using UnityEngine;

public class SpawnerBase : MonoBehaviour {

    [SerializeField]
    private StackPool platformPool;

    [SerializeField]
    private StackPool terrainPool;

    [SerializeField]
    private StackPool obstaclePool;

    public virtual void SpawnPlatform (float posX, float posY)
    {
        GameObject platform = platformPool.Pop();
        platform.transform.position = new Vector2(posX, posY);
        platform.SetActive(true);
    }

    public virtual TerrainRenderer SpawnTerrain (float posX, float posY, int width, int height)
    {
        TerrainRenderer terrainRenderer = terrainPool.Pop().GetComponent<TerrainRenderer>();
        terrainRenderer.transform.position = new Vector2(posX, posY);
        terrainRenderer.gameObject.SetActive(true);
        terrainRenderer.Generate(width, height);
        return terrainRenderer;
    }

    public virtual void SpawnObstacle(float posX, float posY)
    {
        GameObject obstacle = obstaclePool.Pop();
        obstacle.transform.position = new Vector2(posX, posY);
        obstacle.SetActive(true);
    }
}
