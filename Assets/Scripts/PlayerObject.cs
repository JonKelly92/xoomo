using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerObject : MonoBehaviour
{
    public PlayerSide Location { get; set; }

    private PlayerMovement playerMovement;

    private void Awake()
    {
        EventManager.OnGamePlayStateChanged += EventManager_OnGamePlayStateChanged;

        playerMovement = gameObject.GetComponent<PlayerMovement>();

        if (playerMovement == null)
            Debug.LogError("PlayerMovement is null");
    }

    private void OnDestroy()
    {
        EventManager.OnGamePlayStateChanged -= EventManager_OnGamePlayStateChanged;
    }

    private void EventManager_OnGamePlayStateChanged(GamePlayState gamePlayState)
    {
        playerMovement.MovePlayer(Location, gamePlayState); 
    }

    public void SendTapCountEvent(int tapCount) => EventManager.SendingTapCount(tapCount, Location);

}
