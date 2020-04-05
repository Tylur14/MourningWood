using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MenuController
{
    GameController _gameController;
    new void Start()
    {
        base.Start();
        Time.timeScale = 0.0f; 
    }

    new void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Cancel"))
            ResumeGame();
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.None;

        //jukeBox.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 5000;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _gameController.gameIsPaused = false;
        Time.timeScale = 1.0f;
    }

    #region Pause Menu Option Functions

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveGame()
    {
        
    }

    // May not be used???
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
