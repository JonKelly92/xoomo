using UnityEngine;

public class GamePlayArea : MonoBehaviour
{
    public static GamePlayArea Instance { get; private set; }

    [SerializeField] private Transform position_Center_LeftPlayer;// this position is for the player who is on the left and when the round changes that player needs to move to the center of the play area
    [SerializeField] private Transform position_Left_LeftPlayer;
    [SerializeField] private Transform position_Right_LeftPlayer;

    [SerializeField] private Transform position_Center_RightPlayer;
    [SerializeField] private Transform position_Left_RightPlayer;
    [SerializeField] private Transform position_Right_RightPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public Vector3 GetTransformForAnimation(PlayerSide location, GamePlayState gamePlayState)
    {
        Vector3 destination = Vector3.zero;

        // Is this is the player on the left 
        if (location == PlayerSide.Left)
        {
            // Where are we moving that player to
            if (gamePlayState == GamePlayState.Center)
                destination = position_Center_LeftPlayer.position;
            else if (gamePlayState == GamePlayState.Left)
                destination = position_Left_LeftPlayer.position;
            else if (gamePlayState == GamePlayState.Right)
                destination = position_Right_LeftPlayer.position;
        }
        else
        {
            if (gamePlayState == GamePlayState.Center)
                destination = position_Center_RightPlayer.position;
            else if (gamePlayState == GamePlayState.Left)
                destination = position_Left_RightPlayer.position;
            else if (gamePlayState == GamePlayState.Right)
                destination = position_Right_RightPlayer.position;
        }

        return destination;
    }
}
