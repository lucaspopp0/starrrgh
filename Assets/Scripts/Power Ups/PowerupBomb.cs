using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBomb : MonoBehaviour
{
    public int bombAmount = 5;//How long in seconds the player has infinite fuel

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerPowerup>().ObtainPowerup(Hud.PowerupId.Bomb, bombAmount);
            Destroy(gameObject);
        }
    }

}
