using UnityEngine;


// Holds a player's score and gameobject which is used to identify the player later
public class PlayerValues
{
    public int currentTapScore;
    public int overallScore;
    public PlayerSide playerSide;

    public PlayerValues(PlayerSide playerSide)
    {
        currentTapScore = 0;
        overallScore = 0;
        this.playerSide = playerSide;
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
        EventManager.OnGameplayStateChanged += EventManager_OnGameplayStateChanged;

        pauseScore = false;

        playerLeft = new PlayerValues(PlayerSide.Left);
        playerRight = new PlayerValues(PlayerSide.Right);
    }

    private void OnDestroy()
    {
        EventManager.OnSendingTapCount -= EventManager_OnSendingTapCount;
        EventManager.OnScoreCapSet -= EventManager_OnScoreCapSet;
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
    }

    private void EventManager_OnScoreCapSet(int scoreCap) => this.scoreCap = scoreCap;

    private void EventManager_OnSendingTapCount(int tapCount, PlayerSide location)
    {
        if (pauseScore)
            return;

        if (location == PlayerSide.Left)
            UpdateScore(tapCount, playerLeft);

        else
            UpdateScore(tapCount, playerRight);
    }

    private void UpdateScore(int tapCount, PlayerValues player)
    {
        tapCount = tapCount * 5; // I think scores look better when they are a high number

        player.currentTapScore += tapCount;
        player.overallScore += tapCount;

        UpdateTapScoreEvent();
        UpdateOverallScoreEvent(player);

        if (player.currentTapScore >= scoreCap)
        {
            pauseScore = true;
            EventManager.ScoreCapReached(player.playerSide);// this player has won the round
        }
    }

    private void UpdateTapScoreEvent() => EventManager.TapScoreUpdated(playerLeft.currentTapScore, playerRight.currentTapScore);

    private void UpdateOverallScoreEvent(PlayerValues player)
    {
        EventManager.OverallScoreUpdated(player.overallScore, player.playerSide);

        Debug.Log("Left: " + playerLeft.currentTapScore + ", Right: " + playerRight.currentTapScore);
    }

    private void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        // starting a new round so reset the score
        if (playerLeft != null)
            playerLeft.currentTapScore = 0;
        if (playerRight != null)
            playerRight.currentTapScore = 0;

        pauseScore = false;
    }

    public PlayerSide GetWinnerByScore()
    {
        if (playerLeft.overallScore > playerRight.overallScore)
            return PlayerSide.Left;
        else
            return PlayerSide.Right;
    }
}
