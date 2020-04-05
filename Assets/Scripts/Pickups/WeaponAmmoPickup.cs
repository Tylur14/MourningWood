using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoPickup : Pickup
{
    enum WeaponPickupTypes { ammo_Shotgun, ammo_SMG, ammo_Termites, wep_ChainSaw, wep_Shotgun, wep_SMG, wep_Termites}

    [Header("Pickup GFX")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] display_GFX;

    [Header("Pickup Settings")]
    [SerializeField] WeaponPickupTypes wepPickupType;
    bool isAmmoPickup = false;
    int weaponID; // 0 - Axe, 1 - Chainsaw, 2 - Shotgun, 3 - SMG, 4 - Termites
    string messageText;
    [SerializeField] int ammoPickupAmount;

    private void Start()
    {
        switch (wepPickupType)
        {
           case WeaponPickupTypes.ammo_Shotgun:
                messageText = "Picked up some shotgun ammo!";
                sr.sprite = display_GFX[0];
                isAmmoPickup = true;
                weaponID = 2;
                break;

           case WeaponPickupTypes.ammo_SMG:
                messageText ="Picked up some SMG ammo!";
                sr.sprite = display_GFX[0];
                isAmmoPickup = true;
                weaponID = 3;
                break;

           case WeaponPickupTypes.ammo_Termites:
                messageText = "Picked up some mean fuckers!";
                sr.sprite = display_GFX[0];
                isAmmoPickup = true;
                weaponID = 4;
                break;

            case WeaponPickupTypes.wep_ChainSaw:
                messageText = "FIND SOME WOOD!";
                sr.sprite = display_GFX[1];
                isAmmoPickup = false;
                weaponID = 1;
                break;

            case WeaponPickupTypes.wep_Shotgun:
                messageText = "Picked up the shotgun!";
                //messageText = "Blow them out a new hole!";
                sr.sprite = display_GFX[2];
                isAmmoPickup = false;
                weaponID = 2;
                break;

           case WeaponPickupTypes.wep_SMG:
                messageText = "Shred those bastards!";
                sr.sprite = display_GFX[3];
                isAmmoPickup = false;
                weaponID = 3;
                break;

           case WeaponPickupTypes.wep_Termites:
                messageText = "Feast on their souls!";
                sr.sprite = display_GFX[4];
                isAmmoPickup = false;
                weaponID = 4;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!isAmmoPickup)
                other.GetComponent<PlayerInventory>().AcquireWeapon(weaponID);
            if(ammoPickupAmount > 0)
                other.GetComponent<PlayerInventory>().AcquireAmmo(weaponID, ammoPickupAmount);
            GameObject.FindGameObjectWithTag("Display_GuiMessage").GetComponent<Display_GUIMessage>().DisplayWarning(messageText);
            Hide();
        }
    }
}
