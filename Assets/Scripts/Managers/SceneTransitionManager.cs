using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneStates
{
    MainMenu,
    Lobby,
    SinglePlayerGame,
    MultiplayerGame
}


public class SceneTransitionManager : NetworkBehaviour
{
    static public SceneTransitionManager Instance { get; internal set; }

    private const string DefaultMainMenu = "MainMenu";
    private const string Lobby = "Lobby";
    private const string SinglePlayerSceneName = "GameScene";
    private const string MultiPlayerSceneName = "NetworkGameScene";

    private int numberOfClientLoaded;

    private void Awake()
    {
        if (Instance != null)
            GameObject.Destroy(Instance.gameObject);

        Instance = this;

        DontDestroyOnLoad(this);

        EventManager.OnServerStarted += EventManager_OnServerStarted;
    }

    private void EventManager_OnServerStarted()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    }

    public void SwitchScene(SceneStates sceneState)
    {
        string scenename = DefaultMainMenu;

        switch (sceneState)
        {
            case SceneStates.MainMenu:
                scenename = DefaultMainMenu;
                break;
            case SceneStates.Lobby:
                scenename = Lobby;
                break;
            case SceneStates.SinglePlayerGame:
                scenename = SinglePlayerSceneName;
                break;
            case SceneStates.MultiplayerGame:
                scenename = MultiPlayerSceneName;
                break;
        }

        if (NetworkManager.Singleton.IsListening)
        {
            numberOfClientLoaded = 0;
            NetworkManager.Singleton.SceneManager.LoadScene(scenename, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync(scenename);
        }
    }

    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        numberOfClientLoaded += 1;
        EventManager.ClientLoadedScene(clientId);

        // DEBUG ---------------------------------
        Debug.Log("Player loaded scene : " + clientId);
    }

    public bool AllClientsAreLoaded()
    {
        return numberOfClientLoaded == NetworkManager.Singleton.ConnectedClients.Count;
    }

    // TODO : Listen for On Server Stopped? -----------------------
    //public void ExitAndLoadMainMenu()
    //{
    //    NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
    //    SceneManager.LoadScene(DefaultMainMenu);
    //}
}
