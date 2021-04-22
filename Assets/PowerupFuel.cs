using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupFuel : MonoBehaviour
{
    public float fuelTimer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerFuel>().infiniteFuel(fuelTimer);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
