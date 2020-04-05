using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // temp

public class DirectionalAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] Sprite[] animationSprites;
    [SerializeField] float animateTime;

    [Header("Animation Status")]
    public Vector2 facingDir;
    [SerializeField] float degrees;
    [SerializeField] int frameIndex;
    [SerializeField] float timer;
    
    [Header("References")]
    [SerializeField] Transform parent;
    Transform player;

    [Header("WIP")]
    [SerializeField] int shiftLong = 8; // Frame long shift - increments of 8 or however many directions are being used
    [SerializeField] int shiftMod = 0;   // Directional short shift - increments by 1 until reaching animation loop end

    [SerializeField] bool autoTargetPlayerOnStart = false;

    private void Start()
    {
        timer = animateTime;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (autoTargetPlayerOnStart)
        {
            var dir = (player.position - transform.position).normalized;

            dir.x = GetMixRoundedFloat(dir.x);
            dir.z = GetMixRoundedFloat(dir.z) * -1;

            facingDir.x = dir.z;
            facingDir.y = dir.x;
        }
    }

    private void FixedUpdate()
    {
        transform.LookAt(player);
        GetFacingDirection();
        
        if (timer > 0)
            timer -= Time.deltaTime;
        else Animate(animationSprites);

    }

    void Animate(Sprite[] state)
    {
        timer = animateTime;
        frameIndex++;
        if (frameIndex > state.Length / 8 - 1)
            frameIndex = 0;
        GetComponent<SpriteRenderer>().sprite = state[(frameIndex * shiftLong) + shiftMod];
    }

    public void SetTargetDirection(Vector3 tar)
    {
        facingDir.x = tar.z;
        facingDir.y = tar.x;
    }

    void FindFacingDirection()
    {

        if (degrees >= 22.5f && degrees < 67.5f)        // South-East
            shiftMod = 1;
        else if (degrees >= 67.5f && degrees < 112.5f)  // East
            shiftMod = 2;
        else if (degrees >= 112.5f && degrees < 157.5f) // North-East
            shiftMod = 3;
        else if (degrees >= 157.5f && degrees < 202.5f) // North
            shiftMod = 4;
        else if (degrees >= 202.5f && degrees < 247.5f) // North-West
            shiftMod = 5;
        else if (degrees >= 247.5f && degrees < 292.5f) // West
            shiftMod = 6;
        else if (degrees >= 292.5f && degrees < 337.5f) // South-West
            shiftMod = 7;
        else shiftMod = 0;                              // South

        if (facingDir == new Vector2(1, -1))        // South-East
            shiftMod += 7;

        else if (facingDir == new Vector2(1, 0))   // East
            shiftMod += 6;

        else if (facingDir == new Vector2(1, 1))    // North-East
            shiftMod += 5;

        else if (facingDir == new Vector2(0, 1))    // North
            shiftMod += 4;

        else if (facingDir == new Vector2(-1, 1))  // North-West
            shiftMod += 3;

        else if (facingDir == new Vector2(-1, 0))   // West
            shiftMod += 2;

        else if (facingDir == new Vector2(-1, -1))   // South-West
            shiftMod += 1;

        else if (facingDir == new Vector2(0, -1))    // South
            shiftMod += 0;

        if (shiftMod > 7)
        {
            int i = shiftMod - 7;
            shiftMod = -1;
            shiftMod += i;
        }
    }
    void GetFacingDirection()
    {
        Vector2 v2 = Input.mousePosition;
        v2 = Camera.main.ScreenToWorldPoint(v2);

        // Gets a vector that points from the player's position to the target's.
        //var heading = new Vector2(parent.transform.position.x, parent.transform.position.y) - new Vector2(player.transform.position.x, player.transform.position.y);
        var heading = parent.transform.position - player.transform.position;
        //var heading = new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(parent.transform.position.x, parent.transform.position.y);
        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.
        //degrees = Mathf.Atan2(heading.x, heading.y) * Mathf.Rad2Deg - 90;
        degrees = ((Mathf.Atan2(heading.z, heading.x) / Mathf.PI) * 180f);
        if (degrees < 0) 
            degrees += 360f;

        FindFacingDirection();


    }

    int GetMixRoundedFloat(float i)
    {
        if (i > -0.45f && i < 0.45f)
            return 0;
        else if (i >= 0.46f)
            return 1;
        else if (i <= -0.46f)
            return -1;
        else return 0;
    }
}
