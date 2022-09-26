using UnityEngine;

public class GamePlayTimer : MonoBehaviour
{
    private float timeRemaining;
    private bool isGamePlayTimer; // toggle between counting the game play timer or the pre game timer
    private bool stopTimer;

    void Awake()
    {
        EventManager.OnGamePlayTimerStart += EventManager_OnGamePlayTimerStart;
        EventManager.OnGameOver += EventManager_OnGameOver;

        timeRemaining = 0;
        isGamePlayTimer = false;
        stopTimer = true;
    }

    private void OnDestroy()
    {
        EventManager.OnGamePlayTimerStart -= EventManager_OnGamePlayTimerStart;
        EventManager.OnGameOver -= EventManager_OnGameOver;
    }

    private void EventManager_OnGamePlayTimerStart(int startTime)
    {
        timeRemaining = startTime;
        isGamePlayTimer = true;
        stopTimer = false;
    }

    private void EventManager_OnGameOver(PlayerSide obj)
    {
        stopTimer = true;
    }

    void Update()
    {
        if (stopTimer)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining < 0)
            timeRemaining = 0;

        string formattedTime = timeRemaining.ToString("0.00");

        if (isGamePlayTimer)
            UIManager.Instance.UpdateGamePlayTimer(formattedTime);
        else
            UIManager.Instance.UpdatePreGameTimer(formattedTime);

        if (timeRemaining == 0)
        {
            stopTimer = true;
            EventManager.GamePlayTimerEnd();
        }

    }
}
