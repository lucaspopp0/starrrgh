using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : CustomBar {
    public void SetFuel(float normalizedValue) {
        SetNormalizedValue(normalizedValue);
    }

}
