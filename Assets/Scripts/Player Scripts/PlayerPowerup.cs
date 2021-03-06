using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    private Hud _hud;
    [SerializeField] private AudioSource _powerupSound;
    [SerializeField] private TextPopup textPopup;

    private int bombAmountToAdd = 0;
    private int currentBombAmount = 0;
    private float infiniteFuelTime = 0f;
    private int healAmount = 0;
    private float speedCoeff = 0f;
    /* Health = 0,
        Shield = 1,
        Bomb = 2,
        Fuel = 3,
        Speed = 4,
     */
    private int[] powerUpAmounts = new int[5];
    private void Awake() {
        _hud = GameObject.FindWithTag("HUD").GetComponent<Hud>();
    }
    public void ObtainPowerup(Hud.PowerupId id, float value = 0f, int amountGained = 1) {
        RunStats.Current.PowerupsCollected++;
        _hud.GainPowerup(id, amountGained);
        _powerupSound.Play();
        switch (id)
        {
            case Hud.PowerupId.Bomb:
                powerUpAmounts[(int) Hud.PowerupId.Bomb]++;
                bombAmountToAdd = (int)value;
                textPopup.DisplayPopup($"+{(int)value} BOMBS", Color.cyan, 1.5f);
                break;
            case Hud.PowerupId.Fuel:
                powerUpAmounts[(int) Hud.PowerupId.Fuel]++;
                infiniteFuelTime = value;
                textPopup.DisplayPopup($"+1 INFINITE FUEL POWERUP", Color.cyan, 1.5f);
                break;
            case Hud.PowerupId.Health:
                powerUpAmounts[(int) Hud.PowerupId.Health]++;
                healAmount = (int)value;
                textPopup.DisplayPopup("+1 HEALTH POWERUP", Color.cyan, 1.5f);
                break;
            case Hud.PowerupId.Shield:
                powerUpAmounts[(int) Hud.PowerupId.Shield]++;
                textPopup.DisplayPopup($"+1 SHIELD POWERUP", Color.cyan, 1.5f);
                break;
            case Hud.PowerupId.Speed:
                powerUpAmounts[(int) Hud.PowerupId.Speed]++;
                textPopup.DisplayPopup($"+1 SPEED BOOST", Color.cyan, 1.5f);
                speedCoeff = value;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && HasPowerupAvailable(Hud.PowerupId.Health))
        {
            UsePowerup(Hud.PowerupId.Health,healAmount);
        }
        if (Input.GetKeyDown(KeyCode.U) && HasPowerupAvailable(Hud.PowerupId.Shield))
        {
            UsePowerup(Hud.PowerupId.Shield);
        }
        if (Input.GetKeyDown(KeyCode.I) && HasPowerupAvailable(Hud.PowerupId.Bomb) && BombsDepleated())
        {
           //Player needs to "reload" bombs from bomb powerup
           UsePowerup(Hud.PowerupId.Bomb,bombAmountToAdd);
           UseBomb();
        }
        else if (Input.GetKeyDown(KeyCode.I) && !BombsDepleated())
        {//Player has bombs they need to use still
            UseBomb();
        }
       
        if (Input.GetKeyDown(KeyCode.O) && HasPowerupAvailable(Hud.PowerupId.Fuel))
        {
            UsePowerup(Hud.PowerupId.Fuel,infiniteFuelTime);
        }
        if (Input.GetKeyDown(KeyCode.P) && HasPowerupAvailable(Hud.PowerupId.Speed))
        {
            UsePowerup(Hud.PowerupId.Speed, speedCoeff);
        }
    }

    private void UseBomb()
    {
        gameObject.GetComponent<PlayerBomb>().UseBomb();
        currentBombAmount--;
    }
    private bool BombsDepleated()
    {
        return currentBombAmount <= 0;
    }

    private bool HasPowerupAvailable(Hud.PowerupId id)
    {
        return powerUpAmounts[(int) id] > 0;
    }

    public void UsePowerup(Hud.PowerupId id, float value = 0)
    {
        Debug.Log(value);
        powerUpAmounts[(int) id]--;
        _hud.UsePowerup(id);
        switch (id)
        {
            case Hud.PowerupId.Bomb:
                currentBombAmount += (int) value;
                gameObject.GetComponent<PlayerBomb>().AddBombs((int)value);
                break;
            case Hud.PowerupId.Fuel:
                _hud.ActivatePowerup(Hud.PowerupId.Fuel, value);
                gameObject.GetComponent<PlayerFuel>().InfiniteFuel(value);
                break;
            case Hud.PowerupId.Health:
                gameObject.GetComponent<PlayerHealth>().Heal((int)value);
                break;
            case Hud.PowerupId.Shield:
                _hud.ActivatePowerup(Hud.PowerupId.Shield);
                gameObject.GetComponent<PlayerHealth>().setShield(true);
                break;
            case Hud.PowerupId.Speed:
                _hud.ActivatePowerup(Hud.PowerupId.Speed, PowerupSpeed.DURATION);
                gameObject.GetComponent<PlayerMovement>().speedUp(value,PowerupSpeed.DURATION);
                break;
        }
    }
}
