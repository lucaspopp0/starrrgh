using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerFuel : MonoBehaviour
{
    [SerializeField] private FuelBar _fuelBar;
    
    [FormerlySerializedAs("_totalFuelTime")] [SerializeField] private float _currentFuelTime;

    [SerializeField] private float _dashFuelConsumption;

    //The max amount of fuel that can be used (in seconds)
    [SerializeField] public float _maxFuelTime;
    private PlayerMovement _movement;

    private float _infiniteFuelTimer = 0f;

    private bool _initialDash = true;


    private void Start()
    {
        _movement = gameObject.GetComponent<PlayerMovement>();
        _currentFuelTime = _maxFuelTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_infiniteFuelTimer <= 0 )
        {
            if(_initialDash && _movement.isBoost()){
                _initialDash = false;
                _currentFuelTime -= _dashFuelConsumption;
            }
            else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.L)) && (!_movement.isDisabled() || _movement.isLooting()) && !_movement.isBoost())
            {
                _currentFuelTime -= Time.deltaTime;
                _initialDash = true;

            }
            else if(!_movement.isBoost()){
                _initialDash = true;
            }


                if(_currentFuelTime/_maxFuelTime < 0){
                    _fuelBar.SetFuel(0);
                }
                else{
                    _fuelBar.SetFuel(_currentFuelTime / _maxFuelTime);
                }

                if (_currentFuelTime <= 0)
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
        _infiniteFuelTimer = timer;
    }

    //Increase the current fuel amount by the input (in seconds)
    public void AddFuel(float amount)
    {
        _currentFuelTime += amount;
    }

    public float GetMaxFuel()
    {
        return _maxFuelTime;
    }
    public float GetFuel()
    {
        return _currentFuelTime;
    }
}
