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


    private void Start()
    {
        _movement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_infiniteFuelTimer <= 0)
        {

            if (Input.GetKey(KeyCode.W) && !_movement.isDisabled())
            {
                _fuelTime += Time.deltaTime;
                _totalFuelTime += Time.deltaTime;

            }

            if (_fuelTime >= 0)
            {
                _fuelBar.SetFuel(1f - (_totalFuelTime / _maxFuelTime));
            }

            if (Input.GetKeyUp(KeyCode.W) || _movement.isDisabled())
            {
                _fuelTime = 0;
            }

            if (_totalFuelTime >= _maxFuelTime)
            {
                _movement.setDisabled(true);
            }
        }
        else
        {
            _infiniteFuelTimer -= Time.deltaTime;
        }



    }

    public void infiniteFuel(float timer)
    {
        Debug.Log("Fuel Timer");
        _infiniteFuelTimer = timer;
    }
}
