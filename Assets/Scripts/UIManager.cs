using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score_Center_LeftPlayer;
    [SerializeField] private TextMeshProUGUI score_Center_RightPlayer;

    [SerializeField] private TextMeshProUGUI score_Left_LeftPlayer;
    [SerializeField] private TextMeshProUGUI score_Left_RightPlayer;

    [SerializeField] private TextMeshProUGUI score_Right_LeftPlayer;
    [SerializeField] private TextMeshProUGUI score_Right_RightPlayer;

    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private GamePlayState gamePlayState;

    private void Awake()
    {
        EventManager.OnTapScoreUpdate += EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate += EventManager_OnOverallScoreUpdate;
        EventManager.OnGamePlayStateChanged += EventManager_OnGamePlayStateChanged;

        leftButton.onClick.AddListener(LeftBtnPress);
        rightButton.onClick.AddListener(RightBtnPress);
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate -= EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate -= EventManager_OnOverallScoreUpdate;
        EventManager.OnGamePlayStateChanged -= EventManager_OnGamePlayStateChanged;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
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

        Debug.Log("No Null");

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

        EventManager.GamePlayStateChangeCompleted();
        // ---------------------
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore, Location location)
    {
    }
    private void EventManager_OnTapScoreUpdate(int tapScore, Location location)
    {
        // DEBUG: using text meshes to test the code before adding in more complex UI elements

        if (location == Location.Left)
        {
            switch(gamePlayState)
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
        else if (location == Location.Right)
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

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

}
