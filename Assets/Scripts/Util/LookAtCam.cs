using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tyler J. Sims
// Billboard script for Fire Away, NOV 16~ 2019

public class LookAtCam : MonoBehaviour
{
    Transform lookAtTarget;
    private void Awake()
    {
        lookAtTarget = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(lookAtTarget);
    }
}
