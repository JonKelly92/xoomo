using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    protected int totalTapCount;
    protected PlayerObject playerObject;

    protected GameplayState gamePlayState;

    protected virtual void Awake()
    {
        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
        EventManager.OnGameOver += EventManager_OnGameOver;
        EventManager.OnGameplayStateChanged += EventManager_OnGameplayStateChanged;
    }

    protected virtual void Start()
    {
        playerObject = gameObject.GetComponent<PlayerObject>();

        if (playerObject == null)
            Debug.LogError("PlayerObject is null");
    }

    protected virtual void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
        EventManager.OnGameOver -= EventManager_OnGameOver;
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
    }

    protected abstract void EventManager_OnGameplayStateChanged(GameplayState obj);

    protected abstract void EventManager_OnScoreCapReached(PlayerSide obj);

    protected abstract void EventManager_OnGameOver(PlayerSide obj);

    protected abstract void SendTapInput();
    protected abstract  void ClearScore();
}
