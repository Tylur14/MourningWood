using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] bool anyKeyToProgress = true;
    [SerializeField] bool test_ENDSCENE;
    [SerializeField] string levelToLoad;
    [SerializeField] float timeToLoad;

    [SerializeField] GameObject transitionEffect;

    [SerializeField] AudioSource sfx;
    void Update()
    {
        if (Input.anyKeyDown && anyKeyToProgress)
            LoadNextLevel();

        if (test_ENDSCENE)
            Cursor.lockState = CursorLockMode.Confined;
    }
    public void LoadNextLevel()
    {
        if (sfx != null)
            sfx.Play();

        if(transitionEffect != null)
            Instantiate(transitionEffect, new Vector3(0, -15, 0),Quaternion.identity);

        StartCoroutine(loadLevel());
        IEnumerator loadLevel()
        {
            yield return new WaitForSeconds(timeToLoad);
            SceneManager.LoadScene(levelToLoad);
        }
        
    }

    public void QuitGame() // PLACEHOLDER - FOR GAME JAM VERSION ONLY
    {
        Application.Quit();
    }
}
