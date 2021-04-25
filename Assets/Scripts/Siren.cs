using System;
using System.Collections;
using UnityEngine;

public class Siren: MonoBehaviour {

    private const float SIREN_TIME = 0.2f; 
        
    private Transform _player;

    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject blueLight;

    private float _spriteTimer = 0f;
    private int _spriteCounter = 0;

    private void Awake() {
        TurnOff();
    }

    public void TurnOn() {
        gameObject.SetActive(true);
    }

    public void TurnOff() {
        gameObject.SetActive(false);
    }

    private void Update() {
        _spriteTimer += Time.deltaTime;

        if (_spriteTimer >= SIREN_TIME) {
            _spriteTimer -= SIREN_TIME;
            SwitchSprite();
        }
    }

    private void SwitchSprite() {
        _spriteCounter = 1 - _spriteCounter;

        if (_spriteCounter == 0) {
            redLight.SetActive(true);
            blueLight.SetActive(false);
        } else {
            blueLight.SetActive(true);
            redLight.SetActive(false);
        }
    }

}