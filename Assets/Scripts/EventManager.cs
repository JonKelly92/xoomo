using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> OnScoreCapSet; // the max score that players are trying to achieve each round has been set
    public static event Action<PlayerSide> OnScoreCapReached; // a player has won the round
    public static event Action<int, PlayerSide> OnTapScoreUpdate; // the score manager has updated the score for this round and is broadcasting that out
    public static event Action<int, PlayerSide> OnOverallScoreUpdate; // the score manager has updated the score for all the rounds combined and is broadcasting that out
    public static event Action<int, PlayerSide> OnSendingTapCount; // the input scripts are broadcasting the current tap count
    public static event Action<GamePlayState> OnGamePlayStateChanged; // switching from Center to Left/Right or vice versa
    public static event Action OnGamePlayStateChangeCompleted; // using this allows time for the animation to complete before beginning to record score again
    public static event Action OnAnimationStarted; // when changing states an animation plays where the sumos move from one part of the screen to another. We need to keep track of how many animations are playing so we can know how many need to end before we continue
    public static event Action OnAnimationEnded;// we can keep track of how many of the animations have stopped so we know when to continue the gameplay
    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;
    public static event Action<PlayerSide> OnGameOver; // game has ended, location is to determine which player has won (left or right)

    public static void ScoreCapSet(int scoreCape) => OnScoreCapSet?.Invoke(scoreCape);
    public static void ScoreCapReached(PlayerSide location) => OnScoreCapReached?.Invoke(location);
    public static void TapScoreUpdated(int tapScore, PlayerSide location) => OnTapScoreUpdate?.Invoke(tapScore, location);
    public static void OverallScoreUpdated(int overallScore, PlayerSide location) => OnOverallScoreUpdate?.Invoke(overallScore, location);
    public static void SendingTapCount(int tapCount, PlayerSide location) => OnSendingTapCount?.Invoke(tapCount, location);
    public static void GameplayStateChanged(GamePlayState gameplayState) => OnGamePlayStateChanged?.Invoke(gameplayState);
    public static void GamePlayStateChangeCompleted() => OnGamePlayStateChangeCompleted?.Invoke();
    public static void AnimationStarted() => OnAnimationStarted?.Invoke();
    public static void AnimationEnded() => OnAnimationEnded?.Invoke();
    public static void LeftButtonPressed() => OnLeftButtonPressed?.Invoke();
    public static void RightButtonPressed() => OnRightButtonPressed?.Invoke();

    public static void GameOver(PlayerSide location) => OnGameOver?.Invoke(location);
}
