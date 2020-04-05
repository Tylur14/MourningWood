using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Mover : MonoBehaviour
{
    // Currently assumming all movement is relative
    [Header("Event_Mover Settings")]
    public Vector3 endRelativePos;
    public float moveSpeed = 25.0f; // The higher the number the slower the movement! This is not true for Event_Rotator!! In Event_Rotator the lower the value the slower the rotation!

    [Header("Event_Mover Information")]
    public bool moving = false;

    private AudioSource aSrc;
    private Vector3 vel = Vector3.zero;
    private void Start()
    {
        if (GetComponent<AudioSource>() != null)
            aSrc = GetComponent<AudioSource>();

        // This is so only the values of endRelativePos need to be taken into account -- otherwise the exact coords would need to be found
        endRelativePos = new Vector3(
            transform.localPosition.x + endRelativePos.x,
            transform.localPosition.y + endRelativePos.y,
            transform.localPosition.z + endRelativePos.z);
    }
    private void FixedUpdate()
    {
        if (moving && transform.localPosition != endRelativePos)
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, endRelativePos, ref vel, Time.deltaTime * moveSpeed);
        
    }

    public void PlayEvent()
    {
        moving = true;
        if (aSrc != null)
            aSrc.Play();
    }
}
