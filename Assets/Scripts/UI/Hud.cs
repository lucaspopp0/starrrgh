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
    }

    public void SetScore(int score) {
        // Calculate what the max possible width of a number with this many digits will be, and use that so
        // the size of the text doesn't change every time the score does
        var allEights = Regex.Replace(score.ToString(), @"\d", "8");
        var boundarySize = _scoreText.GetPreferredValues(allEights);
        _scoreText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, boundarySize.x);
        
        // Set the actual score text
        _scoreText.text = score.ToString();
    }

    public void Reset() {
        _healthBar.SetHealth(1f);
        _fuelBar.SetFuel(1f);
        SetScore(0);
    }

}