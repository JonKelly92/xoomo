using UnityEngine;

public class GamePlayTimer : MonoBehaviour
{
    private float timeRemaining;
    private bool isGameplayTimer; // toggle between counting the game play timer or the pre game timer
    private bool stopTimer;

    void Awake()
    {
        EventManager.OnGameplayTimerStar += EventManager_OnGamePlayTimerStart;
        EventManager.OnGameOver += EventManager_OnGameOver;
        EventManager.OnPreGameTimerStart += EventManager_OnPreGameTimerStart;

        timeRemaining = 0;
        isGameplayTimer = false;
        stopTimer = true;
    }

    private void OnDestroy()
    {
        EventManager.OnGameplayTimerStar -= EventManager_OnGamePlayTimerStart;
        EventManager.OnGameOver -= EventManager_OnGameOver;
        EventManager.OnPreGameTimerStart -= EventManager_OnPreGameTimerStart;
    }
    private void EventManager_OnPreGameTimerStart(int startTime)
    {
        timeRemaining = startTime;
        isGameplayTimer = false;
        stopTimer = false;
    }

    private void EventManager_OnGamePlayTimerStart(int startTime)
    {
        timeRemaining = startTime;
        isGameplayTimer = true;
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

        string formattedTime;

        if (isGameplayTimer)
        {
            if (timeRemaining <= 0)
            {
                formattedTime = "0.0";
                stopTimer = true;
                EventManager.GameplayTimerEnd();
            }
            else
                formattedTime = timeRemaining.ToString("0.00");

            UIManager.Instance.UpdateGameplayTimer(formattedTime);
        }
        else
        {
            if (timeRemaining < 1 && timeRemaining > 0.5f)
            {
                formattedTime = "GO!";
            }
            else if(timeRemaining < 0.5f)
            {
                formattedTime = "GO!";
                stopTimer = true;
                EventManager.PreGameTimerEnd();
            }
            else
                formattedTime = timeRemaining.ToString("0");

            UIManager.Instance.UpdatePreGameTimer(formattedTime);
        }
    }
}
