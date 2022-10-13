using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiplayerButton;

    private void Awake()
    {
        singlePlayerButton.onClick.AddListener(SinglePlayerButtonPress);
        multiplayerButton.onClick.AddListener(MultiPlayerButtonPress);
    }

    private void OnDestroy()
    {
        singlePlayerButton.onClick.RemoveAllListeners();
        multiplayerButton.onClick.RemoveAllListeners();
    }

    private void SinglePlayerButtonPress()
    {
       SceneTransitionManager.Instance.SwitchScene(SceneStates.SinglePlayerGame);
    }

    private void MultiPlayerButtonPress()
    {
        SceneTransitionManager.Instance.SwitchScene(SceneStates.MultiplayerGame);
    }
}
