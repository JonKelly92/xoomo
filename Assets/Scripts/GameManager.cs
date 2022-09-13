using UnityEngine;

public enum GamePlayState
{
    Left = 1,
    Center = 2,
    Right = 3
}

public enum Location
{
    Left = 1,
    Right = 2
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject HumanPlayerPrefab;
    [SerializeField] private GameObject AIPlayerPrefab;

    private GameObject playerLeft;
    private GameObject playerRight;

    // TODO :
    // Change gameplay state when appropriate
    // Send event when it changes 
    // GameplayStateChanged

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        playerLeft = Instantiate(HumanPlayerPrefab, Vector3.zero, Quaternion.identity);
        PlayerLocation playerLocation = playerLeft.GetComponent<PlayerLocation>();
        if (playerLocation != null)
            playerLocation.Location = Location.Left;

        playerLocation = null;

        playerRight = Instantiate(AIPlayerPrefab, Vector3.zero, Quaternion.identity);
        playerLocation = playerRight.GetComponent<PlayerLocation>();
        if (playerLocation != null)
            playerLocation.Location = Location.Right;
    }

    private void Start()
    {
        // Debug -------------------------

        EventManager.GameplayStateChanged(GamePlayState.Center);
        // --------------------------
    }
}
