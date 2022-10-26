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

    [SerializeField] private TextMeshProUGUI score_Center_LeftPlayer;
    [SerializeField] private TextMeshProUGUI score_Center_RightPlayer;

    [SerializeField] private TextMeshProUGUI score_Left_LeftPlayer;
    [SerializeField] private TextMeshProUGUI score_Left_RightPlayer;

    [SerializeField] private TextMeshProUGUI score_Right_LeftPlayer;
    [SerializeField] private TextMeshProUGUI score_Right_RightPlayer;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [SerializeField] private TextMeshProUGUI preGameTimer; // this timer counts the seconds until the game starts
    [SerializeField] private TextMeshProUGUI gameplayTimer; // this timer counts the seconds until the game ends

    [SerializeField] private Button startButton;

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
        startButton.onClick.AddListener(StartBtnPress);
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

        // DEBUG --------------

        score_Center_LeftPlayer.gameObject.SetActive(false);
        score_Center_RightPlayer.gameObject.SetActive(false);
        score_Left_LeftPlayer.gameObject.SetActive(false);
        score_Left_RightPlayer.gameObject.SetActive(false);
        score_Right_LeftPlayer.gameObject.SetActive(false);
        score_Right_RightPlayer.gameObject.SetActive(false);

        switch (state)
        {
            case GameplayState.Center:
                score_Center_LeftPlayer.gameObject.SetActive(true);
                score_Center_RightPlayer.gameObject.SetActive(true);
                break;

            case GameplayState.Left:
                score_Left_LeftPlayer.gameObject.SetActive(true);
                score_Left_RightPlayer.gameObject.SetActive(true);
                break;

            case GameplayState.Right:
                score_Right_LeftPlayer.gameObject.SetActive(true);
                score_Right_RightPlayer.gameObject.SetActive(true);
                break;
        }

        ResetScores();
        // ---------------------
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore, PlayerSide location)
    {
    }
    private void EventManager_OnTapScoreUpdate(int tapScore, PlayerSide location)
    {
        // DEBUG: using text meshes to test the code before adding in more complex UI elements ---------------

        if (location == PlayerSide.Left)
        {
            switch (gameplayState)
            {
                case GameplayState.Center:
                    score_Center_LeftPlayer.text = tapScore.ToString();
                    break;
                case GameplayState.Left:
                    score_Left_LeftPlayer.text = tapScore.ToString();
                    break;
                case GameplayState.Right:
                    score_Right_LeftPlayer.text = tapScore.ToString();
                    break;
            }
        }
        else if (location == PlayerSide.Right)
        {
            switch (gameplayState)
            {
                case GameplayState.Center:
                    score_Center_RightPlayer.text = tapScore.ToString();
                    break;
                case GameplayState.Left:
                    score_Left_RightPlayer.text = tapScore.ToString();
                    break;
                case GameplayState.Right:
                    score_Right_RightPlayer.text = tapScore.ToString();
                    break;
            }
        }

        // ---------------------------------------------

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

    private void ResetScores()
    {
        score_Center_LeftPlayer.text = "0";
        score_Center_RightPlayer.text = "0";
        score_Left_LeftPlayer.text = "0";
        score_Left_RightPlayer.text = "0";
        score_Right_LeftPlayer.text = "0";
        score_Right_RightPlayer.text = "0";
    }

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

    private void StartBtnPress()
    {
        OnStartPreGameTimer.Invoke();
        EventManager.PreGameTimerStart(GameManager.Instance.preGameTimer);
    }

}
