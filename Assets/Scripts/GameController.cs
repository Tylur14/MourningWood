using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Tyler J. Sims
// Gamecontroller for Fire Away, NOV 16~ 2019

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Level Settings")]
    public string placeholderSettings;
    [Header("Level Status")]
    public string placeholderStatus;
    [Header("Level Info")]
    public string levelName;
    
    [Header("")]
    public GameObject pauseMenu;
    public GameObject gameOverScreen;
    //public Display_GameOver displayGameOver;
    public Jukebox jukeBox;
    public bool gameIsPaused = false;
    public bool gameOver;

    public List<string> scenesInBuild = new List<string>();
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        //if (GameObject.FindGameObjectsWithTag("Barrier_DISABLE") != null)
        //{
        //    foreach (GameObject barrier in GameObject.FindGameObjectsWithTag("Barrier_Hide"))
        //    {
        //        barrier.GetComponent<MeshRenderer>().enabled = false;
        //    }
        //}
    }
    private void Update()
    {
        //if (gameOver && Input.GetKeyDown(KeyCode.Space))
        //    NextLevel();
    }
    void FixedUpdate()
    {
        if (!gameIsPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;

        jukeBox.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 1000;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //displayGameOver.GameOverScreen();
        gameOver = gameIsPaused = true;
    }

    void NextLevel()
    {
        int scn = SceneManager.GetActiveScene().buildIndex+1;
        //print(SceneManager.GetSceneAt(scn).IsValid());
        if (SceneManager.GetSceneByBuildIndex(scn-1).IsValid())
            SceneManager.LoadScene(scn);
        else QuitToMainMenu();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseGame()
    {
        if(pauseMenu != null)
        {
            Cursor.lockState = CursorLockMode.None;

            //jukeBox.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 1000;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            gameIsPaused = true;

            Instantiate(pauseMenu);
        }
        
    }
}
