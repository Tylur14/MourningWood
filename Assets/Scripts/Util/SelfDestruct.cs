using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public bool timedDestroy = false; // Does this object get destroyed after a certain amount of time?
    public bool randomTime = false;
    public Vector2 ranTimeLimits;
    public float countdownTime = 0.0f; // If so how long will it take?

    void Start()
    {
        if (timedDestroy)
            Invoke("DestroySelf", countdownTime);
        else if (randomTime)
            Invoke("DestroySelf", Random.Range(ranTimeLimits.x, ranTimeLimits.y));
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
