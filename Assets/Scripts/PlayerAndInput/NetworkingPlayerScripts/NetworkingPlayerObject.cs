using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class NetworkingPlayerObject : NetworkBehaviour
{
    public PlayerSide Location { get; set; }

    private PlayerMovement playerMovement;

    private void Awake()
    {
        EventManager.OnGameplayStateChanged += EventManager_OnGamePlayStateChanged;

        playerMovement = gameObject.GetComponent<PlayerMovement>();

        if (playerMovement == null)
            Debug.LogError("PlayerMovement is null");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
            Location = PlayerSide.Left;
        else
            Location = PlayerSide.Right;
    }

    public override void OnDestroy()
    {
        EventManager.OnGameplayStateChanged -= EventManager_OnGamePlayStateChanged;

        base.OnDestroy();
    }

    private void EventManager_OnGamePlayStateChanged(GameplayState gamePlayState)
    {
        playerMovement.MovePlayer(Location, gamePlayState);
    }

    public void SendTapCountEvent(int tapCount) => EventManager.SendingTapCount(tapCount, Location);
}
