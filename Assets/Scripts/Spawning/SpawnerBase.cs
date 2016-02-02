using UnityEngine;

public class SpawnerBase : MonoBehaviour {

    [SerializeField]
    private StackPool platformPool;
    [SerializeField]
    private StackPool terrainPool;

    public virtual void SpawnPlatform (float posX, float posY)
    {
        GameObject platform = platformPool.Pop();
        platform.transform.position = new Vector2(posX, posY);
        platform.SetActive(true);
    }

    public virtual void SpawnTerrain (float posX, float posY, int width, int height)
    {
        TerrainRenderer terrainRenderer = terrainPool.Pop().GetComponent<TerrainRenderer>();
        terrainRenderer.transform.position = new Vector2(posX, posY);
        terrainRenderer.gameObject.SetActive(true);
        terrainRenderer.Generate(width, height);
    }
}
