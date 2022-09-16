using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 destination;
    private bool pauseAnimation;

    private void Awake()
    {
        pauseAnimation = true;
    }

    public void MovePlayer (PlayerSide location, GamePlayState gamePlayState)
    {
        destination = GamePlayArea.Instance.GetTransformForAnimation(location, gamePlayState);

        // start animating
        pauseAnimation = false;
        EventManager.AnimationStarted();
    }

    private void FixedUpdate()
    {
        if (pauseAnimation)
            return;

        // TODO 

        // Animate the player moving

        // when done
        pauseAnimation = true;
        EventManager.AnimationEnded();

    }
}
