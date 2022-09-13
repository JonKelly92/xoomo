using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> OnTapScoreUpdate;
    public static event Action<int> OnOverallScoreUpdate;
    public static event Action<GamePlayState> OnGamePlayStateChanged;
    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;

    public static void TapScoreUpdated(int tapScore) => OnTapScoreUpdate?.Invoke(tapScore);
    public static void OverallScoreUpdated(int overallScore) => OnOverallScoreUpdate?.Invoke(overallScore);

    public static void GameplayStateChanged(GamePlayState gameplayState) => OnGamePlayStateChanged?.Invoke(gameplayState);
    public static void LeftButtonPressed() => OnLeftButtonPressed?.Invoke();
    public static void RightButtonPressed() => OnRightButtonPressed?.Invoke();
}
