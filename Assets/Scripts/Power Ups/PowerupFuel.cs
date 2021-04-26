using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupFuel : MonoBehaviour
{
    public float fuelTimer = 3f;//How long in seconds the player has infinite fuel

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().ObtainPowerup();
            other.gameObject.GetComponent<PlayerFuel>().InfiniteFuel(fuelTimer);
            Destroy(gameObject);
        }
    }

}
