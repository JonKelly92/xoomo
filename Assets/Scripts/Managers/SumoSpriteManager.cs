using UnityEngine;

public class SumoSpriteManager : MonoBehaviour
{
    [SerializeField] private GameObject sumoStart;
    [SerializeField] private GameObject sumoCenter;
    [SerializeField] private GameObject sumoLeft;
    [SerializeField] private GameObject sumoRight;

    private void Awake()
    {
        EventManager.OnGameplayStateChanged += EventManager_OnGameplayStateChanged;

        sumoStart.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
    }

    private void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        sumoStart.SetActive(false);
        sumoCenter.SetActive(false);
        sumoLeft.SetActive(false);
        sumoRight.SetActive(false);

        switch (state)
        {
            case GameplayState.Left:
                sumoLeft.SetActive(true);
                break;
            case GameplayState.Right:
                sumoRight.SetActive(true);
                break;
            case GameplayState.Center:
                sumoCenter.SetActive(true);
                break;
            default:
                sumoStart.SetActive(true);
                break;

        }
    }
}
