using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [SerializeField]
    private Spawner spawner;

    public float GameSpeed = 8;
    public bool GameStarted = false;

    // These will never be changed/configured during game time, so they will remain static.
    public static float SmallPlatformWidth = 4.26f;
    public static float SmallPlatformHeight = .96f;
    public static float DefaultGameSpeed = 8;

    void Awake()
    {
        Instance = this;
        GameSpeed = GameStarted ? DefaultGameSpeed : 0;
    }

    public void StartGame()
    {
        GameSpeed = DefaultGameSpeed;
        spawner.SpawnNext();
    }
}
