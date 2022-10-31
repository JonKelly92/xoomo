using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class NetworkingPlayerObject : NetworkBehaviour
{
    // DEBUG ---------------------
    public NetworkVariable<int> TEST_VAR = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    // ---------------------


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
            //testVar.Value += 1;

            if(IsOwner)
                TEST_VAR.Value++;

            Debug.Log(OwnerClientId + " : " + TEST_VAR.Value.ToString());

            //NetworkingGameManager.Instance.TEST_TEXT.text = testVar.Value.ToString();
            //NetworkingGameManager.Instance.LOG_TEXT.text += testVar.Value.ToString() + System.Environment.NewLine;
            //Debug.Log(testVar.Value.ToString());
        }
    }

    private void EventManager_OnGamePlayStateChanged(GameplayState gamePlayState)
    {
        playerMovement.MovePlayer(Location, gamePlayState);
    }

    public void SendTapCountEvent(int tapCount) => EventManager.SendingTapCount(tapCount, Location);

}

