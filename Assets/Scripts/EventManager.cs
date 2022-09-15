using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> OnScoreCapSet; // the max score that players are trying to achieve each round has been set
    public static event Action<Location> OnScoreCapReached; // a player has won the round
    public static event Action<int, Location> OnTapScoreUpdate; // the score manager has updated the score for this round and is broadcasting that out
    public static event Action<int, Location> OnOverallScoreUpdate; // the score manager has updated the score for all the rounds combined and is broadcasting that out
    public static event Action<int, Location> OnSendingTapCount; // the input scripts are broadcasting the current tap count
    public static event Action<GamePlayState> OnGamePlayStateChanged; // switching from Center to Left/Right or vice versa
    public static event Action OnGamePlayStateChangeCompleted; // using this allows time for the animation to complete before beginning to record score again
    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;

    public static void ScoreCapSet(int scoreCape) => OnScoreCapSet?.Invoke(scoreCape);
    public static void ScoreCapReached(Location location) => OnScoreCapReached?.Invoke(location);
    public static void TapScoreUpdated(int tapScore, Location location) => OnTapScoreUpdate?.Invoke(tapScore, location);
    public static void OverallScoreUpdated(int overallScore, Location location) => OnOverallScoreUpdate?.Invoke(overallScore, location);
    public static void SendingTapCount(int tapCount, Location location) => OnSendingTapCount?.Invoke(tapCount, location);

    public static void GameplayStateChanged(GamePlayState gameplayState) => OnGamePlayStateChanged?.Invoke(gameplayState);
    public static void GamePlayStateChangeCompleted() => OnGamePlayStateChangeCompleted?.Invoke();
    public static void LeftButtonPressed() => OnLeftButtonPressed?.Invoke();
    public static void RightButtonPressed() => OnRightButtonPressed?.Invoke();
}
