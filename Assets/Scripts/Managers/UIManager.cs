using System;
using TMPro;
using UnityEngine;
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

    [SerializeField] private TextMeshProUGUI gameplayTimer; // this timer counts the seconds until the game ends

    private GamePlayState gamePlayState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnTapScoreUpdate += EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate += EventManager_OnOverallScoreUpdate;
        EventManager.OnGamePlayStateChanged += EventManager_OnGamePlayStateChanged;
        EventManager.OnGameOver += EventManager_OnGameOver;

        leftButton.onClick.AddListener(LeftBtnPress);
        rightButton.onClick.AddListener(RightBtnPress);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate -= EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate -= EventManager_OnOverallScoreUpdate;
        EventManager.OnGamePlayStateChanged -= EventManager_OnGamePlayStateChanged;
        EventManager.OnGameOver -= EventManager_OnGameOver;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void EventManager_OnGamePlayStateChanged(GamePlayState state)
    {
        gamePlayState = state;

        // TODO: move players to Left or Right or Center based on the game state
        // possibly have an animation
        // send GamePlayStateChangeCompleted when the ui is ready for the game to continue

        // DEBUG --------------

        score_Center_LeftPlayer.gameObject.SetActive(false);
        score_Center_RightPlayer.gameObject.SetActive(false);
        score_Left_LeftPlayer.gameObject.SetActive(false);
        score_Left_RightPlayer.gameObject.SetActive(false);
        score_Right_LeftPlayer.gameObject.SetActive(false);
        score_Right_RightPlayer.gameObject.SetActive(false);

        switch (state)
        {
            case GamePlayState.Center:
                score_Center_LeftPlayer.gameObject.SetActive(true);
                score_Center_RightPlayer.gameObject.SetActive(true);
                break;

            case GamePlayState.Left:
                score_Left_LeftPlayer.gameObject.SetActive(true);
                score_Left_RightPlayer.gameObject.SetActive(true);
                break;

            case GamePlayState.Right:
                score_Right_LeftPlayer.gameObject.SetActive(true);
                score_Right_RightPlayer.gameObject.SetActive(true);
                break;
        }

        ResetScores();

        //EventManager.GamePlayStateChangeCompleted();
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
            switch (gamePlayState)
            {
                case GamePlayState.Center:
                    score_Center_LeftPlayer.text = tapScore.ToString();
                    break;
                case GamePlayState.Left:
                    score_Left_LeftPlayer.text = tapScore.ToString();
                    break;
                case GamePlayState.Right:
                    score_Right_LeftPlayer.text = tapScore.ToString();
                    break;
            }
        }
        else if (location == PlayerSide.Right)
        {
            switch (gamePlayState)
            {
                case GamePlayState.Center:
                    score_Center_RightPlayer.text = tapScore.ToString();
                    break;
                case GamePlayState.Left:
                    score_Left_RightPlayer.text = tapScore.ToString();
                    break;
                case GamePlayState.Right:
                    score_Right_RightPlayer.text = tapScore.ToString();
                    break;
            }
        }

        // ---------------------------------------------

    }

    public void UpdateGamePlayTimer(string formattedTime)
    {
        gameplayTimer.SetText(formattedTime);
    }

    public void UpdatePreGameTimer(string formattedTime)
    {

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

}
