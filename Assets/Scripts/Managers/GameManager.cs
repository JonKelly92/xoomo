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

    [SerializeField] private int _roundScoreCap = 130; // when a player reaches this score the round ends and the game play state is changed (swithcing from center to left/right or from left/right to center)
    [SerializeField] private int _preGameTimer = 3;
    [SerializeField] private int _gamePlayTimer = 45;

    private GameplayState _currentGamePlayState;

    public int RoundScoreCap { get { return _roundScoreCap; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
        EventManager.OnRestartGame += EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu += EventManager_OnExitToMainMenu;
        EventManager.OnGameplayTimerEnd += EventManager_OnGamePlayTimerEnd;
        EventManager.OnPreGameTimerEnd += EventManager_OnPreGameTimerEnd;
    }

    private void Start()
    {
        PlayerFactory.Instance.CreateHumanPlayer(PlayerSide.Left);
        PlayerFactory.Instance.CreateAIPlayer(PlayerSide.Right);

        EventManager.PreGameTimerStart(_preGameTimer);
    }

    private void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
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
        EventManager.ScoreCapSet(_roundScoreCap); ;
        SetGamePlayState(GameplayState.Center);
        EventManager.GamePlayTimerStart(_gamePlayTimer);
    }

    private void SetGamePlayState(GameplayState state)
    {
        _currentGamePlayState = state;
        EventManager.GameplayStateChanged(state);
    }

    private void EventManager_OnScoreCapReached(PlayerSide playerSide)
    {
        if (_currentGamePlayState == GameplayState.Center)
        {
            if (playerSide == PlayerSide.Left)// Left player won the round so we move to the right for the next round
                SetGamePlayState(GameplayState.Right);
            else
                SetGamePlayState(GameplayState.Left);
        }
        else if (_currentGamePlayState == GameplayState.Right || _currentGamePlayState == GameplayState.Left)
        {
            // if the current game play state is Left (right player has advantage)
            // && the location being passed is Right (right player also won the current round)
            // then the Right player has won the match
            if (_currentGamePlayState == GameplayState.Left && playerSide == PlayerSide.Right ||
                _currentGamePlayState == GameplayState.Right && playerSide == PlayerSide.Left)
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
    }

    private void EventManager_OnRestartGame() => SceneTransitionManager.Instance.SwitchScene(SceneStates.GameScene);

    private void EventManager_OnExitToMainMenu() => SceneTransitionManager.Instance.SwitchScene(SceneStates.MainMenu);
}
