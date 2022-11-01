using Unity.Netcode;
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

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        DontDestroyOnLoad(this);

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
            NetworkManager.Singleton.SceneManager.LoadScene(scenename, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync(scenename);
        }
    }
}
