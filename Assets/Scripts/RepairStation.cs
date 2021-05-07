using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RepairStation : MonoBehaviour
{
    [SerializeField] private float baseWaitTime = 2f;
    private float timer;
    private float waitBeforeRepairing = 0f;
    private bool isWaiting = false;
    private bool isRepairing = false;
    [SerializeField] private int repairRate = 50;//How much HP is restored per second

    private ScoreController _scoreController;
    private void Start()
    {
        _scoreController = GameObject.Find("Score Controller").GetComponent<ScoreController>();
        waitBeforeRepairing = baseWaitTime;
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitBeforeRepairing -= Time.deltaTime;
        }

        if (waitBeforeRepairing <= 0)
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
        timer += Time.deltaTime;

        if (other.gameObject.tag == "Player" && isRepairing)
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth.GetHealth() < PlayerHealth.MAX_HEALTH && timer >= 1 && _scoreController.GetScore() > 0)
            {
                timer = 0;
                _scoreController.SubScore(25);
                playerHealth.Heal(repairRate);

            }
        }    
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isRepairing = false;
            isWaiting = false;
            waitBeforeRepairing = baseWaitTime;
        }
        
    }

    private 

        void BeginFueling()
    {
        isWaiting = false;
        isRepairing = true;
    }
    void BeginWait()
    {
        isWaiting = true;
    }
}