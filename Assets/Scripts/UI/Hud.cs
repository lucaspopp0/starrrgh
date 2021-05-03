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

    private static readonly Color[] CHANGE_COLORS = new[] { Color.red, Color.cyan, Color.yellow, Color.magenta, Color.white };

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private FuelBar _fuelBar;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private DeathPopup deathPopup;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private PowerupIndicator[] powerupIndicators;
    public HealthBar healthBar => _healthBar;
    public FuelBar fuelBar => _fuelBar;
    public TMP_Text scoreText => _scoreText;

    private int _displayedScore = 0;
    private int _displayedColorIndex = 0;
    private int score = 0;

    private void Start() {
        Reset();
        pauseMenu.Close();
        SetScore(0, true);
    }

    private void Update() {
        if (_displayedScore != score) {
            var difference = score - _displayedScore;

            if (difference == 1 || difference == -1) {
                _scoreText.color = Color.white;
            } else {
                _displayedColorIndex = (_displayedColorIndex + 1) % CHANGE_COLORS.Length;
                _scoreText.color = CHANGE_COLORS[_displayedColorIndex];
            }
            
            difference /= 4;
            if (difference == 0) {
                difference = _displayedScore > score ? -1 : 1;
            }
                
            _displayedScore += difference;
            
            _scoreText.text = _displayedScore.ToString();
        }
    }

    public void SetScore(int score, bool instant = false) {
        this.score = score;
        
        if (instant) {
            _displayedScore = score;
            _scoreText.text = "0";
        } 
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