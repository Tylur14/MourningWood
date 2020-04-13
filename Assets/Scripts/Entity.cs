using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Tyler J. Sims
// Entity class for Fire Away, DEC 04 2019
// Remade for Mourning Wood, FEB 09 2020

// GOING TO BE SPLIT INTO TreemonMotor AND PlayerMotor

public class Entity : MonoBehaviour
{
    [Header("Entity Settings")]
    [SerializeField] float _health;              // The current health for the entity
    public float maxHealth;                      // The starting health for the entity
    [SerializeField] AudioClip[] _audio_Hurt;    // The SFX for the entity being hurt but not killed
    [SerializeField] GameObject _hitEffect;      // Death SFX for the entity
    [SerializeField] GameObject _deathEffect;    // Death SFX for the entity
    public float _moveSpeed;

    [Header("Entity Status")]
    [SerializeField] bool dead = false;          // Is the entity dead? If so what do we want to do with it?
    private AudioSource _audioSrc;
    [SerializeField] Animator _anim;

    public void Start()
    {
        if (GetComponent<Animator>() != null)
            _anim = GetComponent<Animator>();
    }

    public void TakeDamage(float DamageTaken)
    {
        _health -= DamageTaken;
        if (_health <= 0 && dead==false) 
            Die();
        else if (_health > 0 && _audioSrc != null)
        {
            // ! - Put in seperate funciton
            int i = Random.Range(0, _audio_Hurt.Length);
            if (_audioSrc.clip != _audio_Hurt[i])
                _audioSrc.clip = _audio_Hurt[i];
            if (!_audioSrc.isPlaying)
                _audioSrc.Play();
        }
         
    }

    public void DisplayHit(Vector3 hitPos)
    {
        if(_hitEffect!=null)
            Instantiate(_hitEffect, hitPos, Quaternion.identity);
    }

    public void Die()
    {
        dead = true;
        if (_deathEffect != null)
        {
            Instantiate(_deathEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
            
        if (_anim != null)
            _anim.SetTrigger("Die");
        else
            Destroy(this.gameObject);
        Destroy(this);
    }
}
