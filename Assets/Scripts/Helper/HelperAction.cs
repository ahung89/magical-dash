using UnityEngine;

public class HelperAction : MonoBehaviour {
    private Spawner spawner;

    void Awake ()
    {
        spawner = GetComponent<Spawner>();
    }

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (CanPlaceItem(point, GameSettings.SmallPlatformHeight, GameSettings.SmallPlatformWidth))
            {
                spawner.SpawnPlatform(point.x, point.y);
            } else
            {
                Debug.Log("Can't place platform.");
            }
        }	
	}

    bool CanPlaceItem(Vector2 point, float height, float width)
    {
        Vector2 lowerLeft = new Vector2(point.x - width / 2f, point.y - height / 2f);
        Vector2 upperRight = new Vector2(point.x + width / 2f, point.y + height / 2f);
        return Physics2D.OverlapArea(lowerLeft, upperRight, 1 << LayerMask.NameToLayer("Platform")) == null;
    }
}
