using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public float selfDestructTime;
    private void Awake()
    {
        GetComponent<SelfDestruct>().countdownTime = selfDestructTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Do damage to enemies
        // May move to an OTS
    }
}
