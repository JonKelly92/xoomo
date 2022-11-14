using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private TextMeshProUGUI _winnersName;

    [SerializeField] private GameObject _fightBarRed;
    [SerializeField] private GameObject _fightBarBlue;

    [SerializeField] private TextMeshProUGUI _overallScoreLeftPlayer;
    [SerializeField] private TextMeshProUGUI _overallScoreRightPlayer;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [SerializeField] private TextMeshProUGUI _preGameTimer; // this timer counts the seconds until the game starts
    [SerializeField] private TextMeshProUGUI _gameplayTimer; // this timer counts the seconds until the game ends

    [SerializeField] private UnityEvent OnStartPreGameTimer;
    [SerializeField] private UnityEvent OnGameplayStart;

    private GameplayState _gameplayState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnTapScoreUpdate += EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate += EventManager_OnOverallScoreUpdate;
        EventManager.OnGameplayStateChanged += EventManager_OnGameplayStateChanged;
        EventManager.OnGameOver += EventManager_OnGameOver;
        EventManager.OnPreGameTimerEnd += EventManager_OnPreGameTimerEnd;

        _leftButton.onClick.AddListener(LeftBtnPress);
        _rightButton.onClick.AddListener(RightBtnPress);
        _restartButton.onClick.AddListener(RestartGame);
        _exitButton.onClick.AddListener(ExitToMainMenu);
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate -= EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate -= EventManager_OnOverallScoreUpdate;
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
        EventManager.OnGameOver -= EventManager_OnGameOver;
        EventManager.OnPreGameTimerEnd -= EventManager_OnPreGameTimerEnd;

        _leftButton.onClick.RemoveAllListeners();
        _rightButton.onClick.RemoveAllListeners();
        _restartButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }

    private void EventManager_OnPreGameTimerEnd()
    {
        // The game has started 
        OnGameplayStart.Invoke(); 
    }

    private void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        _gameplayState = state;
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore, PlayerSide location)
    {
        if (location == PlayerSide.Left)
            _overallScoreLeftPlayer.text = overallScore.ToString();
        else
            _overallScoreRightPlayer.text = overallScore.ToString();

    }
    private void EventManager_OnTapScoreUpdate(int tapScoreLeft, int tapScoreRight)
    {
        float scoreCap = GameManager.Instance.RoundScoreCap;

        float scorePercentage = Math.Clamp((tapScoreLeft / scoreCap), 0, 1);
        _fightBarRed.transform.localScale = new Vector3(scorePercentage, 1, 1);

        scorePercentage = Math.Clamp((tapScoreRight / scoreCap), 0, 1);
        _fightBarBlue.transform.localScale = new Vector3(scorePercentage, 1, 1);
    }

    public void UpdateGameplayTimer(string formattedTime)
    {
        _gameplayTimer.SetText(formattedTime);
    }

    public void UpdatePreGameTimer(string formattedTime)
    {
        _preGameTimer.SetText(formattedTime);
    }

    private void EventManager_OnGameOver(PlayerSide location)
    {
        if (location == PlayerSide.Left)
            _winnersName.text = "You Win!";
        else
            _winnersName.text = "CPU Wins";

        _gameOverPanel.SetActive(true);
    }

    private void RestartGame() => EventManager.RestartGame();
    private void ExitToMainMenu() => EventManager.ExitToMainMenu();

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

}
