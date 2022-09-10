using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// Holds a player's score and gameobject which is used to identify the player later
public class PlayerValues
{
    public int currentTapScore;
    public int currentTiltScore;
    public int overallScore;
    public GameObject playerGameObject;

    public PlayerValues()
    {
        currentTapScore = 0;
        currentTiltScore = 0;
        overallScore = 0;
    }
}

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; }

    private PlayerValues playerOne;
    private PlayerValues playerTwo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        playerOne = new PlayerValues();
        playerTwo = new PlayerValues();

        // Send events to update the UI
        UpdateOverallScoreEvent(playerOne);
        UpdateTapScoreEvent(playerOne);
        UpdateTiltScoreEvent(playerOne);
    }

    public void UpdateTiltScore(float score, GameObject playerObject)
    {
        PlayerValues player = IdentifyPlayer(playerObject);

        // Calculate current score

        // Calculate overall score

        // Store score in player

        // Send event to update the UI
    }

    public void UpdateTapScore(int score, GameObject playerObject)
    {
        PlayerValues player = IdentifyPlayer(playerObject);

        player.currentTapScore += (score * 5); // x5 because it looks better than a double digit score
        player.overallScore += player.currentTapScore; 

        // Send event to update the UI
        UpdateTapScoreEvent(player);
        UpdateOverallScoreEvent(player);
    }

    // Finds which player sent us data so we can update their score
    private PlayerValues IdentifyPlayer(GameObject playerObject)
    {
        if (playerOne.playerGameObject == null)
        {
            playerOne.playerGameObject = playerObject;
            return playerOne;
        }
        else if (playerTwo.playerGameObject == null)
        {
            playerTwo.playerGameObject = playerObject;
            return playerTwo;
        }
        else if (playerOne.playerGameObject == playerObject)
            return playerOne;
        else if (playerTwo.playerGameObject == playerObject)
            return playerTwo;
        else
        {
            Debug.LogError("A third player has entered the game.");
            return null;
        }
    }

    private void UpdateTapScoreEvent(PlayerValues player) => EventManager.TapScoreUpdated(player.currentTapScore);
    private void UpdateTiltScoreEvent(PlayerValues player) => EventManager.TiltScoreUpdate(player.currentTiltScore);
    private void UpdateOverallScoreEvent(PlayerValues player) => EventManager.OverallScoreUpdated(player.overallScore);
}
