using UnityEngine;

public enum GamePlayState
{
    Left = 1,
    Center = 2,
    Right = 3
}

public class GameManager : MonoBehaviour
{
    // TODO :
    // Change gameplay state when appropriate
    // Send event when it changes 
    // GameplayStateChanged

    private void Start()
    {
        // Debug -------------------------
        EventManager.GameplayStateChanged(GamePlayState.Center);
        // --------------------------
    }
}
