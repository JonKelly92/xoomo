using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class NetworkingPlayerObject : NetworkBehaviour
{
    private NetworkVariable<int> testVar = new NetworkVariable<int>();

    public PlayerSide Location { get; set; }

    private PlayerMovement playerMovement;

    private void Awake()
    {
        EventManager.OnGameplayStateChanged += EventManager_OnGamePlayStateChanged;

        playerMovement = gameObject.GetComponent<PlayerMovement>();

        if (playerMovement == null)
            Debug.LogError("PlayerMovement is null");
    }

    public override void OnDestroy()
    {
        EventManager.OnGameplayStateChanged -= EventManager_OnGamePlayStateChanged;

        base.OnDestroy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            testVar.Value += 1;
            NetworkingGameManager.Instance.TEST_TEXT.text = testVar.Value.ToString();
            Debug.Log(testVar.Value.ToString());
        }
    }

    private void EventManager_OnGamePlayStateChanged(GameplayState gamePlayState)
    {
        playerMovement.MovePlayer(Location, gamePlayState);
    }

    public void SendTapCountEvent(int tapCount) => EventManager.SendingTapCount(tapCount, Location);

}

