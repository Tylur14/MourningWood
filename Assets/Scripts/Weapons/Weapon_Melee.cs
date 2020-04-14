using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Melee : Weapon
{
     
    [SerializeField] bool requireFireRate = true;
    
    [Header("SFX")]
    [SerializeField] AudioClip idleSFX; // Typically not required, mainly for chainsaw
    [SerializeField] AudioClip activeSFX; // SFX to play when melee weapon is used
    AudioSource _aSRC;

    
    new void Start()
    {
        base.Start();
        _aSRC = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Multi check to verify the player can use the weapon
        if (Input.GetButton("Fire1") && (fireTimer <= 0 || !requireFireRate))
            Attack();
        else if(Input.GetButtonUp("Fire1"))
            GetComponent<Animator>().SetBool("Attacking", false);

        if (fireTimer > 0)
        { fireTimer -= Time.deltaTime; FindObjectOfType<PlayerInventory>().canSwapWeapons = false; }
        else if (fireTimer <= 0)
            FindObjectOfType<PlayerInventory>().canSwapWeapons = true;

        if(idleSFX != null && !GetComponent<Animator>().GetBool("Attacking"))
            PlayAudio(idleSFX);
    }

    void Attack()
    {
        GetComponent<Animator>().SetBool("Attacking", true);
        if (activeSFX != null)
            PlayAudio(activeSFX);
        var dir = PickDir();
        fireTimer = fireRate;
        StartCoroutine(SendHit());
        IEnumerator SendHit()
        {
            yield return new WaitForSeconds(fireRate / 2);
            FireOut(dir);
        }
        
    }

    void PlayAudio(AudioClip clip)
    {
        if (_aSRC.clip != clip)
        { _aSRC.Stop(); _aSRC.clip = clip; }
        if (!_aSRC.isPlaying)
        {
            _aSRC.Play();
        }
    }

    

    public Vector3 PickDir()
    {
        Vector3 returnValue = new Vector3(playerTransform.forward.x* 7, playerTransform.forward.y * 7, playerTransform.forward.z * 7);
        return returnValue;
    }
}
