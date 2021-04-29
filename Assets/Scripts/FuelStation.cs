using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour
{
    [SerializeField] private float baseWaitTime = 2f; 
    private float waitBeforeFueling = 0f;
    private bool isWaiting = false;
    private bool isFueling = false;
    [SerializeField] private float fuelingRate = 2.5f;//What how many seconds of fuel is restored per second

    private void Start()
    {
        waitBeforeFueling = baseWaitTime;
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            BeginWait();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player" && isFueling)
        {
            PlayerFuel playerFuel = other.gameObject.GetComponent<PlayerFuel>();
            if (playerFuel.GetFuel() < playerFuel.GetMaxFuel())
            {
                playerFuel.AddFuel(fuelingRate * Time.deltaTime);

            }
        }    
    }

    private void OnTriggerExit2D(Collider2D other)
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
        isWaiting = false;
        isFueling = true;
    }
    void BeginWait()
    {
        isWaiting = true;
    }
}
