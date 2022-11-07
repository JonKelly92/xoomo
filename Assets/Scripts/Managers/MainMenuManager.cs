using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button difficultyBtn;

    [SerializeField] private GameObject difficultyPanel;

    [SerializeField] private Button easyBtn;
    [SerializeField] private Button normalBtn;
    [SerializeField] private Button hardBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(StartBtnPress);
        difficultyBtn.onClick.AddListener(DifficultyBtnPress);

        easyBtn.onClick.AddListener(EasySelected);
        normalBtn.onClick.AddListener(NormalSelected);
        hardBtn.onClick.AddListener(HardSelected);
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
        difficultyBtn.onClick.RemoveAllListeners();

        easyBtn.onClick.RemoveAllListeners();
        normalBtn.onClick.RemoveAllListeners();
        hardBtn.onClick.RemoveAllListeners();
    }

    private void StartBtnPress()
    {
        SceneTransitionManager.Instance.SwitchScene(SceneStates.GameScene);
    }

    private void DifficultyBtnPress()
    {
        var difficulty = AIDifficultyManager.Instance.GetDifficulty();

        switch (difficulty)
        {
            case AIDifficulty.Easy:
                easyBtn.interactable = false;
                break;
            case AIDifficulty.Normal:
                normalBtn.interactable = false;
                break;
            case AIDifficulty.Hard:
                hardBtn.interactable = false;
                break;
        }

        difficultyPanel.SetActive(true);
    }

    private void EasySelected() => AIDifficultyManager.Instance.SetDifficulty(AIDifficulty.Easy);
    private void NormalSelected() => AIDifficultyManager.Instance.SetDifficulty(AIDifficulty.Normal);
    private void HardSelected() => AIDifficultyManager.Instance.SetDifficulty(AIDifficulty.Hard);
}
