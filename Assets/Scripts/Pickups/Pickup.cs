using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tyler J. Sims
// pickup base call for Fire Away, JAN 02 2020

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour
{
    AudioSource _aSrc;
    public GameObject GFX;
    bool readyToDestroy = false;

    private void Awake()
    {
        _aSrc = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //transform.Rotate(0, rotSpeed * Time.deltaTime, 0); // OLD -- but neat rotate code
        if (readyToDestroy && !_aSrc.isPlaying)
            Destroy(this.gameObject);
    }
    public void Hide()
    {
        _aSrc.Play();
        readyToDestroy = true;
        Destroy(GFX);
        GetComponent<BoxCollider>().enabled = false;
    }
}
