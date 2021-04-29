using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupIndicator : MonoBehaviour {

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text countText;
    private int _amount; 

    // Start is called before the first frame update
    void Start() {
        SetAmount(0);
    }

    public void SetAmount(int amount) {
        _amount = amount;
        countText.text = amount > 0 ? "x" + amount : "";
        image.color = amount > 0 ? Color.white : new Color(1f, 1f, 1f, 0.2f);
    }

    public void UseOne() {
        SetAmount(_amount - 1);
    }
}
