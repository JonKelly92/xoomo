using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    // SCORE
    public static event Action<int> OnScoreCapSet; // the max score that players are trying to achieve each round has been set
    public static event Action<PlayerSide> OnScoreCapReached; // a player has won the round
    public static event Action<int, PlayerSide> OnTapScoreUpdate; // the score manager has updated the score for this round and is broadcasting that out
    public static event Action<int, PlayerSide> OnOverallScoreUpdate; // the score manager has updated the score for all the rounds combined and is broadcasting that out
    // TIMER
    public static event Action<int> OnGameplayTimerStar; // this timer counts down to the end of the game, parameter seconds until end of the game
    public static event Action OnGameplayTimerEnd; // times up, the game is over
    public static event Action<int> OnPreGameTimerStart; // this timer counts down to the game startings (i.e. 3-2-1-GO!)
    public static event Action OnPreGameTimerEnd; // The timer has ended now the game can begin
    // INPUT   
    public static event Action<int, PlayerSide> OnSendingTapCount; // the input scripts are broadcasting the current tap count
    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;
    // STATES
    public static event Action<GameplayState> OnGameplayStateChanged; // switching from Center to Left/Right or vice versa
    public static event Action OnGameplayStateChangeCompleted; // using this allows the animation to complete before continueing
    public static event Action<PlayerSide> OnGameOver; // game has ended, PlayerSide is to determine which player has won (left or right)
    public static event Action OnRestartGame;
    public static event Action OnExitToMainMenu;
    // ANIMATION
    public static event Action OnAnimationStarted; // when changing states an animation plays where the sumos move from one part of the screen to another. We need to keep track of how many animations are playing so we can know how many need to end before we continue
    public static event Action OnAnimationEnded;// we can keep track of how many of the animations have stopped so we know when to continue the gameplay
   


    // SCORE
    public static void ScoreCapSet(int scoreCape) => OnScoreCapSet?.Invoke(scoreCape);
    public static void ScoreCapReached(PlayerSide location) => OnScoreCapReached?.Invoke(location);
    public static void TapScoreUpdated(int tapScore, PlayerSide location) => OnTapScoreUpdate?.Invoke(tapScore, location);
    public static void OverallScoreUpdated(int overallScore, PlayerSide location) => OnOverallScoreUpdate?.Invoke(overallScore, location);
    // TIMER
    public static void GamePlayTimerStart(int secondsUntilGameEnd) => OnGameplayTimerStar?.Invoke(secondsUntilGameEnd);
    public static void GameplayTimerEnd() => OnGameplayTimerEnd?.Invoke();
    public static void PreGameTimerStart(int secondsToCountDown) => OnPreGameTimerStart?.Invoke(secondsToCountDown);
    public static void PreGameTimerEnd() => OnPreGameTimerEnd?.Invoke();
    // INPUT
    public static void SendingTapCount(int tapCount, PlayerSide location) => OnSendingTapCount?.Invoke(tapCount, location);
    public static void LeftButtonPressed() => OnLeftButtonPressed?.Invoke();
    public static void RightButtonPressed() => OnRightButtonPressed?.Invoke();
    // STATES
    public static void GameplayStateChanged(GameplayState gameplayState) => OnGameplayStateChanged?.Invoke(gameplayState);
    public static void GamePlayStateChangeCompleted() => OnGameplayStateChangeCompleted?.Invoke();
    public static void GameOver(PlayerSide location) => OnGameOver?.Invoke(location);
    public static void RestartGame() => OnRestartGame?.Invoke();
    public static void ExitToMainMenu() => OnExitToMainMenu?.Invoke();
    // ANIMATION
    public static void AnimationStarted() => OnAnimationStarted?.Invoke();
    public static void AnimationEnded() => OnAnimationEnded?.Invoke();
  
}
