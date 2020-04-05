using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    // The character controller that allows the player to move about
    CharacterController characterController; 

    [Header("Movement")]
    [SerializeField] float gravity = 20.0f; // How many gravitys are you on my dude?
    [SerializeField] float moveSpeed; // The current movement speed
    [SerializeField] float walkSpeed; // How fast the player should go  when 'walking'
    [SerializeField] float runSpeed; // How fast the player should go when 'running'

    [Header("Rotation")]
    public float rotSpeed = 100.0f; // How quickly the player turns
    Vector2 yawLimit = new Vector2(-360, 360);
    public bool inverted = false; // Is the camera controls inverted?


    public void ToggleRun(bool state)
    {
        if (state)
            moveSpeed = walkSpeed;
        else if (!state)
            moveSpeed = runSpeed;
    }
    public virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
    } 
    public void Move(Vector2 moveDirection)
    {
        Vector2 newDir = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed); // Apply momentum
        Vector3 move = transform.right * newDir.x + transform.forward * newDir.y; // Set it as the facing direction of the player

        move.y -= gravity; // Apply gravity
       
        characterController.Move(move * Time.deltaTime); // Apply movement on player
    }
    public void Rotate(float yaw)
    {
        //Simple inverter for more camera controls customization
        var i = -1;
        if (inverted)
            i = 1;

        var rot = this.transform.eulerAngles; // Get reference to gameObject's rotation information.

        // Handles Yaw Input
        rot.y += yaw * Time.deltaTime * rotSpeed * -i;
        rot.y = Mathf.Clamp(rot.y, yawLimit.x, yawLimit.y);

        this.transform.eulerAngles = rot; // Set the gameObject's rotation information.
    }
}
