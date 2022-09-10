using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> OnTapScoreUpdate;
    public static event Action<int> OnTiltScoreUpdate;
    public static event Action<int> OnOverallScoreUpdate;

    public static void TapScoreUpdated(int tapScore) => OnTapScoreUpdate?.Invoke(tapScore);
    public static void TiltScoreUpdate(int tiltScore) => OnTiltScoreUpdate?.Invoke(tiltScore);
    public static void OverallScoreUpdated(int overallScore) => OnOverallScoreUpdate?.Invoke(overallScore);

}
