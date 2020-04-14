using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Tyler J. Sims
// Input handler for Fire Away, DEC 04 2019

public class PlayerInput : PlayerMotor
{
    [Header("Input")]
    public Vector2 input;
    public float yawInput;

    GameController gameController;
    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !gameController.gameIsPaused && !gameController.gameOver)
            gameController.PauseGame();
        #region Player Movement Inputs
        // Gets movement input
        input.y = Input.GetAxisRaw("Vertical");
        input.x = Input.GetAxisRaw("Horizontal");

        // Gets Yaw Input
        yawInput = Input.GetAxis("Mouse X");
        if (Input.GetAxis("AltLookYaw") != 0)
            yawInput = Input.GetAxis("AltLookYaw");

        // Check if the player is running
        if (Input.GetButtonDown("Run") || Input.GetAxisRaw("Run") != 0)
        {
            ToggleRun(false);
        }
        else if (Input.GetButtonUp("Run") || Input.GetAxisRaw("Run") == 0)
            ToggleRun(true);
        #endregion
    }
    private void FixedUpdate()
    {
        if (characterController.velocity != Vector3.zero)
            PlayWalkSFX();
        else if (characterController.velocity == Vector3.zero && walkSrc.isPlaying)
            walkSrc.Stop();

        if (characterController.velocity != Vector3.zero)
            print(characterController.velocity);

        if (!gameController.gameIsPaused)
        {
            Move(new Vector2(input.x, input.y)); // Move player
            Rotate(yawInput); // Rotate player
            Cursor.visible = false; // Make sure the player cursor is not visible during normal play
            Cursor.lockState = CursorLockMode.Locked; // Makesure the player cursor cannot leave the window during normal play
        }
    }
    
}
