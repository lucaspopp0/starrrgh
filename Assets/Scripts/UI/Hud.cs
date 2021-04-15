using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud: MonoBehaviour {

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private FuelBar _fuelBar;
    [SerializeField] private TMP_Text _scoreText;

    public HealthBar healthBar => _healthBar;
    public FuelBar fuelBar => _fuelBar;
    public TMP_Text scoreText => _scoreText;

    private int score = 0;

    private void Start() {
        Reset();
        StartCoroutine(SlowlyIncrement());
    }

    public void SetScore(int score) {
        _scoreText.text = score.ToString();
    }

    public void Reset() {
        _healthBar.SetHealth(1f);
        _fuelBar.SetFuel(1f);
        SetScore(0);
    }

    public IEnumerator SlowlyIncrement() {
        while (true) {
            yield return new WaitForSeconds(1f);
            SetScore(++score);
        }
    }

}