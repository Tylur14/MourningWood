using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAnimationController : MonoBehaviour
{
    [SerializeField] DirectionalAnimator[] animations;
    [SerializeField] DirectionalAnimator currentAnim;

    private void Start()
    {
        InitAnimator();
    }

    void InitAnimator() // Need better function name
    {
        foreach (DirectionalAnimator animation in animations)
            if (animation != currentAnim)
                animation.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            else if (animation == currentAnim)
                animation.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void ChangeAnimation(int a) // 0 : IDLE, 1 : MOVING, 2 : ATTACKING
    {
        if (currentAnim != animations[a])
            currentAnim = animations[a];
        InitAnimator();
    }
    public void SetAnimDirection(Vector3 tar)
    {
        foreach (DirectionalAnimator animation in animations)
        {
            animation.facingDir.x = tar.z;
            animation.facingDir.y = tar.x;
        }
            
    }
}
