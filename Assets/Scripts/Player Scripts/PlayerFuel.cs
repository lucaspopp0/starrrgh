using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFuel : MonoBehaviour
{
    [SerializeField] private FuelBar _fuelBar;

    [SerializeField] private float _fuelTime;

    [SerializeField] private float _totalFuelTime;

    [SerializeField] private float _maxFuelTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _fuelTime += Time.deltaTime;
            _totalFuelTime += Time.deltaTime;

        }
        
        if (_fuelTime >= 0)
        {
            _fuelBar.SetFuel(1f - (_totalFuelTime/_maxFuelTime)/10);
        }
        
        if (Input.GetKeyUp(KeyCode.W))
        {
            _fuelTime = 0;
        }


        
    }
}
