using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int, Location> OnTapScoreUpdate;
    public static event Action<int, Location> OnOverallScoreUpdate;
    public static event Action<int, Location> OnSendingTapCount;
    public static event Action<GamePlayState> OnGamePlayStateChanged;
    public static event Action OnLeftButtonPressed;
    public static event Action OnRightButtonPressed;

    public static void TapScoreUpdated(int tapScore, Location location) => OnTapScoreUpdate?.Invoke(tapScore, location);
    public static void OverallScoreUpdated(int overallScore, Location location) => OnOverallScoreUpdate?.Invoke(overallScore, location);
    public static void SendingTapCount(int tapCount, Location location) => OnSendingTapCount?.Invoke(tapCount, location);

    public static void GameplayStateChanged(GamePlayState gameplayState) => OnGamePlayStateChanged?.Invoke(gameplayState);
    public static void LeftButtonPressed() => OnLeftButtonPressed?.Invoke();
    public static void RightButtonPressed() => OnRightButtonPressed?.Invoke();
}
