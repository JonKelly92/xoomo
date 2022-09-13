using UnityEngine;

[RequireComponent(typeof(PlayerLocation))]
public abstract class PlayerInput : MonoBehaviour
{
    [SerializeField] protected int waitForScoreUpdate = 30; // frames to wait before updating the score
    protected int frameCount; // how many frames have a passed since score was last updated
    protected int totalTapCount;
    protected Location location;

    protected GamePlayState gamePlayState;

    protected virtual void Start()
    {
        PlayerLocation playerLocation = gameObject.GetComponent<PlayerLocation>();

        if (playerLocation != null)
            location = playerLocation.Location;
    }

    protected abstract void SendTapInput();
    protected abstract  void ClearScore();
}
