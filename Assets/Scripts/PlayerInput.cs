using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    protected abstract void SendTiltInput(); 
    protected abstract void SendTapInput();
}
