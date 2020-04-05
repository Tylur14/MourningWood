using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Shouldn't be needed in final
// Tyler J. Sims
// Player script for Fire Away, DEC 04 2019
public class Player : MonoBehaviour
{
    [Header("Player Health Status & Settings")]
    public float health = 1.0f;
    public float maxBaseHealth = 1.0f;
    public float maxHealth = 2.0f;

    public bool isDead = false;
    public float deadPos = -1.25f;

    PlayerInput _pInput;
    AudioSource _audioSrc;
    [SerializeField] AudioClip audio_Hurt, audio_Death;

    [SerializeField] GameObject playerGUI;
    public TextMeshProUGUI healthDisplay;

    private void Start()
    {
        //healthDisplay = GameObject.FindGameObjectWithTag("Display_Health").GetComponent<TextMeshProUGUI>();
        _audioSrc = GetComponent<AudioSource>();
        _pInput = GetComponent<PlayerInput>();
    }
    public void AutoRestart()
    {
        FindObjectOfType<GameController>().RestartLevel();
    }
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            _pInput.enabled = false;
            FindObjectOfType<CameraMoveBounce>().enabled = false;
            print("Player Died!");
            PlaySFX(audio_Death, false);
            var pos = transform.position;
            this.transform.position = new Vector3(pos.x, pos.y + deadPos, pos.z);
            Invoke("AutoRestart", 2.0f);
        }
    }
    public void TakeDamage(float DamageTaken)
    {
        if (!isDead)
        {
            health -= DamageTaken;
            PlaySFX(audio_Hurt, true);
            if (health <= 0) { Die(); health = 0; }
            UpdateHealthDisplay();
        }
    }

    public void PlaySFX(AudioClip clip, bool wait)
    {
        if(_audioSrc != null)
        {
            _audioSrc.clip = clip;
            if (!wait)
            { _audioSrc.Stop(); _audioSrc.Play(); }
            else if (wait && !_audioSrc.isPlaying)
                _audioSrc.Play();
        }
            
    }

    public void UpdateHealthDisplay()
    {
        healthDisplay.text = "Health: " + (100 * health).ToString("#") + "%";
    }
}
