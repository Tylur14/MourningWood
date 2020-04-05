using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Tyler J. Sims
// Jukebox controller for Fire Away, DEC 14 2019
public class Jukebox : MonoBehaviour
{
    [Header("Jukebox Settings")]
    [Range(0.0f,1.0f)]
    public float minVolume, maxVolume; // Will add support for player settings later
    public float trackChangeSpeed = 1.0f;

    [Header("Jukebox Information")]
    public bool changingTracks = false;
    public AudioClip nextTrack;
    public float currentVolume;
    private AudioSource aSrc;

    void Start()
    {
        aSrc = GetComponent<AudioSource>();
        currentVolume = aSrc.volume;
        nextTrack = aSrc.clip;
    }

    void Update()
    {

        if(changingTracks && CheckTrack())
        {
            if (aSrc.volume < maxVolume)
            {
                aSrc.volume += Time.deltaTime * trackChangeSpeed;
                if (!aSrc.isPlaying)
                {
                    aSrc.Play();
                    print(aSrc.isPlaying);
                }
                    
                
            }
            else changingTracks = false;
            currentVolume = aSrc.volume;
        }

        else if (changingTracks && aSrc.volume >= minVolume && !CheckTrack())
        {
            if(aSrc.volume <= minVolume)
            {
                aSrc.clip = nextTrack;
            }
            else
            {
                aSrc.volume -= Time.deltaTime * trackChangeSpeed;
            }
            currentVolume = aSrc.volume;
        }
    }

    public bool CheckTrack()
    {
        if (nextTrack == aSrc.clip)
            return true;
        else
            return false;
    }

    public void ChangeTrack(AudioClip track)
    {
        changingTracks = true;
        nextTrack = track;
    }
}
