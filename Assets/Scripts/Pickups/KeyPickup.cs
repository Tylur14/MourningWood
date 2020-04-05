using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tyler J. Sims
// Key pickup script for Fire Away, DEC 12 2019

public class KeyPickup : Pickup
{
    [Header("Key ID")]
    [Range(0,2)]
    public int assocdKeyID; // 0 = gold key, 1 = silver key, 2 = bone key 
    public Sprite[] assocdSprite;
    
    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = assocdSprite[assocdKeyID];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInventory>().AcquireKey(assocdKeyID);
            Hide();
        }
    }
}
