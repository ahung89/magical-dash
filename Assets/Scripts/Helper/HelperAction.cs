using UnityEngine;

public class HelperAction : MonoBehaviour {
    private Spawner spawner;

    private float maxVerticalShift;
    private float maxHorizontalShift;

    private float smallPlatformHeight;
    private float smallPlatformWidth;

    [SerializeField]
    private float shiftBuffer;

    void Awake ()
    {
        spawner = GetComponent<Spawner>();
        maxHorizontalShift = GameSettings.SmallPlatformWidth / 2;
        maxVerticalShift = GameSettings.SmallPlatformHeight / 2;
        smallPlatformHeight = GameSettings.SmallPlatformHeight;
        smallPlatformWidth = GameSettings.SmallPlatformWidth;
    }

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            PlacePlatform(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }	
	}

    bool CanPlaceItem(Vector2 point)
    {
        Vector2 lowerLeft = new Vector2(point.x - smallPlatformWidth / 2f, point.y - smallPlatformHeight / 2f);
        Vector2 upperRight = new Vector2(point.x + smallPlatformWidth / 2f, point.y + smallPlatformHeight / 2f);
        return Physics2D.OverlapArea(lowerLeft, upperRight, 1 << LayerMask.NameToLayer("Platform")) == null;
    }

    void PlacePlatform(Vector2 point)
    {
        Vector2 lowerLeft = new Vector2(point.x - smallPlatformWidth / 2f, point.y - smallPlatformHeight / 2f);
        Vector2 upperRight = new Vector2(point.x + smallPlatformWidth / 2f, point.y + smallPlatformHeight / 2f);
        Collider2D[] colliders = Physics2D.OverlapAreaAll(lowerLeft, upperRight, 1 << LayerMask.NameToLayer("Platform"));

        float leftOverlap = -1, rightOverlap = -1, topOverlap = -1, bottomOverlap = -1;
        float leftEdge = point.x - smallPlatformWidth / 2, rightEdge = point.x + smallPlatformWidth / 2, topEdge = point.y + smallPlatformHeight / 2, bottomEdge = point.y - smallPlatformHeight / 2;

        if (colliders.Length == 0)
        {
            spawner.SpawnPlatform(point.x, point.y);
            return;
        }

        foreach (Collider2D collider in colliders)
        {
            Bounds bounds = collider.bounds;

            if (bounds.min.x > leftEdge & rightEdge - bounds.min.x < maxHorizontalShift)
            {
                rightOverlap = Mathf.Max(rightOverlap, rightEdge - bounds.min.x);
            }

            if (bounds.max.x < rightEdge && bounds.max.x - leftEdge < maxHorizontalShift)
            {
                leftOverlap = Mathf.Max(leftOverlap, bounds.max.x - leftEdge);
            }

            if (bounds.min.y > bottomEdge && topEdge - bounds.min.y < maxVerticalShift)
            {
                topOverlap = Mathf.Max(topOverlap, topEdge - bounds.min.y);
            }

            if (bounds.max.y < topEdge && bounds.max.y - bottomEdge < maxVerticalShift)
            {
                bottomOverlap = Mathf.Max(bottomOverlap, bounds.max.y - bottomEdge);
            }
        }

        if ((rightOverlap > -1 && TryPlace(point.x - (rightOverlap + shiftBuffer), point.y)) 
            || (leftOverlap > -1 && TryPlace(point.x + (leftOverlap + shiftBuffer), point.y))
            || (bottomOverlap > -1 && TryPlace(point.x, point.y + bottomOverlap + shiftBuffer))
            || (topOverlap > -1 && TryPlace(point.x, point.y - (topOverlap + shiftBuffer)))
            || (rightOverlap > -1 && topOverlap > -1 && TryPlace(point.x - (rightOverlap + shiftBuffer), point.y - (topOverlap + shiftBuffer)))
            || (rightOverlap > -1 && bottomOverlap > -1 && TryPlace(point.x - (rightOverlap + shiftBuffer), point.y + bottomOverlap + shiftBuffer))
            || (leftOverlap > -1 && topOverlap > -1 && TryPlace(point.x + leftOverlap + shiftBuffer, point.y - (topOverlap + shiftBuffer)))
            || (leftOverlap > -1 && bottomOverlap > -1 && TryPlace(point.x + leftOverlap + shiftBuffer, point.y + bottomOverlap + shiftBuffer)))
        {
            return;
        }
        else
        {
            Debug.Log("Can't place platform here.");
            // TODO: Play a sound.
        }
    }

    bool TryPlace(float x, float y)
    {
        if (CanPlaceItem(new Vector2(x, y)))
        {
            spawner.SpawnPlatform(x, y);
            return true;
        }

        return false;
    }
}
