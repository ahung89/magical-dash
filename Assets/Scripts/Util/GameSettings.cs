using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public float GameSpeed = 8;

    // These will never be changed/configured during game time, so they will remain static.
    public static float SmallPlatformWidth = 4.26f;
    public static float SmallPlatformHeight = .96f;

    public void Awake()
    {
        Instance = this;
    }
}
