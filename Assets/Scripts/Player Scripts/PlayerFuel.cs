using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFuel : MonoBehaviour
{
    [SerializeField] private FuelBar _fuelBar;

    [SerializeField] private float _fuelTime;

    [SerializeField] private float _totalFuelTime;

    //The max amount of fuel that can be used (in seconds)
    [SerializeField] private float _maxFuelTime;
    private PlayerMovement _movement;
    private float _infiniteFuelTimer = 0f;

    private bool _initialDash = true;


    private void Start()
    {
        _movement = gameObject.GetComponent<PlayerMovement>();
        _totalFuelTime = _maxFuelTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_infiniteFuelTimer <= 0)
        {
            if(_initialDash && _movement.isBoost()){
                _initialDash = false;
                _fuelTime += 5;
                _totalFuelTime -= 5;
            }
            else if (Input.GetKey(KeyCode.W) && !_movement.isDisabled() && !_movement.isBoost())
            {
                _fuelTime += Time.deltaTime;
                _totalFuelTime -= Time.deltaTime;
                _initialDash = true;

            }
            else if(!_movement.isBoost()){
                _initialDash = true;
            }

            if (_fuelTime >= 0)//W was pressed for a nonzero amount of time, update hud
            {
                if(_totalFuelTime/_maxFuelTime < 0){
                    _fuelBar.SetFuel(0);
                }
                else{
                    _fuelBar.SetFuel(_totalFuelTime / _maxFuelTime);
                }
                
            }

            if (Input.GetKeyUp(KeyCode.W) || _movement.isDisabled())//You are not thrusting so stop using fuel
            {
                _fuelTime = 0;
            }

            if (_totalFuelTime <= 0)
            {
                _movement.setDisabled(true);
            }
        }
        else
        {
            _infiniteFuelTimer -= Time.deltaTime;
        }



    }

    public void InfiniteFuel(float timer)
    {
        Debug.Log("Fuel Timer");
        _infiniteFuelTimer = timer;
    }

    //Increase the current fuel amount by the input (in seconds)
    public void AddFuel(float amount)
    {
        _totalFuelTime += amount;
    }
}
