using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject startButtonPanel;

    private const int RequiredPlayers = 2;

    private void Awake()
    {
        hostButton.onClick.AddListener(HostButtonPress);
        clientButton.onClick.AddListener(ClientButtonPress);
    }

    public override void OnDestroy()
    {
        hostButton.onClick.RemoveAllListeners();
        clientButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }

    private void HostButtonPress()
    {
        startButtonPanel.gameObject.SetActive(false);

        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    }

    private void ClientButtonPress()
    {
        startButtonPanel.gameObject.SetActive(false);

        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if(!IsServer)
            return;
        
        if (NetworkManager.Singleton.ConnectedClientsList.Count == RequiredPlayers)
            LoadGameScene();
    }

    private void LoadGameScene()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
        SceneTransitionManager.Instance.SwitchScene(SceneStates.MultiplayerGame);
    }
}
