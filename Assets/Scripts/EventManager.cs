using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> OnTapScoreUpdate_PlayerOne;
    public static event Action<int> OnOverallScoreUpdate_PlayerOne;
    public static event Action<int> OnTapScoreUpdate_PlayerTwo;
    public static event Action<int> OnOverallScoreUpdate_PlayerTwo;
    public static event Action<GamePlayState> OnGamePlayStateChanged;
    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;

    public static void TapScoreUpdated_PlayerOne(int tapScore) => OnTapScoreUpdate_PlayerOne?.Invoke(tapScore);
    public static void OverallScoreUpdated_PlayerOne(int overallScore) => OnOverallScoreUpdate_PlayerOne?.Invoke(overallScore);
    public static void TapScoreUpdated_PlayerTwo(int tapScore) => OnTapScoreUpdate_PlayerTwo?.Invoke(tapScore);
    public static void OverallScoreUpdated_PlayerTwo(int overallScore) => OnOverallScoreUpdate_PlayerTwo?.Invoke(overallScore);

    public static void GameplayStateChanged(GamePlayState gameplayState) => OnGamePlayStateChanged?.Invoke(gameplayState);
    public static void LeftButtonPressed() => OnLeftButtonPressed?.Invoke();
    public static void RightButtonPressed() => OnRightButtonPressed?.Invoke();
}
