using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        EventManager.OnTapScoreUpdate += EventManager_OnTapScoreUpdate;
        EventManager.OnTiltScoreUpdate += EventManager_OnTiltScoreUpdate;
        EventManager.OnOverallScoreUpdate += EventManager_OnOverallScoreUpdate;
    }

    private void OnDestroy()
    {
        EventManager.OnTapScoreUpdate -= EventManager_OnTapScoreUpdate;
        EventManager.OnTiltScoreUpdate -= EventManager_OnTiltScoreUpdate;
        EventManager.OnOverallScoreUpdate -= EventManager_OnOverallScoreUpdate;
    }

    private void EventManager_OnOverallScoreUpdate(int overallScore)
    {
    }

    private void EventManager_OnTiltScoreUpdate(int tiltScore)
    {
    }

    private void EventManager_OnTapScoreUpdate(int tapSore) => scoreText.text = tapSore.ToString();
}
