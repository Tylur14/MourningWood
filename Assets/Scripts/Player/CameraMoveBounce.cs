using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Tyler J. Sims
// Input handler for Fire Away, DEC 05 2019
public class CameraMoveBounce : MonoBehaviour
{
    [Header("Camera Bounce Settings")] // Settings for camera bounce -- edit in Unity Inspector
    [SerializeField] Vector3 restPosition;
    [SerializeField] Vector3 downPosition; // Currently this is always the *-1 of upPosition
    [SerializeField] float speed;
    [SerializeField] float threshold;
    [SerializeField] PlayerInput pInput;

    [Header("Camera Bounce Information")] // Current status of camera bob state
    public bool goingUp;
    private Vector3 vel = Vector3.zero;

    private void FixedUpdate()
    {
        if (pInput.input.x != 0 || pInput.input.y != 0)
            PingPong();
        else
            SetRest();

    }

    void PingPong()
    {
        if (goingUp)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, restPosition,ref vel, Time.deltaTime * speed);
        else if(!goingUp)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, downPosition,ref vel, Time.deltaTime * speed);
        
        if (transform.localPosition.y > restPosition.y - threshold)
            goingUp = false;
        else if (transform.localPosition.y < downPosition.y + threshold)
            goingUp = true;
    }

    void SetRest()
    {
        if (transform.localPosition != restPosition)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, restPosition, ref vel,Time.deltaTime * speed);
    }
}
