using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button tutorialBtn;
    [SerializeField] private Button difficultyBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartBtnPress);
        startBtn.onClick.AddListener(TutorialBtnPress);
        startBtn.onClick.AddListener(DifficultyBtnPress);
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
        tutorialBtn.onClick.RemoveAllListeners();
        difficultyBtn.onClick.RemoveAllListeners();
    }

    private void StartBtnPress()
    {
       SceneTransitionManager.Instance.SwitchScene(SceneStates.GameScene);
    }

    private void TutorialBtnPress()
    {
    }

    private void DifficultyBtnPress()
    {
    }
}
