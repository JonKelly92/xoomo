using UnityEngine;
using UnityEngine.UI;

public class HumanInput : PlayerInput
{
    // For Debug ------------------------
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    // ----------------------------------------

    private void Awake()
    {
        leftButton.onClick.AddListener(LeftBtnPress);
        rightButton.onClick.AddListener(RightBtnPress);

        frameCount = 0;
        tapScore = 0;
    }

    void Start()
    {
    }

    private void OnDestroy()
    {
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        frameCount++;

        if (frameCount >= waitForScoreUpdate)
        {
            SendTapInput();
            frameCount = 0;
            tapScore = 0;
        }
    }

    private void LeftBtnPress()
    {
        tapScore++;
    }

    private void RightBtnPress()
    {
        tapScore--;
    }

    protected override void SendTiltInput()
    {
        // ScoreManager.Instance.UpdateTiltScore();
    }
    protected override void SendTapInput()
    {
        ScoreManager.Instance.UpdateTapScore(tapScore, gameObject);
    }
}
