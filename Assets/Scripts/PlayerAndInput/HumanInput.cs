using UnityEngine;

public class HumanInput : PlayerInput
{
    private enum AcceptInputFrom
    {
        LeftBtn = 1,
        RightBtn = 2,
        None = 3
    }

    private int _leftTapCount;
    private int _rightTapCount;
    private AcceptInputFrom _activeButton;

    protected override void Awake()
    {
        base.Awake();

        EventManager.OnLeftButtonPressed += LeftBtnPress;
        EventManager.OnRightButtonPressed += RightBtnPress;

        ClearScore();
        _activeButton = AcceptInputFrom.None;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnLeftButtonPressed -= LeftBtnPress;
        EventManager.OnRightButtonPressed -= RightBtnPress;
    }

    // Round has ended and the new game play state is being chosen, block input while this is happening
   // protected override void EventManager_OnScoreCapReached(PlayerSide obj) => _activeButton = AcceptInputFrom.None;

    // A new round is starting
    protected override void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        ClearScore();

        GamePlayState = state;

        // Setting the state now so we can immediately start recieving input
        switch (GamePlayState)
        {
            case GameplayState.Left:
                _activeButton = AcceptInputFrom.LeftBtn;
                break;
            case GameplayState.Right:
                _activeButton = AcceptInputFrom.RightBtn;
                break;
            case GameplayState.Center:
                _activeButton = AcceptInputFrom.LeftBtn;
                break;
            default:
                _activeButton = AcceptInputFrom.None;
                break;

        }
    }

    private void FixedUpdate()
    {
        // block all input
        if (_activeButton == AcceptInputFrom.None)
            return;

        // DEBUG -----------------------------
        //if (Input.GetKey(KeyCode.Z))
        //    leftTapCount += 1;
        //else if (Input.GetKey(KeyCode.M))
        //    rightTapCount += 1;
        //----------------------------------------


        if (GamePlayState == GameplayState.Left)
        {
            TotalTapCount += _leftTapCount;
            _leftTapCount = 0;
        }
        else if (GamePlayState == GameplayState.Right)
        {
            TotalTapCount += _rightTapCount;
            _rightTapCount = 0;
        }
        // In the Center state the player needs to alternate between pressing the Left and Right buttons
        else if (GamePlayState == GameplayState.Center)
        {
            if (_activeButton == AcceptInputFrom.LeftBtn)
            {
                TotalTapCount += _leftTapCount;

                // only switch to getting input from the right button if the left button was pressed
                if (_leftTapCount > 0)
                    _activeButton = AcceptInputFrom.RightBtn;
            }
            else
            {
                TotalTapCount += _rightTapCount;

                if (_rightTapCount > 0)
                    _activeButton = AcceptInputFrom.LeftBtn;
            }

            _leftTapCount = 0;
            _rightTapCount = 0;
        }

        if (TotalTapCount > 0)
        {
            SendTapInput();
            ClearScore();
        }
    }

    protected override void SendTapInput() => PlayerObject.SendTapCountEvent(TotalTapCount);

    private void LeftBtnPress()
    {
        _leftTapCount++;
    }

    private void RightBtnPress()
    {
        _rightTapCount++;
    }

    protected override void ClearScore()
    {
        TotalTapCount = 0;
        _leftTapCount = 0;
        _rightTapCount = 0;
    }

    protected override void EventManager_OnGameOver(PlayerSide obj) => _activeButton = AcceptInputFrom.None;
}
