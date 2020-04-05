using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Selection Controls")]
    [SerializeField] int selectionIndex;       // Which selection the player is currently highlighting, once submit button is pressed, this option will execute
    [SerializeField] Image[] selectionArrows;  // The GFX to indicated which selection is currently highlighted
    [SerializeField] GameObject optionsHolder; // Holds all the selections for the main menu (not to be confused with the options menu)
    [SerializeField] AudioSource sfx_SelectionChange;  // Audio to play each time the selection changes (Always the same sfx, which is why there are not audio clips for it)
    [SerializeField] AudioSource sfx_SelectionSubmit;  // Audio to play when a selection is submitted
    [SerializeField] AudioClip[] clips_SelectionSubmits;  // Audio clips to swap out in sfx_SelectionSubmit
    bool canToggleSelection;                   // To prevent the toggle getting called infinitely;

    [Header("Selection Settings")]
    [SerializeField] UnityEvent[] selectionEvents;

    [Header("Selection Status")]
    [SerializeField] bool selectionSubmitted = false;

    [Header("Spawnables")]
    [SerializeField] GameObject OptionsMenu;
    public void Start()
    {
        selectionIndex = 0;
        selectionArrows = optionsHolder.GetComponentsInChildren<Image>();
        UpdateSelectionDisplay(0, false);
    }

    public void Update()
    {
        if (!selectionSubmitted) // Prevent the player from being able to change selection and attempt to select the new option
        {
            var selectionInput = Input.GetAxisRaw("Vertical");

            if (selectionInput > 0 && canToggleSelection)
                UpdateSelectionDisplay(-1);
            else if (selectionInput < 0 && canToggleSelection)
                UpdateSelectionDisplay(1);
            else if (selectionInput == 0)
                canToggleSelection = true;

            if (Input.GetButtonDown("Submit"))
                SubmitSelection();
        }
        
    }

    // UPDATE THE SELECTIONS DISPLAY
    void UpdateSelectionDisplay(int mod, bool playSFX = true)
    {
        canToggleSelection = false; // Don't allow this to get called infinitely
        selectionIndex += mod;      // Increment the index by the passed in value

        if(playSFX)
            sfx_SelectionChange.Play(); // Play the selection change sfx

        // Verify that the index is within range
        if (selectionIndex > selectionArrows.Length - 1)
            selectionIndex = 0;
        else if (selectionIndex < 0)
            selectionIndex = selectionArrows.Length - 1;

        // Cycle through each selection graphic and disable it
        foreach (Image arrow in selectionArrows)
            arrow.enabled = false;
        // Finally, enable the selection graphic associated with the selection index
        selectionArrows[selectionIndex].enabled = true;
    }

    // SUBMIT THE HIGHLIGHTED COMMAND
    void SubmitSelection()
    {
        if(GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().Stop(); // Stop the BGM

        selectionSubmitted = true; // Make sure the player cannot navigate once a selection has been submitted

        selectionArrows[selectionIndex].color = Color.grey; // Indicated to the player that the selection has been submitted
        sfx_SelectionSubmit.clip = clips_SelectionSubmits[selectionIndex]; // Do not execute the associated command until the SFX is done playing
        sfx_SelectionSubmit.Play(); // Play the submission SFX
        StartCoroutine(CompleteAction()); // Start the coroutine to execute the associated command
        IEnumerator CompleteAction()
        {
            yield return new WaitForSeconds(sfx_SelectionSubmit.clip.length);
            selectionSubmitted = false;
            selectionArrows[selectionIndex].color = Color.white;
            selectionEvents[selectionIndex].Invoke();
        }
        
    }

    // Start New Game
    public void StartNewGame() // ? - Is there an instance where the first level would not be in build index 1? If so then fix this logic!
    {
        SceneManager.LoadScene(1);
    }

    // Spawn Options Menu
    public void InitOptionsMenu()
    {
        var optionsMenu = Instantiate(OptionsMenu);
        optionsMenu.GetComponent<OptionsController>().atMainMenu = true;
        Destroy(this.gameObject);

    }

    // Quit out of application
    public void QuitGame()
    {
        // if in editor mode, stop play
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
