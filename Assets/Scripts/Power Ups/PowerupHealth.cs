using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupHealth : MonoBehaviour
{
    public int heathIncrease = 300;//Max health is 1000 hp

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().ObtainPowerup(Hud.PowerupId.Health);
            other.gameObject.GetComponent<PlayerHealth>().Heal(heathIncrease);
            Destroy(gameObject);
        }
    }
}
