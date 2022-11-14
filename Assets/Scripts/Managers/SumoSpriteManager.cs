using UnityEngine;

public class SumoSpriteManager : MonoBehaviour
{
    [SerializeField] private GameObject _sumoStart;
    [SerializeField] private GameObject _sumoCenter;
    [SerializeField] private GameObject _sumoLeft;
    [SerializeField] private GameObject _sumoRight;

    private void Awake()
    {
        EventManager.OnGameplayStateChanged += EventManager_OnGameplayStateChanged;

        _sumoStart.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.OnGameplayStateChanged -= EventManager_OnGameplayStateChanged;
    }

    private void EventManager_OnGameplayStateChanged(GameplayState state)
    {
        _sumoStart.SetActive(false);
        _sumoCenter.SetActive(false);
        _sumoLeft.SetActive(false);
        _sumoRight.SetActive(false);

        switch (state)
        {
            case GameplayState.Left:
                _sumoLeft.SetActive(true);
                break;
            case GameplayState.Right:
                _sumoRight.SetActive(true);
                break;
            case GameplayState.Center:
                _sumoCenter.SetActive(true);
                break;
            default:
                _sumoStart.SetActive(true);
                break;

        }
    }
}
