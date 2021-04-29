using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupIndicator : MonoBehaviour {

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text keyText;
    private int _amount; 

    // Start is called before the first frame update
    void Start() {
        SetAmount(0);
    }

    public void SetAmount(int amount) {
        _amount = amount;
        if (amount > 0) {
            countText.text = "x" + amount;
            image.color = Color.white;
            slotImage.color = Color.white;
            keyText.color = Color.white;
        } else {
            countText.text = "";
            image.color = new Color(1f, 1f, 1f, 0.2f);
            slotImage.color = new Color(1f, 1f, 1f, 0.5f);
            keyText.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    public void Get(int gained) {
        SetAmount(_amount + gained);
    }
    
    public void Use(int used = 1) {
        SetAmount(_amount - used);
    }
}
