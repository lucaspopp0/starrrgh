using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupShield : MonoBehaviour
{
    public bool _shielded = true;//Prevents the player from dying for one instance

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().setShield(_shielded);
            Destroy(gameObject);
        }
    }
}
