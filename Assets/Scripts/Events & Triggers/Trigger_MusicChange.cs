using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_MusicChange : MonoBehaviour
{
    public AudioClip trackIndex;

    private void OnTriggerEnter(Collider other) // This assumes two things: 1.) There is a Jukebox in the level and 2.) The track index given is valid
    {
        if (other.tag == "Player")
            FindObjectOfType<Jukebox>().ChangeTrack(trackIndex);
    }
}
