using UnityEngine;

public class GamePlayTimer : MonoBehaviour
{
    private float _timeRemaining;
    private bool _isGameplayTimer; // toggle between counting the game play timer or the pre game timer
    private bool _stopTimer;

    void Awake()
    {
        EventManager.OnGameplayTimerStart += EventManager_OnGamePlayTimerStart;
        EventManager.OnGameOver += EventManager_OnGameOver;
        EventManager.OnPreGameTimerStart += EventManager_OnPreGameTimerStart;

        _timeRemaining = 0;
        _isGameplayTimer = false;
        _stopTimer = true;
    }

    private void OnDestroy()
    {
        EventManager.OnGameplayTimerStart -= EventManager_OnGamePlayTimerStart;
        EventManager.OnGameOver -= EventManager_OnGameOver;
        EventManager.OnPreGameTimerStart -= EventManager_OnPreGameTimerStart;
    }
    private void EventManager_OnPreGameTimerStart(int startTime)
    {
        _timeRemaining = startTime;
        _isGameplayTimer = false;
        _stopTimer = false;
    }

    private void EventManager_OnGamePlayTimerStart(int startTime)
    {
        _timeRemaining = startTime;
        _isGameplayTimer = true;
        _stopTimer = false;
    }

    private void EventManager_OnGameOver(PlayerSide obj)
    {
        _stopTimer = true;
    }

    void Update()
    {
        if (_stopTimer)
            return;

        _timeRemaining -= Time.deltaTime;

        string formattedTime;

        if (_isGameplayTimer)
        {
            if (_timeRemaining <= 0)
            {
                formattedTime = "0.0";
                _stopTimer = true;
                EventManager.GameplayTimerEnd();
            }
            else
                formattedTime = _timeRemaining.ToString("0.00");

            UIManager.Instance.UpdateGameplayTimer(formattedTime);
        }
        else
        {
            if (_timeRemaining < 1 && _timeRemaining > 0.5f)
            {
                formattedTime = "GO!";
            }
            else if(_timeRemaining < 0.5f)
            {
                formattedTime = "GO!";
                _stopTimer = true;
                EventManager.PreGameTimerEnd();
            }
            else
                formattedTime = _timeRemaining.ToString("0");

            UIManager.Instance.UpdatePreGameTimer(formattedTime);
        }
    }
}
