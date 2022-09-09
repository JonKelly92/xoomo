using System.Collections;
using System.Collections.Generic;
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

        // Send event to update the UI
    }

    public void UpdateTiltScore(float score, GameObject playerObject)
    {
        PlayerValues player = IdentifyPlayer(playerObject);

        // Calculate current score

        // Calculate overall score

        // Store score in player

        // Send event to update the UI
    }

    public void UpdateTapScore(float score, GameObject playerObject)
    {
        PlayerValues player = IdentifyPlayer(playerObject);
    }


    // Finds which player sent us data so we can update their score
    private PlayerValues IdentifyPlayer(GameObject playerObject)
    {
        if (playerOne == null)
        {
            playerOne.playerGameObject = playerObject;
            return playerOne;
        }
        else if (playerTwo == null)
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
}
