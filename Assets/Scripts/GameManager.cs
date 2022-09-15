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

    private PlayerObject playerLeft;
    private PlayerObject playerRight;

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

        GameObject leftPlayerObject = Instantiate(HumanPlayerPrefab, Vector3.zero, Quaternion.identity);
        playerLeft = leftPlayerObject.GetComponent<PlayerObject>();
        if (playerLeft != null)
            playerLeft.Location = Location.Left;

        GameObject rightPlayerObject = Instantiate(AIPlayerPrefab, Vector3.zero, Quaternion.identity);
        playerRight = rightPlayerObject.GetComponent<PlayerObject>();
        if (playerRight != null)
            playerRight.Location = Location.Right;
    }

    private void Start()
    {
        // Debug -------------------------
        EventManager.GameplayStateChanged(GamePlayState.Center);
        // --------------------------
    }
}
