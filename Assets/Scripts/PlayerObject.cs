using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public Location Location { get; set; }

    public void SendTapCountEvent(int tapCount) => EventManager.SendingTapCount(tapCount, Location);

}
