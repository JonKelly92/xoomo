using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private void Awake()
    {
        EventManager.OnTapScoreUpdate += EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate += EventManager_OnOverallScoreUpdate;

        leftButton.onClick.AddListener(LeftBtnPress);
        rightButton.onClick.AddListener(RightBtnPress);
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate -= EventManager_OnTapScoreUpdate;
        EventManager.OnOverallScoreUpdate -= EventManager_OnOverallScoreUpdate;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore)
    {
    }
    private void EventManager_OnTapScoreUpdate(int tapSore) => scoreText.text = tapSore.ToString();

    private void LeftBtnPress() => EventManager.LeftButtonPressed();

    private void RightBtnPress() => EventManager.RightButtonPressed();

}
