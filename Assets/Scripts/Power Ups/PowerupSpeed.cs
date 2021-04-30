using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed : MonoBehaviour
{
    public float _speedUpConstant = 2f;//Speed increase of 100%
    public static float DURATION = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerPowerup>().ObtainPowerup(Hud.PowerupId.Speed,_speedUpConstant);
            Destroy(gameObject);
        }
    }
}
