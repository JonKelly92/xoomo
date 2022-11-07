using UnityEngine;


public class AIInput : PlayerInput
{
    [SerializeField] private AIDifficulty aiDifficulty;

    private bool pauseTapCount;

    private float timeBetweenTaps;
    private float timeSinceScoreWasSent;

    protected override void Start()
    {
        base.Start();

        pauseTapCount = true;

        timeBetweenTaps = AIDifficultyManager.Instance.GetTimeBetweenTaps();

        ClearScore();
    }

    private void FixedUpdate()
    {
        // block "input", primarily for when rounds are changing and animations are happening
        if (pauseTapCount)
            return;

        timeSinceScoreWasSent -= Time.deltaTime;

        if (timeSinceScoreWasSent <= 0)
        {
            totalTapCount = Random.Range(1, 3);
            SendTapInput();
            ClearScore();
        }
    }

    // Round is starting
    protected override void EventManager_OnGameplayStateChanged(GameplayState obj)
    {
        ClearScore();
        pauseTapCount = false;
    }

    protected override void EventManager_OnScoreCapReached(PlayerSide obj) => pauseTapCount = true;

    protected override void SendTapInput() => playerObject.SendTapCountEvent(totalTapCount);

    protected override void ClearScore()
    {
        totalTapCount = 0;
        timeSinceScoreWasSent = timeBetweenTaps;
    }

    protected override void EventManager_OnGameOver(PlayerSide obj) => pauseTapCount = true;
}
