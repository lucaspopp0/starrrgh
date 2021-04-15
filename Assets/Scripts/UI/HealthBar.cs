using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : CustomBar {

    public void SetHealth(float normalizedValue) {
        SetNormalizedValue(normalizedValue);
    }

}