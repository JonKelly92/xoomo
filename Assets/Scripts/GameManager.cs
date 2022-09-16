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

    [SerializeField] int roundScoreCap = 130; // when a player reaches this score the round ends and the game play state is changed (swithcing from center to left/right or from left/right to center)

    [SerializeField] private GameObject HumanPlayerPrefab;
    [SerializeField] private GameObject AIPlayerPrefab;

    private PlayerObject playerLeft;
    private PlayerObject playerRight;

    private GamePlayState currentGamePlayState;

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

        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
    }

    private void Start()
    {
        // DEBUG ---------------
        StartNewGame();
        // ---------------------
    }

    private void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
    }

    private void StartNewGame()
    {
        EventManager.ScoreCapSet(roundScoreCap); ;
        SetGamePlayState(GamePlayState.Center);
    }

    private void SetGamePlayState(GamePlayState state)
    {
        currentGamePlayState = state;
        EventManager.GameplayStateChanged(state);

        // DEBUG -------------------------------------
        //Debug.Log("Game play state: " + currentGamePlayState.ToString());
    }

    private void EventManager_OnScoreCapReached(Location location)
    {
        if (currentGamePlayState == GamePlayState.Center)
        {
            if (location == Location.Left)// Left player won the round so we move to the right for the next round
                SetGamePlayState(GamePlayState.Right);
            else
                SetGamePlayState(GamePlayState.Left);
        }
        else if (currentGamePlayState == GamePlayState.Right || currentGamePlayState == GamePlayState.Left)
        {
            // if the current game play state is Left (right player has advantage)
            // && the location being passed is Right (right player also won the current round)
            // then the Right player has won the match
            if (currentGamePlayState == GamePlayState.Left && location == Location.Right ||
                currentGamePlayState == GamePlayState.Right && location == Location.Left)
            {
                EventManager.GameOver(location);
            }
            else
                SetGamePlayState(GamePlayState.Center);
        }
    }
}
