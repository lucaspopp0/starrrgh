using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour
{
    private static float baseWaitTime = 1f;
    [SerializeField] private float waitBeforeFueling = baseWaitTime;
    private bool isWaiting = false;
    private bool isFueling = false;
    private float fuelingRate = 3f;//What how many seconds of fuel is restored per second

    private void Update()
    {
        if (isWaiting)
        {
            waitBeforeFueling -= Time.deltaTime;
        }

        if (waitBeforeFueling <= 0)
        {
            BeginFueling();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            BeginWait();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && isFueling)
        {
            other.gameObject.GetComponent<PlayerFuel>().AddFuel(fuelingRate);
        }    
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isFueling = false;
            isWaiting = false;
            waitBeforeFueling = baseWaitTime;
        }
        
    }

    private 

    void BeginFueling()
    {
        isFueling = true;
    }
    void BeginWait()
    {
        isWaiting = true;
    }
}
