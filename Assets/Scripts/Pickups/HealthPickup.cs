using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tyler J. Sims
// Health pickup script for Fire Away, DEC 07 2019

public class HealthPickup : Pickup
{
    
    public bool givePastMaxHealth = false; // This is for super health packs that can give up to 200% health
    [Range(0.0f,1.0f)]
    public float healthGiven;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            if (player.health < player.maxBaseHealth && !givePastMaxHealth)
            {
                player.health += healthGiven;

                if (player.health > player.maxBaseHealth)
                    player.health = player.maxBaseHealth;

                player.UpdateHealthDisplay();
                Hide();
            }
            else if (player.health < player.maxHealth && givePastMaxHealth)
            {
                player.health += healthGiven;

                if (player.health > player.maxHealth)
                    player.health = player.maxHealth;

                player.UpdateHealthDisplay();
                Hide();
            }
        }
    }
}
