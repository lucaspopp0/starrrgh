using System;
using System.Collections;
using System.Text.RegularExpressions;
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
        var allEights = Regex.Replace(score.ToString(), @"\d", "8");
        var boundarySize = _scoreText.GetPreferredValues(allEights);
        _scoreText.text = score.ToString();
        _scoreText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, boundarySize.x);
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