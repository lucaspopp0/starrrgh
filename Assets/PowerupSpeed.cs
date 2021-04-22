using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed : MonoBehaviour
{
    public float _speedUpConstant = 1.5f;//Speed increase of 50%

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().speedUp(_speedUpConstant);
            Destroy(gameObject);
        }
    }
}
