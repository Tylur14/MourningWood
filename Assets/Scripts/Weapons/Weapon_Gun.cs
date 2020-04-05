using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Gun : Weapon
{
    
    
    [Header("Gun Settings")]
    public int shotIndex = 1; // ~ Typically 1 for a single shot & >1 for multiple bullets (such as for the shotgun) 
    public Vector2 xOutMod = new Vector2(0.0f, 0.0f); // ~ Typically 0,0 for straight shot & -0.25f, 0.25f for a random bullet spread.


    new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && fireTimer <= 0 && FindObjectOfType<PlayerInventory>().CanUseWeapon(weaponID))
            Shoot();

        if (fireTimer > 0)
        { fireTimer -= Time.deltaTime; FindObjectOfType<PlayerInventory>().canSwapWeapons = false; }
        else if(fireTimer<=0)
            FindObjectOfType<PlayerInventory>().canSwapWeapons = true;
    }
    
    public void Shoot()
    {
        print("Shooting");
        GetComponent<AudioSource>().Play();
        for (int i = 0; i < shotIndex; i++)
        {
            var dir = PickDir();

            FireOut(dir);

        }
        GetComponent<Animator>().SetTrigger("Attack");
        fireTimer = fireRate;

    }

    public Vector3 PickDir()
    {
        Vector3 modValueX = new Vector2(Random.Range(xOutMod.x, xOutMod.y), Random.Range(xOutMod.x, xOutMod.y));
        Vector3 returnValue = new Vector3((playerTransform.forward.x + modValueX.x) * 7, (playerTransform.forward.y + (modValueX.x * modValueX.y)) * 7, (playerTransform.forward.z + modValueX.y) * 7);

        return returnValue;
    }
}
