using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour {
    
    private Slider _slider;

    private void Awake() {
        _slider = GetComponent<Slider>();
    }

    public void SetHealth(float normalizedValue) {
        _slider.normalizedValue = normalizedValue;
    }

}