
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

        EventManager.OnGamePlayStateChanged += EventManager_OnGamePlayStateChanged;
        EventManager.OnLeftButtonPressed += LeftBtnPress;
        EventManager.OnRightButtonPressed += RightBtnPress;

        ClearScore();
        activeButton = AcceptInputFrom.None;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnGamePlayStateChanged -= EventManager_OnGamePlayStateChanged;
        EventManager.OnLeftButtonPressed -= LeftBtnPress;
        EventManager.OnRightButtonPressed -= RightBtnPress;
    }

    // Round has ended and the new game play state is being chosen, block input while this is happening
    protected override void EventManager_OnScoreCapReached(PlayerSide obj) => activeButton = AcceptInputFrom.None;

    private void EventManager_OnGamePlayStateChanged(GamePlayState state) => gamePlayState = state;

    // A new round is starting
    protected override void EventManager_OnGamePlayStateChangeCompleted()
    {
        ClearScore();

        // Setting the state now so we can immediately start recieving input
        switch (gamePlayState)
        {
            case GamePlayState.Left:
                activeButton = AcceptInputFrom.LeftBtn;
                break;
            case GamePlayState.Right:
                activeButton = AcceptInputFrom.RightBtn;
                break;
            case GamePlayState.Center:
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

        if (gamePlayState == GamePlayState.Left)
        {
            totalTapCount += leftTapCount;
            leftTapCount = 0;
        }
        else if (gamePlayState == GamePlayState.Right)
        {
            totalTapCount += rightTapCount;
            rightTapCount = 0;
        }
        // In the Center state the player needs to alternate between pressing the Left and Right buttons
        else if (gamePlayState == GamePlayState.Center)
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

        frameCount++;

        if (frameCount >= waitForScoreUpdate)
        {
            SendTapInput();
            ClearScore();
        }
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
        frameCount = 0;
        totalTapCount = 0;
        leftTapCount = 0;
        rightTapCount = 0;
    }
}
