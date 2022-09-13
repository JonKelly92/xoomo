using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    [SerializeField] protected int waitForScoreUpdate = 30; // frames to wait before updating the score
    protected int frameCount; // how many frames have a passed since score was last updated
    protected int totalTapCount;

    protected GamePlayState gamePlayState;

    protected abstract void SendTapInput();
}
