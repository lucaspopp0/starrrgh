using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupFuel : MonoBehaviour
{
    private float fuelTimer = 30f;//How long in seconds the player has infinite fuel

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerPowerup>().ObtainPowerup(Hud.PowerupId.Fuel,fuelTimer);
            Destroy(gameObject);
        }
    }

}
