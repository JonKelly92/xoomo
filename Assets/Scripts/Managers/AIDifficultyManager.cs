using UnityEngine;

public enum AIDifficulty
{
    Easy = 1,
    Normal = 2,
    Hard = 3
}

public class AIDifficultyManager : MonoBehaviour
{
    static public AIDifficultyManager Instance { get; internal set; }

    private float _timeBetweenTaps_Easy = 0.4f;
    private float _timeBetweenTaps_Normal = 0.25f;
    private float _timeBetweenTaps_Hard = 0.1f;

    private AIDifficulty _currentDifficulty;

    private const string PlayerPrefKey = "Difficulty";

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        DontDestroyOnLoad(this);

        GetDifficulty();
    }

    public AIDifficulty GetDifficulty ()
    {
        int difficulty = PlayerPrefs.GetInt(PlayerPrefKey, 2);

        _currentDifficulty = (AIDifficulty)difficulty;

        return _currentDifficulty;
    }

    public void SetDifficulty (AIDifficulty difficulty)
    {
        PlayerPrefs.SetInt(PlayerPrefKey, (int)difficulty);
        _currentDifficulty = difficulty;
    }

    /// <summary>
    /// Returns the time between each AI button press based on the current difficulty selected
    /// </summary>
    public float GetTimeBetweenTaps()
    {
        float timeBetweenTaps = 0f;

        switch (_currentDifficulty)
        {
            case AIDifficulty.Easy:
                timeBetweenTaps = _timeBetweenTaps_Easy;
                break;

            case AIDifficulty.Normal:
                timeBetweenTaps = _timeBetweenTaps_Normal;
                break;

            case AIDifficulty.Hard:
                timeBetweenTaps = _timeBetweenTaps_Hard;
                break;
        }

        return timeBetweenTaps;
    }
}