using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private GameObject startButtonPanel;

    private bool allPlayersInLobby;
    private Dictionary<ulong, bool> clientsInLobby;
    private const int RequiredPlayers = 1; // DEBUG ---------------------- make this 2

    private void Awake()
    {
        clientsInLobby = new Dictionary<ulong, bool>();

        hostButton.onClick.AddListener(HostButtonPress);
        clientButton.onClick.AddListener(ClientButtonPress);
        readyButton.onClick.AddListener(PlayerIsReady);
    }

    public override void OnDestroy()
    {
        hostButton.onClick.RemoveAllListeners();
        clientButton.onClick.RemoveAllListeners();
        readyButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    private void HostButtonPress()
    {
        var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        // if (utpTransport) m_HostIpInput.text = "127.0.0.1";

        if (NetworkManager.Singleton.StartHost())
        {
            startButtonPanel.SetActive(false);

            clientsInLobby.Add(NetworkManager.LocalClientId, false);

            allPlayersInLobby = false;

            //Server will be notified when a client connects
            NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
            EventManager.OnClientLoadedScene += EventManager_ClientLoadedScene;

            EventManager.ServerStarted();
        }
        else
        {
            Debug.LogError("Failed to start host.");
        }
    }

    private void ClientButtonPress()
    {
        var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (utpTransport)
        {
            // DEBUG ---------------------------- unitl match making implemented
            utpTransport.SetConnectionData("127.0.0.1", 7777);
        }
        if (NetworkManager.Singleton.StartClient())
        {
            startButtonPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Failed to start client.");
        }
    }

    private void UpdateAndCheckPlayersInLobby()
    {
        allPlayersInLobby = clientsInLobby.Count >= RequiredPlayers;

        foreach (var clientLobbyStatus in clientsInLobby)
        {
            SendClientReadyStatusUpdatesClientRpc(clientLobbyStatus.Key, clientLobbyStatus.Value);
            if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientLobbyStatus.Key))

                //If some clients are still loading into the lobby scene then this is false
                allPlayersInLobby = false;
        }

        if (allPlayersInLobby)
            LoadGameScene();
    }

    private void LoadGameScene()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        EventManager.OnClientLoadedScene -= EventManager_ClientLoadedScene;

        SceneTransitionManager.Instance.SwitchScene(SceneStates.MultiplayerGame);
    }

    private void EventManager_ClientLoadedScene(ulong clientId)
    {
        if (IsServer)
        {
            if (!clientsInLobby.ContainsKey(clientId))
            {
                clientsInLobby.Add(clientId, false);
            }

            UpdateAndCheckPlayersInLobby();
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (IsServer)
        {
            UpdateAndCheckPlayersInLobby();
        }
    }

    /// <summary>
    ///     SendClientReadyStatusUpdatesClientRpc
    ///     Sent from the server to the client when a player's status is updated.
    ///     This also populates the connected clients' (excluding host) player state in the lobby
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="isReady"></param>
    [ClientRpc]
    private void SendClientReadyStatusUpdatesClientRpc(ulong clientId, bool isReady)
    {
        if (!IsServer)
        {
            if (!clientsInLobby.ContainsKey(clientId))
                clientsInLobby.Add(clientId, isReady);
            else
                clientsInLobby[clientId] = isReady;
        }
    }

    public void PlayerIsReady()
    {
        clientsInLobby[NetworkManager.Singleton.LocalClientId] = true;
        if (IsServer)
        {
            UpdateAndCheckPlayersInLobby();
        }
        else
        {
            OnClientIsReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnClientIsReadyServerRpc(ulong clientid)
    {
        if (clientsInLobby.ContainsKey(clientid))
        {
            clientsInLobby[clientid] = true;
            UpdateAndCheckPlayersInLobby();
        }
    }
}
