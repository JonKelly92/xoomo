using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneStates
{
    MainMenu,
    Lobby,
    GameScene,
}

public class SceneTransitionManager : MonoBehaviour
{
    static public SceneTransitionManager Instance { get; internal set; }

    private const string DefaultMainMenu = "MainMenu";
    private const string Lobby = "Lobby";
    private const string GameSceneName = "GameScene";

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
            case SceneStates.GameScene:
                scenename = GameSceneName;
                break;
        }
            
        SceneManager.LoadSceneAsync(scenename);
    }
}
