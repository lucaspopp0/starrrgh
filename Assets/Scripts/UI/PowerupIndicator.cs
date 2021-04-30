using System;
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
    private float activeDuration = 0;

    // Start is called before the first frame update
    void Start() {
        SetAmount(0);
    }

    private void Update()
    {
        if (activeDuration >= 0)
        {
            slotImage.color = Color.blue;
            activeDuration -= Time.deltaTime;
        }
        else
        {
            SetAmount(_amount);//This is goofy but I'm using this to reset the colors
        }
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

    public void Activate(float duration)
    {
        activeDuration = duration;
    }
}
