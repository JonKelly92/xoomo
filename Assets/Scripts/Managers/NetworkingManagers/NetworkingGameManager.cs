using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkingGameManager : NetworkBehaviour
{
    public static NetworkingGameManager Instance { get; private set; }

    // DEBUG -----------------------------
    public TextMeshProUGUI LOG_TEXT;
    //public TextMeshProUGUI TEST_TEXT;
    public NetworkVariable<int> TEST_VAR = new NetworkVariable<int>(0);

    [SerializeField] int roundScoreCap = 130; // when a player reaches this score the round ends and the game play state is changed (swithcing from center to left/right or from left/right to center)

    [SerializeField] int gamePlayTimer = 45;

    private GameplayState currentGamePlayState;

    private int animationsInProgress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        EventManager.OnScoreCapReached += EventManager_OnScoreCapReached;
        EventManager.OnAnimationStarted += EventManager_OnAnimationStarted;
        EventManager.OnAnimationEnded += EventManager_OnAnimationEnded;
        EventManager.OnRestartGame += EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu += EventManager_OnExitToMainMenu;
        EventManager.OnGameplayTimerEnd += EventManager_OnGamePlayTimerEnd;
        EventManager.OnPreGameTimerEnd += EventManager_OnPreGameTimerEnd;

        animationsInProgress = 0;

        // DEBUG --------------------------
        Application.logMessageReceived += Application_logMessageReceived;
    }
    // DEBUG NETWORKING ------------------------------------------
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        LOG_TEXT.text += condition + Environment.NewLine;
    }

    //public void TextNetworkingVar()
    //{
    //    TEST_VAR.Value++;
    //    Debug.Log("TEST VAR : " + TEST_VAR.Value.ToString());
    //}


    // ------------------------------------------

    private void SetPlayersSide()
    {
        // TODO ------------------------------
        // The below code doens't work because only the server can access NetworkManager.Singleton.ConnectedClients

        //NetworkManager.Singleton.ConnectedClients.TryGetValue(OwnerClientId, out var client);
        //var playerObject = client.PlayerObject.GetComponent<NetworkingPlayerObject>();

        //// Assign the players to either the left or right side of the play area
        //if (IsServer)
        //    playerObject.Location = PlayerSide.Left;
        //else
        //    playerObject.Location = PlayerSide.Right;

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        SetPlayersSide();

        EventManager.PreGameTimerStart(3);
    }

    public override void OnDestroy()
    {
        EventManager.OnScoreCapReached -= EventManager_OnScoreCapReached;
        EventManager.OnAnimationStarted -= EventManager_OnAnimationStarted;
        EventManager.OnAnimationEnded -= EventManager_OnAnimationEnded;
        EventManager.OnRestartGame -= EventManager_OnRestartGame;
        EventManager.OnExitToMainMenu -= EventManager_OnExitToMainMenu;
        EventManager.OnGameplayTimerEnd -= EventManager_OnGamePlayTimerEnd;
        EventManager.OnPreGameTimerEnd -= EventManager_OnPreGameTimerEnd;

        // DEBUG --------------------------
        Application.logMessageReceived -= Application_logMessageReceived;

        base.OnDestroy();
    }

    private void EventManager_OnPreGameTimerEnd()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        EventManager.ScoreCapSet(roundScoreCap); ;
        SetGamePlayState(GameplayState.Center);
        EventManager.GamePlayTimerStart(gamePlayTimer);
    }

    private void SetGamePlayState(GameplayState state)
    {
        currentGamePlayState = state;
        EventManager.GameplayStateChanged(state);
    }

    private void EventManager_OnScoreCapReached(PlayerSide playerSide)
    {
        if (currentGamePlayState == GameplayState.Center)
        {
            if (playerSide == PlayerSide.Left)// Left player won the round so we move to the right for the next round
                SetGamePlayState(GameplayState.Right);
            else
                SetGamePlayState(GameplayState.Left);
        }
        else if (currentGamePlayState == GameplayState.Right || currentGamePlayState == GameplayState.Left)
        {
            // if the current game play state is Left (right player has advantage)
            // && the location being passed is Right (right player also won the current round)
            // then the Right player has won the match
            if (currentGamePlayState == GameplayState.Left && playerSide == PlayerSide.Right ||
                currentGamePlayState == GameplayState.Right && playerSide == PlayerSide.Left)
            {
                EventManager.GameOver(playerSide);
            }
            else
                SetGamePlayState(GameplayState.Center);
        }
    }

    private void EventManager_OnGamePlayTimerEnd()
    {
        PlayerSide playerSide = ScoreManager.Instance.GetWinnerByScore();
        EventManager.GameOver(playerSide);


        // DEBUG--------------------------------------------------------------
        Debug.Log("Winner by overall score : " + playerSide.ToString());
    }

    private void EventManager_OnAnimationStarted()
    {
        animationsInProgress++;
    }

    private void EventManager_OnAnimationEnded()
    {
        animationsInProgress--;

        if (animationsInProgress == 0)
            EventManager.GamePlayStateChangeCompleted();
        else if (animationsInProgress < 0)
            Debug.LogError("More animations have finished than were started"); // just in case, save some headaches later
    }

    private void EventManager_OnRestartGame() => SceneTransitionManager.Instance.SwitchScene(SceneStates.MultiplayerGame);

    private void EventManager_OnExitToMainMenu() => SceneTransitionManager.Instance.SwitchScene(SceneStates.MainMenu);
}
