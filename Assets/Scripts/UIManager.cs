using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tapScoreLeft;
    [SerializeField] private TextMeshProUGUI tapScoreRight;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

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
        // TODO: move players to Left or Right or Center based on the game state
        // possibly have an animation
        // send GamePlayStateChangeCompleted when the ui is ready for the game to continue

        // DEBUG --------------
        EventManager.GamePlayStateChangeCompleted();
        // ---------------------
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore, Location location)
    {
    }
    private void EventManager_OnTapScoreUpdate(int tapScore, Location location)
    {
        if (location == Location.Left)
            tapScoreLeft.text = tapScore.ToString();
        else
            tapScoreRight.text = tapScore.ToString();
    }

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

}
