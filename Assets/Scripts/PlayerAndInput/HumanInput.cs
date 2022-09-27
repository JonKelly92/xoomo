
using UnityEngine;

public class HumanInput : PlayerInput
{
    private enum AcceptInputFrom
    {
        LeftBtn = 1,
        RightBtn = 2,
        None = 3
    }

    private int leftTapCount;
    private int rightTapCount;
    private AcceptInputFrom activeButton;

    protected override void Awake()
    {
        base.Awake();

        EventManager.OnGameplayStateChanged += EventManager_OnGamePlayStateChanged;
        EventManager.OnLeftButtonPressed += LeftBtnPress;
        EventManager.OnRightButtonPressed += RightBtnPress;

        ClearScore();
        activeButton = AcceptInputFrom.None;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnGameplayStateChanged -= EventManager_OnGamePlayStateChanged;
        EventManager.OnLeftButtonPressed -= LeftBtnPress;
        EventManager.OnRightButtonPressed -= RightBtnPress;
    }

    // Round has ended and the new game play state is being chosen, block input while this is happening
    protected override void EventManager_OnScoreCapReached(PlayerSide obj) => activeButton = AcceptInputFrom.None;

    private void EventManager_OnGamePlayStateChanged(GameplayState state) => gamePlayState = state;

    // A new round is starting
    protected override void EventManager_OnGamePlayStateChangeCompleted()
    {
        ClearScore();

        // Setting the state now so we can immediately start recieving input
        switch (gamePlayState)
        {
            case GameplayState.Left:
                activeButton = AcceptInputFrom.LeftBtn;
                break;
            case GameplayState.Right:
                activeButton = AcceptInputFrom.RightBtn;
                break;
            case GameplayState.Center:
                activeButton = AcceptInputFrom.LeftBtn;
                break;
            default:
                activeButton = AcceptInputFrom.None;
                break;

        }
    }

    protected override void SendTapInput() => playerObject.SendTapCountEvent(totalTapCount);

    private void FixedUpdate()
    {
        // block all input
        if (activeButton == AcceptInputFrom.None)
            return;

        // DEBUG -----------------------------
        if (Input.GetKey(KeyCode.Z))
            leftTapCount += 1;
        else if (Input.GetKey(KeyCode.M))
            rightTapCount += 1;
        //----------------------------------------


        if (gamePlayState == GameplayState.Left)
        {
            totalTapCount += leftTapCount;
            leftTapCount = 0;
        }
        else if (gamePlayState == GameplayState.Right)
        {
            totalTapCount += rightTapCount;
            rightTapCount = 0;
        }
        // In the Center state the player needs to alternate between pressing the Left and Right buttons
        else if (gamePlayState == GameplayState.Center)
        {
            if (activeButton == AcceptInputFrom.LeftBtn)
            {
                totalTapCount += leftTapCount;

                // only switch to getting input from the right button if the left button was pressed
                if (leftTapCount > 0)
                    activeButton = AcceptInputFrom.RightBtn;
            }
            else
            {
                totalTapCount += rightTapCount;

                if (rightTapCount > 0)
                    activeButton = AcceptInputFrom.LeftBtn;
            }

            leftTapCount = 0;
            rightTapCount = 0;
        }

        SendTapInput();
        ClearScore();
    }

    private void LeftBtnPress()
    {
        leftTapCount++;
    }

    private void RightBtnPress()
    {
        rightTapCount++;
    }

    protected override void ClearScore()
    {
        totalTapCount = 0;
        leftTapCount = 0;
        rightTapCount = 0;
    }

    protected override void EventManager_OnGameOver(PlayerSide obj) => activeButton = AcceptInputFrom.None;
}
