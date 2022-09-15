using UnityEngine;

public enum AIDifficulty
{
    Easy = 1,
    Normal = 2,
    Hard = 3
}

public class AIInput : PlayerInput
{
    [SerializeField] private int minTap_Easy = 0;
    [SerializeField] private int maxTap_Easy = 2;

    [SerializeField] private int minTap_Normal = 2;
    [SerializeField] private int maxTap_Normal = 4;

    [SerializeField] private int minTap_Hard = 4;
    [SerializeField] private int maxTap_Hard = 6;

    [SerializeField] private AIDifficulty aiDifficulty;

    private bool pauseTapCount;

    protected override void Start()
    {
        base.Start();
        pauseTapCount = false;
        ClearScore();
    }

    private void FixedUpdate()
    {
        // block "input", primarily for when rounds are changing and animations are happening
        if (pauseTapCount)
            return;

        frameCount++;

        if (frameCount >= waitForScoreUpdate)
        {
            GetTapCount();
            SendTapInput();
            ClearScore();
        }
    }

    // Round is starting
    protected override void EventManager_OnGamePlayStateChangeCompleted()
    {
        ClearScore();
        pauseTapCount = false;
    }

    protected override void EventManager_OnScoreCapReached(Location obj) => pauseTapCount = true;

    protected override void SendTapInput() => playerObject.SendTapCountEvent(totalTapCount);

    private void GetTapCount()
    {
        int tapCount = 0;

        switch (aiDifficulty)
        {
            case AIDifficulty.Easy:
                tapCount = Random.Range(minTap_Easy, maxTap_Easy);
                break;

            case AIDifficulty.Normal:
                tapCount = Random.Range(minTap_Normal, maxTap_Normal);
                break;

            case AIDifficulty.Hard:
                tapCount = Random.Range(minTap_Hard, maxTap_Hard);
                break;
        }

        totalTapCount = tapCount;
    }

    protected override void ClearScore()
    {
        frameCount = 0;
        totalTapCount = 0;
    }
}
