using UnityEngine;


// Holds a player's score and gameobject which is used to identify the player later
public class PlayerValues
{
    public int currentTapScore;
    public int overallScore;
    public Location location;

    public PlayerValues(Location location)
    {
        currentTapScore = 0;
        overallScore = 0;
        this.location = location;
    }
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private PlayerValues playerLeft;
    private PlayerValues playerRight;

    private int scoreCap;

    private bool pauseScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnSendingTapCount += EventManager_OnSendingTapCount;
        EventManager.OnScoreCapSet += EventManager_OnScoreCapSet;
        EventManager.OnGamePlayStateChangeCompleted += EventManager_OnGamePlayStateChangeCompleted;

        pauseScore = false;
    }

    private void OnDestroy()
    {
        EventManager.OnSendingTapCount -= EventManager_OnSendingTapCount;
        EventManager.OnScoreCapSet -= EventManager_OnScoreCapSet;
        EventManager.OnGamePlayStateChangeCompleted -= EventManager_OnGamePlayStateChangeCompleted;
    }

    private void EventManager_OnScoreCapSet(int scoreCap) => this.scoreCap = scoreCap;

    private void EventManager_OnSendingTapCount(int tapCount, Location location)
    {
        // primarily used when switching between gameplay states
        if (pauseScore)
            return;

        if (location == Location.Left)
        {
            if (playerLeft == null)
                playerLeft = new PlayerValues(location);

            UpdateScore(tapCount, playerLeft);
        }
        else
        {
            if (playerRight == null)
                playerRight = new PlayerValues(location);

            UpdateScore(tapCount, playerRight);
        }
    }

    private void UpdateScore(int tapCount, PlayerValues player)
    {
        player.currentTapScore += (tapCount * 5); // x5 because high numbers look better as a score
        player.overallScore += player.currentTapScore;

        UpdateTapScoreEvent(player);
        UpdateOverallScoreEvent(player);

        if (player.currentTapScore >= scoreCap)
        {
            pauseScore = true;
            EventManager.ScoreCapReached(player.location);// this player has won the round
        }
    }

    private void UpdateTapScoreEvent(PlayerValues player)
    {
        EventManager.TapScoreUpdated(player.currentTapScore, player.location);
    }
    private void UpdateOverallScoreEvent(PlayerValues player)
    {
        EventManager.OverallScoreUpdated(player.overallScore, player.location);
    }

    private void EventManager_OnGamePlayStateChangeCompleted()
    {
        // starting a new round so reset the score
        if (playerLeft != null)
            playerLeft.currentTapScore = 0;
        if (playerRight != null)
            playerRight.currentTapScore = 0;

        pauseScore = false;
    }

}
