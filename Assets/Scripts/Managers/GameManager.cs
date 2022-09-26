using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePlayState
{
    Left = 1,
    Center = 2,
    Right = 3
}

public enum PlayerSide
{
    Left = 1,
    Right = 2
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public const string MainMenuSceneName = "MainMenu";

    [SerializeField] int roundScoreCap = 130; // when a player reaches this score the round ends and the game play state is changed (swithcing from center to left/right or from left/right to center)

    [SerializeField] int gamePlayTimer = 45;

    [SerializeField] private GameObject HumanPlayerPrefab;
    [SerializeField] private GameObject AIPlayerPrefab;

    private PlayerObject playerLeft;
    private PlayerObject playerRight;

    private GamePlayState currentGamePlayState;

    private int animationsInProgress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        GameObject leftPlayerObject = Instantiate(HumanPlayerPrefab, Vector3.zero, Quaternion.identity);
        playerLeft = leftPlayerObject.GetComponent<PlayerObject>();
        if (playerLeft != null)
            playerLeft.Location = PlayerSide.Left;

        GameObject rightPlayerObject = Instantiate(AIPlayerPrefab, Vector3.zero, Quaternion.identity);
        playerRight = rightPlayerObject.GetComponent<PlayerObject>();
        if (playerRight != null)
            playerRight.Location = PlayerSide.Right;

        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
        EventManager.OnAnimationStarted += EventManager_OnAnimationStarted;
        EventManager.OnAnimationEnded += EventManager_OnAnimationEnded;
        EventManager.OnRestartGame += EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu += EventManager_OnExitToMainMenu;
        EventManager.OnGamePlayTimerEnd += EventManager_OnGamePlayTimerEnd;

        animationsInProgress = 0;
    }

    private void Start()
    {
        // DEBUG --------------- Not sure if I'm going to keep this here
        StartNewGame();
        // ---------------------
    }

    private void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
        EventManager.OnAnimationStarted -= EventManager_OnAnimationStarted;
        EventManager.OnAnimationEnded -= EventManager_OnAnimationEnded;
        EventManager.OnRestartGame -= EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu -= EventManager_OnExitToMainMenu;
        EventManager.OnGamePlayTimerEnd -= EventManager_OnGamePlayTimerEnd;
    }

    private void StartNewGame()
    {
        EventManager.ScoreCapSet(roundScoreCap); ;
        SetGamePlayState(GamePlayState.Center);

        // DEBUG ---------------- Actually needs to start after the pre game timer 
        EventManager.GamePlayTimerStart(gamePlayTimer);
    }

    private void SetGamePlayState(GamePlayState state)
    {
        currentGamePlayState = state;
        EventManager.GameplayStateChanged(state);
    }

    private void EventManager_OnScoreCapReached(PlayerSide playerSide)
    {
        if (currentGamePlayState == GamePlayState.Center)
        {
            if (playerSide == PlayerSide.Left)// Left player won the round so we move to the right for the next round
                SetGamePlayState(GamePlayState.Right);
            else
                SetGamePlayState(GamePlayState.Left);
        }
        else if (currentGamePlayState == GamePlayState.Right || currentGamePlayState == GamePlayState.Left)
        {
            // if the current game play state is Left (right player has advantage)
            // && the location being passed is Right (right player also won the current round)
            // then the Right player has won the match
            if (currentGamePlayState == GamePlayState.Left && playerSide == PlayerSide.Right ||
                currentGamePlayState == GamePlayState.Right && playerSide == PlayerSide.Left)
            {
                EventManager.GameOver(playerSide);
            }
            else
                SetGamePlayState(GamePlayState.Center);
        }
    }

    private void EventManager_OnGamePlayTimerEnd()
    {
        PlayerSide playerSide = ScoreManager.Instance.GetWinnerByScore();
        EventManager.GameOver(playerSide);


        // DEBUG--------------------------------------------------------------
        Debug.Log("Winner by overall score : " + playerSide.ToString());
    }

    private void EventManager_OnAnimationStarted()
    {
        animationsInProgress++;
    }

    private void EventManager_OnAnimationEnded()
    {
        animationsInProgress--;

        if (animationsInProgress == 0)
            EventManager.GamePlayStateChangeCompleted();
        else if (animationsInProgress < 0)
            Debug.LogError("More animations have finished than were started"); // just in case, save some headaches later
    }

    private void EventManager_OnRestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    private void EventManager_OnExitToMainMenu() => SceneManager.LoadScene(MainMenuSceneName);
}
