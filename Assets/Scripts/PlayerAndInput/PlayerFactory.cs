using UnityEngine;

public class PlayerFactory : MonoBehaviour
{
    public static PlayerFactory Instance { get; private set; }

    [SerializeField] private PlayerObject _humanPlayerPrefab;
    [SerializeField] private PlayerObject _aiPlayerPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void CreateHumanPlayer(PlayerSide playerSide) => InstantiatePlayer(_humanPlayerPrefab, playerSide);
    public void CreateAIPlayer(PlayerSide playerSide) => InstantiatePlayer(_aiPlayerPrefab, playerSide);

    private void InstantiatePlayer(PlayerObject playerPrefab, PlayerSide playerSide)
    {
        PlayerObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        if (player != null)
            player.Location = playerSide;
    }
}
