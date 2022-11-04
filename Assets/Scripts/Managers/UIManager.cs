using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject fightBarRed;
    [SerializeField] private GameObject fightBarBlue;

    [SerializeField] private TextMeshProUGUI overallScore_LeftPlayer;
    [SerializeField] private TextMeshProUGUI overallScore_RightPlayer;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [SerializeField] private TextMeshProUGUI preGameTimer; // this timer counts the seconds until the game starts
    [SerializeField] private TextMeshProUGUI gameplayTimer; // this timer counts the seconds until the game ends

    [SerializeField] private UnityEvent OnStartPreGameTimer;
    [SerializeField] private UnityEvent OnGameplayStart;

    private GameplayState gameplayState;

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

        leftButton.onClick.AddListener(LeftBtnPress);
        rightButton.onClick.AddListener(RightBtnPress);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate -= EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate -= EventManager_OnOverallScoreUpdate;
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
        EventManager.OnGameOver -= EventManager_OnGameOver;
        EventManager.OnPreGameTimerEnd -= EventManager_OnPreGameTimerEnd;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void EventManager_OnPreGameTimerEnd()
    {
        // The game has started 
        OnGameplayStart.Invoke(); 
    }

    private void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        gameplayState = state;
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore, PlayerSide location)
    {
        if (location == PlayerSide.Left)
            overallScore_LeftPlayer.text = overallScore.ToString();
        else
            overallScore_RightPlayer.text = overallScore.ToString();

    }
    private void EventManager_OnTapScoreUpdate(int tapScoreLeft, int tapScoreRight)
    {
        float scoreCap = GameManager.Instance.RoundScoreCap;

        float scorePercentage = Math.Clamp((tapScoreLeft / scoreCap), 0, 1);
        fightBarRed.transform.localScale = new Vector3(scorePercentage, 1, 1);

        scorePercentage = Math.Clamp((tapScoreRight / scoreCap), 0, 1);
        fightBarBlue.transform.localScale = new Vector3(scorePercentage, 1, 1);
    }

    public void UpdateGameplayTimer(string formattedTime)
    {
        gameplayTimer.SetText(formattedTime);
    }

    public void UpdatePreGameTimer(string formattedTime)
    {
        preGameTimer.SetText(formattedTime);
    }

    private void EventManager_OnGameOver(PlayerSide location) => gameOverPanel.SetActive(true);

    private void RestartGame() => EventManager.RestartGame();
    private void ExitToMainMenu() => EventManager.ExitToMainMenu();

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

}
