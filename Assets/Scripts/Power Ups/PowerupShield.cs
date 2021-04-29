using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupShield : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().ObtainPowerup(Hud.PowerupId.Shield);
            other.gameObject.GetComponent<PlayerHealth>().setShield(true);
            Destroy(gameObject);
        }
    }
}
