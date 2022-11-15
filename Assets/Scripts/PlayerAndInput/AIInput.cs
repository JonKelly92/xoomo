using UnityEngine;

public class AIInput : PlayerInput
{
    private bool _pauseTapCount;

    private float _timeBetweenTaps;
    private float _timeSinceScoreWasSent;

    protected override void Start()
    {
        base.Start();

        _pauseTapCount = true;

        _timeBetweenTaps = AIDifficultyManager.Instance.GetTimeBetweenTaps();

        ClearScore();
    }

    private void FixedUpdate()
    {
        // block "input", primarily for when rounds are changing and animations are happening
        if (_pauseTapCount)
            return;

        _timeSinceScoreWasSent -= Time.deltaTime;

        if (_timeSinceScoreWasSent <= 0)
        {
            TotalTapCount = Random.Range(1, 3);
            SendTapInput();
            ClearScore();
        }
    }

    // Round is starting
    protected override void EventManager_OnGameplayStateChanged(GameplayState obj)
    {
        ClearScore();
        _pauseTapCount = false;
    }

    protected override void SendTapInput() => PlayerObject.SendTapCountEvent(TotalTapCount);

    protected override void ClearScore()
    {
        TotalTapCount = 0;
        _timeSinceScoreWasSent = _timeBetweenTaps;
    }

    protected override void EventManager_OnGameOver(PlayerSide obj) => _pauseTapCount = true;
}
