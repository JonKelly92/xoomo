using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerObject : MonoBehaviour
{
    public PlayerSide Location { get; set; }

    public void SendTapCountEvent(int tapCount) => EventManager.SendingTapCount(tapCount, Location);

}

