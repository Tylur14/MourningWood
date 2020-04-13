using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    [SerializeField] float timeToLoad;
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
        StartCoroutine(loadLevel());
        IEnumerator loadLevel()
        {
            yield return new WaitForSeconds(timeToLoad);
            SceneManager.LoadScene(levelToLoad);
        }
        
    }
}
