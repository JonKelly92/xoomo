using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;

    private void Awake()
    {
        singlePlayerButton.onClick.AddListener(SinglePlayerButtonPress);
    }

    private void OnDestroy()
    {
        singlePlayerButton.onClick.RemoveAllListeners();
    }

    private void SinglePlayerButtonPress()
    {
       SceneTransitionManager.Instance.SwitchScene(SceneStates.GameScene);
    }
}
