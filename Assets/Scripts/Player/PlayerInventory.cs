using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool[] hasKeys; // 0 - Gold Key, 1 - Silver Key, 2 - Bone Key
    [Header("Ammo")]
    public int a_shotgunShells;
    public int a_bullets;
    public int a_termites;

    [Header("Weapons")]
    [SerializeField] GameObject[] allWeapons; // 0 - Axe, 1 - Chainsaw, 2 - Shotgun, 3 - SMG, 4 - Termites
    public GameObject[] acquiredWeapons;      // 0 - Axe, 1 - Chainsaw, 2 - Shotgun, 3 - SMG, 4 - Termites
    
    [Header("Current Weapon")]
    [SerializeField] GameObject currentWeapon;
    [SerializeField] Transform currentWepHolder;
    public int currentWepIndex = 0;

    [Header("Inventory State")]
    public bool canSwapWeapons;
    private void Start()
    {
        ChangeWeapon(0);
    }

    #region Inventory Management Inputs
    private void Update()
    {
        if (Input.GetButtonDown("Fire2") && canSwapWeapons)
            ChangeWeapon();
    }
    #endregion

    public void AcquireWeapon(int weaponID)
    {
        if (acquiredWeapons[weaponID] == null)
        {
            acquiredWeapons[weaponID] = allWeapons[weaponID];
            FindObjectOfType<PlayerGUIController>().HightlightWeaponNumber(weaponID);
        }
    }
    public void AcquireAmmo(int weaponID, int addAmount)
    {
        switch (weaponID)
        {
            case 2: // Shotgun Ammo
                a_shotgunShells += addAmount;
                break;
            case 3: // SMG Ammo
                a_bullets += addAmount;
                break;
            case 4: // MORE TERMITES, MORE TERMITES!
                a_termites += addAmount;
                break;
        }
        if (currentWepIndex == weaponID)
            UpdateWeaponAmmo(weaponID);
    }
    public void AcquireKey(int keyID)
    {
        if (hasKeys[keyID] == false)
            hasKeys[keyID] = true;
        FindObjectOfType<PlayerGUIController>().UpdateKeysDisplay();
    }

    void ChangeWeapon()
    {
        currentWepIndex += 1;
        if (currentWepIndex > allWeapons.Length - 1)
            currentWepIndex = 0;
        if (acquiredWeapons[currentWepIndex] != null)
        {
            if(currentWeapon!=null)
                Destroy(currentWeapon);
            currentWeapon = Instantiate(acquiredWeapons[currentWepIndex], currentWepHolder);
            UpdateWeaponAmmo(currentWepIndex);
            FindObjectOfType<PlayerGUIController>().HighlightWeaponSlot(currentWepIndex);
            canSwapWeapons = true;
        }
        else if(acquiredWeapons[currentWepIndex] == null)
        {
            ChangeWeapon();
        }
    }

    void ChangeWeapon(int weaponIndex)
    {
        if(acquiredWeapons[weaponIndex] != null)
        {
            if (currentWeapon != null)
                Destroy(currentWeapon);
            currentWeapon = Instantiate(acquiredWeapons[weaponIndex], currentWepHolder);
            currentWepIndex = weaponIndex;
            UpdateWeaponAmmo(currentWepIndex);
            FindObjectOfType<PlayerGUIController>().HighlightWeaponSlot(weaponIndex);
            canSwapWeapons = true;
        }
    }

    void UpdateWeaponAmmo(int weaponIndex)
    {
        FindObjectOfType<PlayerGUIController>().ToggleAmmoText();
        switch (weaponIndex)
        {
            case 2: // Shotgun Ammo
                FindObjectOfType<PlayerGUIController>().UpdateAmmoDisplay(a_shotgunShells);
                break;
            case 3: // SMG Ammo
                FindObjectOfType<PlayerGUIController>().UpdateAmmoDisplay(a_bullets);
                break;
            case 4: // MORE TERMITES, MORE TERMITES!
                FindObjectOfType<PlayerGUIController>().UpdateAmmoDisplay(a_termites);
                break;
            default:
                FindObjectOfType<PlayerGUIController>().ToggleAmmoText(false);
                break;
        }
    }

    public bool CanUseWeapon(int weaponID)
    {
        print(weaponID);
        switch (weaponID)
        {
            case 2: // Shotgun
                if (a_shotgunShells > 0)
                { a_shotgunShells -= 1; FindObjectOfType<PlayerGUIController>().UpdateAmmoDisplay(a_shotgunShells); return true; }
                else return false;
            case 3: // SMG
                if (a_bullets > 0)
                { a_bullets -= 1; FindObjectOfType<PlayerGUIController>().UpdateAmmoDisplay(a_bullets); return true; }
                else return false;
            case 4: // TERMITES!
                if (a_termites > 0)
                { a_termites -= 1; FindObjectOfType<PlayerGUIController>().UpdateAmmoDisplay(a_termites); return true; }
                else return false;
        }
        return true;
    }

    void Save()
    {
        // Save current inventory for next scene
    }
    private void Load()
    {
        // Load in player inventory OR use default settings if temp data is not found
        // IF it uses default data, save this default data to temp
    }
    void Delete()
    {
        // Remove temp inventory data during game over
    }
}
