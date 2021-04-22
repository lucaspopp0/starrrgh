using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupShield : MonoBehaviour
{
    public bool _shielded = true;//Max health is 1000 hp

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
