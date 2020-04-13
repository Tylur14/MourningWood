using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    [SerializeField] float timeToLoad;

    [SerializeField] GameObject transitionEffect;

    [SerializeField] AudioSource sfx;
    void Update()
    {
        if (Input.anyKeyDown)
            LoadNextLevel();
    }
    void LoadNextLevel()
    {
        if (sfx != null)
            sfx.Play();

        Instantiate(transitionEffect, new Vector3(0, -15, 0),Quaternion.identity);

        StartCoroutine(loadLevel());
        IEnumerator loadLevel()
        {
            yield return new WaitForSeconds(timeToLoad);
            SceneManager.LoadScene(levelToLoad);
        }
        
    }
}
