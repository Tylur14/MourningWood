using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerGUIController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] PlayerInventory _playerInventory;

    [Header("UI References")]
    // -Section: ammo-
    [SerializeField] TextMeshProUGUI currentAmmoCount;  // The display for how much ammo the player has for the currently selected weapon 
                                                        //  |-> This will display nothing for the Axe which has essentially unlimited ammo (swings)
    [SerializeField] GameObject ammoText;               // This is hidden when the current weapon does not use ammo
    
    // -Section: weapons-
    [SerializeField] TextMeshProUGUI[] weaponSlotNumbers; // The number that will be highlighted when the associated weapon is in the player's inventory
    [SerializeField] Image[] weaponSlotDisplays;          // The image that will be changed out with a pressed or notpressed icon depending on what weapon is selected

    // -Section: keys-
    [SerializeField] Image[] keyDisplays;              // This will show which keys the player has acquired

    [Header("Images")]
    [SerializeField] Sprite[] weaponSlotGFX;
    private void Start()
    {
        UpdateKeysDisplay();
    }

    public void UpdateKeysDisplay()
    {
        for (int i = 0; i < _playerInventory.hasKeys.Length; i++)
            if (_playerInventory.hasKeys[i])
                keyDisplays[i].enabled = true;
            else if(!_playerInventory.hasKeys[i])
                keyDisplays[i].enabled = false;
    }

    public void HighlightWeaponSlot(int slotIndex)
    {
        for (int i = 0; i < weaponSlotDisplays.Length; i++)
            if (i == slotIndex)
                weaponSlotDisplays[i].sprite = weaponSlotGFX[1]; // Set to 'selected' sprite
            else if (i != slotIndex)
                weaponSlotDisplays[i].sprite = weaponSlotGFX[0]; // Set to 'not selected' sprite
    }


    // This next function should only trigger once when picking up a new weapon
    public void HightlightWeaponNumber(int numIndex) // Not to be confused with HightlightWeaponSlot which gets called whenever a weapon changes
    {
        weaponSlotNumbers[numIndex].color = Color.white;
    }

    public void UpdateAmmoDisplay(int amountToDisplay)
    {
        currentAmmoCount.text = amountToDisplay.ToString();
    }

    public void ToggleAmmoText(bool state = true)
    {
        currentAmmoCount.gameObject.SetActive(state);
        ammoText.SetActive(state);
    }
}
