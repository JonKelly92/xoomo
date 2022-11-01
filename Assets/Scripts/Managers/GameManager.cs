using UnityEngine;

public enum GameplayState
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

    [SerializeField] int roundScoreCap = 130; // when a player reaches this score the round ends and the game play state is changed (swithcing from center to left/right or from left/right to center)
    [SerializeField] int preGameTimer = 3;
    [SerializeField] int gamePlayTimer = 45;

    [SerializeField] private GameObject HumanPlayerPrefab;
    [SerializeField] private GameObject AIPlayerPrefab;

    private GameplayState currentGamePlayState;

    private int animationsInProgress;
    public int PreGameTimer
    {
        get { return preGameTimer; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        GameObject leftPlayerObject = Instantiate(HumanPlayerPrefab, Vector3.zero, Quaternion.identity);
        PlayerObject playerLeft = leftPlayerObject.GetComponent<PlayerObject>();
        if (playerLeft != null)
            playerLeft.Location = PlayerSide.Left;

        GameObject rightPlayerObject = Instantiate(AIPlayerPrefab, Vector3.zero, Quaternion.identity);
        PlayerObject playerRight = rightPlayerObject.GetComponent<PlayerObject>();
        if (playerRight != null)
            playerRight.Location = PlayerSide.Right;

        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
        EventManager.OnAnimationStarted += EventManager_OnAnimationStarted;
        EventManager.OnAnimationEnded += EventManager_OnAnimationEnded;
        EventManager.OnRestartGame += EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu += EventManager_OnExitToMainMenu;
        EventManager.OnGameplayTimerEnd += EventManager_OnGamePlayTimerEnd;
        EventManager.OnPreGameTimerEnd += EventManager_OnPreGameTimerEnd;

        animationsInProgress = 0;
    }

    private void Start()
    {
       // EventManager.PreGameTimerStart(3);
    }

    private void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
        EventManager.OnAnimationStarted -= EventManager_OnAnimationStarted;
        EventManager.OnAnimationEnded -= EventManager_OnAnimationEnded;
        EventManager.OnRestartGame -= EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu -= EventManager_OnExitToMainMenu;
        EventManager.OnGameplayTimerEnd -= EventManager_OnGamePlayTimerEnd;
        EventManager.OnPreGameTimerEnd -= EventManager_OnPreGameTimerEnd;
    }

    private void EventManager_OnPreGameTimerEnd()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        EventManager.ScoreCapSet(roundScoreCap); ;
        SetGamePlayState(GameplayState.Center);

        // DEBUG ---------------- Actually needs to start after the pre game timer 
        EventManager.GamePlayTimerStart(gamePlayTimer);
    }

    private void SetGamePlayState(GameplayState state)
    {
        currentGamePlayState = state;
        EventManager.GameplayStateChanged(state);
    }

    private void EventManager_OnScoreCapReached(PlayerSide playerSide)
    {
        if (currentGamePlayState == GameplayState.Center)
        {
            if (playerSide == PlayerSide.Left)// Left player won the round so we move to the right for the next round
                SetGamePlayState(GameplayState.Right);
            else
                SetGamePlayState(GameplayState.Left);
        }
        else if (currentGamePlayState == GameplayState.Right || currentGamePlayState == GameplayState.Left)
        {
            // if the current game play state is Left (right player has advantage)
            // && the location being passed is Right (right player also won the current round)
            // then the Right player has won the match
            if (currentGamePlayState == GameplayState.Left && playerSide == PlayerSide.Right ||
                currentGamePlayState == GameplayState.Right && playerSide == PlayerSide.Left)
            {
                EventManager.GameOver(playerSide);
            }
            else
                SetGamePlayState(GameplayState.Center);
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

    private void EventManager_OnRestartGame() => SceneTransitionManager.Instance.SwitchScene(SceneStates.SinglePlayerGame);

    private void EventManager_OnExitToMainMenu() => SceneTransitionManager.Instance.SwitchScene(SceneStates.MainMenu);
}
