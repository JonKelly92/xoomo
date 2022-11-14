using UnityEngine;


// Holds a player's score and gameobject which is used to identify the player later
public class PlayerValues
{
    public int CurrentTapScore;
    public int OverallScore;
    public PlayerSide PlayerSide;

    public PlayerValues(PlayerSide playerSide)
    {
        CurrentTapScore = 0;
        OverallScore = 0;
        this.PlayerSide = playerSide;
    }
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private PlayerValues _playerLeft;
    private PlayerValues _playerRight;

    private int _scoreCap;

    private bool _pauseScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnSendingTapCount += EventManager_OnSendingTapCount;
        EventManager.OnScoreCapSet += EventManager_OnScoreCapSet;
        EventManager.OnGameplayStateChanged += EventManager_OnGameplayStateChanged;

        _pauseScore = false;

        _playerLeft = new PlayerValues(PlayerSide.Left);
        _playerRight = new PlayerValues(PlayerSide.Right);
    }

    private void OnDestroy()
    {
        EventManager.OnSendingTapCount -= EventManager_OnSendingTapCount;
        EventManager.OnScoreCapSet -= EventManager_OnScoreCapSet;
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
    }

    private void EventManager_OnScoreCapSet(int scoreCap) => this._scoreCap = scoreCap;

    private void EventManager_OnSendingTapCount(int tapCount, PlayerSide location)
    {
        if (_pauseScore)
            return;

        if (location == PlayerSide.Left)
            UpdateScore(tapCount, _playerLeft);

        else
            UpdateScore(tapCount, _playerRight);
    }

    private void UpdateScore(int tapCount, PlayerValues player)
    {
        tapCount = tapCount * 5; // I think scores look better when they are a high number

        player.CurrentTapScore += tapCount;
        player.OverallScore += tapCount;

        UpdateTapScoreEvent();
        UpdateOverallScoreEvent(player);

        if (player.CurrentTapScore >= _scoreCap)
        {
            _pauseScore = true;
            EventManager.ScoreCapReached(player.PlayerSide);// this player has won the round
        }
    }

    private void UpdateTapScoreEvent() => EventManager.TapScoreUpdated(_playerLeft.CurrentTapScore, _playerRight.CurrentTapScore);

    private void UpdateOverallScoreEvent(PlayerValues player)
    {
        EventManager.OverallScoreUpdated(player.OverallScore, player.PlayerSide);
    }

    private void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        // starting a new round so reset the score
        if (_playerLeft != null)
            _playerLeft.CurrentTapScore = 0;
        if (_playerRight != null)
            _playerRight.CurrentTapScore = 0;

        _pauseScore = false;
    }

    public PlayerSide GetWinnerByScore()
    {
        if (_playerLeft.OverallScore > _playerRight.OverallScore)
            return PlayerSide.Left;
        else
            return PlayerSide.Right;
    }
}
