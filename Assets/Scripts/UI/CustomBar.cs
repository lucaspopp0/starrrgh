
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomBar: MonoBehaviour {

    [SerializeField] protected Slider slider;
    [SerializeField] protected TMP_Text text;

    public void SetNormalizedValue(float normalizedValue) {
        text.text = String.Format("{0:F0}%", normalizedValue * 100f);
        slider.normalizedValue = normalizedValue;
    }

}
