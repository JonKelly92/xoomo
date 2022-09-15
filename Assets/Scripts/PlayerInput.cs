using UnityEngine;

[RequireComponent(typeof(PlayerObject))]
public abstract class PlayerInput : MonoBehaviour
{
    [SerializeField] protected int waitForScoreUpdate = 30; // frames to wait before updating the score
    protected int frameCount; // how many frames have a passed since score was last updated
    protected int totalTapCount;
    protected PlayerObject playerObject;

    protected GamePlayState gamePlayState;

    protected virtual void Awake()
    {
        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
        EventManager.OnGamePlayStateChangeCompleted += EventManager_OnGamePlayStateChangeCompleted;
    }

    protected virtual void Start()
    {
        playerObject = gameObject.GetComponent<PlayerObject>();

        if (playerObject == null)
            Debug.LogError("player object is null");
    }


    protected virtual void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
        EventManager.OnGamePlayStateChangeCompleted -= EventManager_OnGamePlayStateChangeCompleted;
    }

    protected abstract void EventManager_OnGamePlayStateChangeCompleted();

    protected abstract void EventManager_OnScoreCapReached(Location obj);

    protected abstract void SendTapInput();
    protected abstract  void ClearScore();
}
