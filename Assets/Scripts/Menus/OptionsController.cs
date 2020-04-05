using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    [Header("Graphics Options")]
    public TMP_Dropdown screenResolutionOptions; // DROPDOWN ITEM TO CONTROL SCREEN RESOLUTION
    private List<Resolution> validResList = new List<Resolution>(); // LIST OF ALL VALID SCREEN RESOLUTIONS
    public Toggle fullScreenToggle;
    public bool canToggle = false;
    public bool inFullscreen; // IS GAME IN FULLSCREEN?

    [Header("Volume Sliders")]
    [SerializeField] Slider Vol_MasterSlider;
    [SerializeField] Slider Vol_MusicSlider;
    [SerializeField] Slider Vol_SFXSlider;
    
    public AudioMixer mixer;

    [Header("Misc")]
    public bool atMainMenu = false;
    public GameObject mainMenu;
    void Start()
    {
        Time.timeScale = 0.0f;
        GetValidResolutions();

        canToggle = false; // Prevents a weird issue with the fullscreen toggling when spawned
        inFullscreen = Screen.fullScreen;
        fullScreenToggle.isOn = inFullscreen;
        canToggle = true;

        InitVolume();
        
    }

    void Update()
    {
        // CHECK TO CLOSE OUT THE MENU
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseMenu();
    }

    #region Resolution Functions
    // CHECK MONITOR AND GET ALL VALID SCREEN RESOLUTIONS
    void GetValidResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            validResList.Add(res); // Add to list of resolutions
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(); // Create new 'OptionData' to populate dropdown list
            data.text = res.ToString(); // Get resolution text
            screenResolutionOptions.options.Add(data); // Set dropdown text

            // CHECK IF RES IS EQUAL TO CURRENT SCREEN RESOLUTION, IF SO THEN MARK THE VALUE IN THE DROPDOWN
            if (res.ToString() == Screen.currentResolution.ToString())
            {
                screenResolutionOptions.value = screenResolutionOptions.options.Count - 1;
            }
        }
    }

    // THIS RUNS EVERYTIME THE VALUE CHANGES IN THE 'screenResolutionOptions' DROPDOWN ITEM
    public void CheckResDropDownInput()
    {
        SetResolution(screenResolutionOptions.value); // BY DEFAULT THIS SHOULD MATCH UP WITH AN EXISTING INDEX
    }

    // SET RESOLUTION BASED ON USER SELECTION
    void SetResolution(int resIndex)
    {
        Resolution r = validResList[resIndex];
        Screen.SetResolution(r.width, r.height, inFullscreen);

    }
    #endregion

    #region Volume Functions
    void InitVolume()
    {
        if (!PlayerPrefs.HasKey("Master"))
            PlayerPrefs.SetFloat("Master", 1.0f);
        Vol_MasterSlider.value = PlayerPrefs.GetFloat("Master");
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 1.0f);
        Vol_MusicSlider.value = PlayerPrefs.GetFloat("Music");

        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 1.0f);
        Vol_SFXSlider.value = PlayerPrefs.GetFloat("SFX");
    }

    public void SetVolume(Slider input)
    {
        mixer.SetFloat(input.gameObject.name, input.value);
        if (input.value <= -29)
            mixer.SetFloat(input.gameObject.name, -80.0f);
        PlayerPrefs.SetFloat(input.gameObject.name, input.value);
    }
    #endregion

    // TOGGLE FULLSCREEN -- Duh
    public void ToggleFullScreen()
    {
        if (canToggle)
        {
            inFullscreen = !inFullscreen;
            if (inFullscreen)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else if (!inFullscreen)
                Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    // Player has selected to return to the main menu
    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // CLOSES OPTIONS MENU
    public void CloseMenu()
    {
        Time.timeScale = 1.0f;
        if (atMainMenu)
            Instantiate(mainMenu);
        Destroy(this.gameObject);
    }
}
