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

    private float timeBetweenTaps_Easy = 0.4f;
    private float timeBetweenTaps_Normal = 0.25f;
    private float timeBetweenTaps_Hard = 0.1f;

    private AIDifficulty currentDifficulty;

    private string playerPrefKey = "Difficulty";

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
        int difficulty = PlayerPrefs.GetInt(playerPrefKey, 2);

        currentDifficulty = (AIDifficulty)difficulty;

        return currentDifficulty;
    }

    public void SetDifficulty (AIDifficulty difficulty)
    {
        PlayerPrefs.SetInt(playerPrefKey, (int)difficulty);
        currentDifficulty = difficulty;
    }

    /// <summary>
    /// Returns the time between each AI button press based on the current difficulty selected
    /// </summary>
    public float GetTimeBetweenTaps()
    {
        float timeBetweenTaps = 0f;

        switch (currentDifficulty)
        {
            case AIDifficulty.Easy:
                timeBetweenTaps = timeBetweenTaps_Easy;
                break;

            case AIDifficulty.Normal:
                timeBetweenTaps = timeBetweenTaps_Normal;
                break;

            case AIDifficulty.Hard:
                timeBetweenTaps = timeBetweenTaps_Hard;
                break;
        }

        return timeBetweenTaps;
    }
}