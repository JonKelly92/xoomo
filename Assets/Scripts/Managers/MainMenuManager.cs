using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button optionsButton;

    private const string SinglePlayerSceneName = "GameScene";
    private const string MultiPlayerSceneName = "";
    private const string OptionsPlayerSceneName = "";

    private void Awake()
    {
        singlePlayerButton.onClick.AddListener(SinglePlayerButtonPress);
        multiplayerButton.onClick.AddListener(MultiPlayerButtonPress);
        optionsButton.onClick.AddListener(OptionsButtonPress);
    }

    private void OnDestroy()
    {
        singlePlayerButton.onClick.RemoveAllListeners();
        multiplayerButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
    }

    private void SinglePlayerButtonPress()
    {
        SceneManager.LoadScene(SinglePlayerSceneName);
    }

    private void MultiPlayerButtonPress()
    {
    }

    private void OptionsButtonPress()
    {
    }
}
