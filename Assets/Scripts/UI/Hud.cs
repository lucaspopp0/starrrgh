using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud: MonoBehaviour {

    public enum PowerupId {

        Health = 0,
        Shield = 1,
        Bomb = 2,
        Fuel = 3,
        Speed = 4,

    };
    
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private FuelBar _fuelBar;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private DeathPopup deathPopup;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private PowerupIndicator[] powerupIndicators;
    public HealthBar healthBar => _healthBar;
    public FuelBar fuelBar => _fuelBar;
    public TMP_Text scoreText => _scoreText;

    private int score = 0;

    private void Start() {
        Reset();
        pauseMenu.Close();
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

    public void Pause() {
        pauseMenu.Open();
    }

    public void Unpause() {
        pauseMenu.Resume();
    }

    public void PlayerDied() {
        deathPopup.Open();
    }

    public void GainPowerup(PowerupId id, int amountGained) {
        powerupIndicators[id.GetHashCode()].Get(amountGained);
    }

    public void UsePowerup(PowerupId id, float duration = 1f) {
        powerupIndicators[id.GetHashCode()].Use();
    }

    public void ActivatePowerup(PowerupId id, float duration = 1)
    {
        powerupIndicators[id.GetHashCode()].Activate(duration);

    }

}