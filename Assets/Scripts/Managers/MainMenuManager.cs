using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _difficultyBtn;

    [SerializeField] private GameObject _difficultyPanel;

    [SerializeField] private Button _easyBtn;
    [SerializeField] private Button _normalBtn;
    [SerializeField] private Button _hardBtn;

    private void Awake()
    {
        _startBtn.onClick.AddListener(StartBtnPress);
        _difficultyBtn.onClick.AddListener(DifficultyBtnPress);

        _easyBtn.onClick.AddListener(EasySelected);
        _normalBtn.onClick.AddListener(NormalSelected);
        _hardBtn.onClick.AddListener(HardSelected);
    }

    private void OnDestroy()
    {
        _startBtn.onClick.RemoveAllListeners();
        _difficultyBtn.onClick.RemoveAllListeners();

        _easyBtn.onClick.RemoveAllListeners();
        _normalBtn.onClick.RemoveAllListeners();
        _hardBtn.onClick.RemoveAllListeners();
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
                _easyBtn.interactable = false;
                break;
            case AIDifficulty.Normal:
                _normalBtn.interactable = false;
                break;
            case AIDifficulty.Hard:
                _hardBtn.interactable = false;
                break;
        }

        _difficultyPanel.SetActive(true);
    }

    private void EasySelected() => AIDifficultyManager.Instance.SetDifficulty(AIDifficulty.Easy);
    private void NormalSelected() => AIDifficultyManager.Instance.SetDifficulty(AIDifficulty.Normal);
    private void HardSelected() => AIDifficultyManager.Instance.SetDifficulty(AIDifficulty.Hard);
}
