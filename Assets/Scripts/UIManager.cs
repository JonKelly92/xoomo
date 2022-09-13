using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tapScore_PlayerOne;
    [SerializeField] private TextMeshProUGUI tapScore_PlayerTwo;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private void Awake()
    {
        EventManager.OnTapScoreUpdate_PlayerOne += EventManager_OnTapScoreUpdate_PlayerOne;
        EventManager.OnOverallScoreUpdate_PlayerOne += EventManager_OnOverallScoreUpdate_PlayerOne;
        EventManager.OnTapScoreUpdate_PlayerTwo += EventManager_OnTapScoreUpdate_PlayerTwo;
        EventManager.OnOverallScoreUpdate_PlayerTwo += EventManager_OnOverallScoreUpdate_PlayerTwo;

        leftButton.onClick.AddListener(LeftBtnPress);
        rightButton.onClick.AddListener(RightBtnPress);
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate_PlayerOne -= EventManager_OnTapScoreUpdate_PlayerOne;
        EventManager.OnOverallScoreUpdate_PlayerOne -= EventManager_OnOverallScoreUpdate_PlayerOne;
        EventManager.OnTapScoreUpdate_PlayerTwo -= EventManager_OnTapScoreUpdate_PlayerTwo;
        EventManager.OnOverallScoreUpdate_PlayerTwo -= EventManager_OnOverallScoreUpdate_PlayerTwo;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
    }

    private void EventManager_OnOverallScoreUpdate_PlayerOne(int overallScore)
    {
    }
    private void EventManager_OnTapScoreUpdate_PlayerOne(int tapSore) => tapScore_PlayerOne.text = tapSore.ToString();

    private void EventManager_OnOverallScoreUpdate_PlayerTwo(int overallScore)
    {
    }
    private void EventManager_OnTapScoreUpdate_PlayerTwo(int tapSore) => tapScore_PlayerTwo.text = tapSore.ToString();

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

}
