using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RepairStation : MonoBehaviour {

    private const int HEALTH_COST = 25;
    [SerializeField] private float baseWaitTime = 2f;
    private float timer;
    private float waitBeforeRepairing = 0f;
    private bool isWaiting = false;
    private bool isRepairing = false;
    [SerializeField] private int repairRate = 50;//How much HP is restored per second
    [SerializeField] private StationUI stationUI;

    private ScoreController _scoreController;
    private void Start()
    {
        _scoreController = GameObject.Find("Score Controller").GetComponent<ScoreController>();
        waitBeforeRepairing = baseWaitTime;
        UpdateUI();
    }

    private void Update()
    {
        if (isWaiting)
        {
            waitBeforeRepairing -= Time.deltaTime;
            stationUI.slider.normalizedValue = (baseWaitTime - waitBeforeRepairing + timer) / (baseWaitTime + 1f);
        }

        if (waitBeforeRepairing <= 0)
        {
            BeginFueling();
        }
    }

    private void UpdateUI(PlayerHealth playerHealth = null) {
        if (playerHealth == null) {
            stationUI.gameObject.SetActive(false);
        } else {
            stationUI.gameObject.SetActive(true);

            if (playerHealth.GetHealth() < PlayerHealth.MAX_HEALTH && _scoreController.GetScore() >= HEALTH_COST) {
                stationUI.slider.gameObject.SetActive(true);
                stationUI.slider.normalizedValue = (baseWaitTime - waitBeforeRepairing + timer) / (baseWaitTime + 1f);
                stationUI.priceText.text = $"{HEALTH_COST} loot = {repairRate * 100 / PlayerHealth.MAX_HEALTH}% health";
            } else {
                stationUI.slider.gameObject.SetActive(false);
                stationUI.priceText.text = _scoreController.GetScore() < HEALTH_COST ? "Need more loot" : "All healed!";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            timer = 0f;
            BeginWait();
            waitBeforeRepairing = baseWaitTime;
            UpdateUI(other.GetComponent<PlayerHealth>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (waitBeforeRepairing <= 0) {
            timer += Time.deltaTime;
        }

        if (other.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth.GetHealth() < PlayerHealth.MAX_HEALTH && timer >= 1 && _scoreController.GetScore() >= HEALTH_COST) {
                timer = 0;
                playerHealth.Heal(repairRate);
                _scoreController.SubScore(HEALTH_COST);
                waitBeforeRepairing = baseWaitTime;
                isRepairing = false;
                isWaiting = true;
            }
            UpdateUI(playerHealth);
        }    
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isRepairing = false;
            isWaiting = false;
            waitBeforeRepairing = baseWaitTime;
            stationUI.gameObject.SetActive(false);
            UpdateUI();
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