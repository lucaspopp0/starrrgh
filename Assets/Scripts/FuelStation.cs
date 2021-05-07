using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour {

    private const int FUEL_COST = 20;
    [SerializeField] private float baseWaitTime = 2f; 
    private float waitBeforeFueling = 0f;
    private bool isWaiting = false;
    private bool isFueling = false;
    [SerializeField] private float fuelingRate = 20f;//What how many seconds of fuel is restored per second
    [SerializeField] private StationUI stationUI;

    private ScoreController _scoreController;

    private void Start()
    {
        _scoreController = GameObject.Find("Score Controller").GetComponent<ScoreController>();
        waitBeforeFueling = baseWaitTime;
        UpdateUI();
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

    private void UpdateUI(PlayerFuel playerFuel = null) {
        if (playerFuel == null) {
            stationUI.gameObject.SetActive(false);
        } else {
            stationUI.gameObject.SetActive(true);

            if (playerFuel.GetFuel() < playerFuel.GetMaxFuel() && _scoreController.GetScore() >= FUEL_COST) {
                stationUI.slider.gameObject.SetActive(true);
                stationUI.slider.normalizedValue = (baseWaitTime - waitBeforeFueling) / baseWaitTime;
                stationUI.priceText.text = $"{FUEL_COST} loot = {fuelingRate * 100 / playerFuel.GetMaxFuel()}% fuel";
            } else {
                stationUI.slider.gameObject.SetActive(false);
                stationUI.priceText.text = _scoreController.GetScore() < FUEL_COST ? "Need more loot" : "All fueled up!";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            BeginWait();
            UpdateUI(other.GetComponent<PlayerFuel>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            PlayerFuel playerFuel = other.gameObject.GetComponent<PlayerFuel>();
            if (isFueling && playerFuel.GetFuel() < playerFuel.GetMaxFuel() && _scoreController.GetScore() > 0) {
                BeginWait();
                isFueling = false;
                _scoreController.SubScore(FUEL_COST);
                
                if (playerFuel._maxFuelTime - playerFuel.GetFuel() < 40)
                {
                    playerFuel.AddFuel(playerFuel.GetMaxFuel() - playerFuel.GetFuel());
                }
                else
                {
                    playerFuel.AddFuel(fuelingRate);
                }
            }
            UpdateUI(playerFuel);
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
        waitBeforeFueling = baseWaitTime;
    }
}
