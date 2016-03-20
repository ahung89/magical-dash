using UnityEngine;

public class SpawnerBase : MonoBehaviour {

    [SerializeField]
    private StackPool platformPool;

    [SerializeField]
    private StackPool terrainPool;

    [SerializeField]
    private StackPool obstaclePool;

    [SerializeField]
    private StackPool itemPool;

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
        SpawnObject(obstaclePool, posX, posY);
    }

    public virtual void SpawnItem(float posX, float posY)
    {
        SpawnObject(itemPool, posX, posY);
    }

    private void SpawnObject(StackPool pool, float posX, float posY)
    {
        GameObject obj = pool.Pop();
        obj.transform.position = new Vector2(posX, posY);
        obj.SetActive(true);
    }
}
