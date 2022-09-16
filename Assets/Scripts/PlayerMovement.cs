using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    private Vector3 moveDirection;
    private Vector3 destination;
    private bool pauseAnimation;

    private void Awake()
    {
        pauseAnimation = true;
    }

    public void MovePlayer(PlayerSide location, GamePlayState gamePlayState)
    {
        destination = GamePlayArea.Instance.GetTransformForAnimation(location, gamePlayState);

        if (transform.position.x > destination.x)
            moveDirection = Vector3.left;
        else
            moveDirection = Vector3.right;

        // start animating
        pauseAnimation = false;
        EventManager.AnimationStarted();
    }

    private void FixedUpdate()
    {
        if (pauseAnimation)
            return;

        // Animate the player moving   
        transform.Translate(moveDirection * Time.deltaTime * speed);

        // reached our destination (or atleast very close)
        if (Vector3.Distance(transform.position, destination) < 0.2f)
        {
            transform.position = destination;
            pauseAnimation = true;
            EventManager.AnimationEnded();
        }

    }
}
