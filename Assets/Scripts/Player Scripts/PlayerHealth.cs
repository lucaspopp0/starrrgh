using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] private Gradient damageColorGradient;
	[SerializeField] private SpriteRenderer shipSpriteRenderer;
	[SerializeField] private AudioSource hurtSound;
	[SerializeField] private TextPopup textPopup;
		
	private Hud _hud;
    private int _health;
    public static int MAX_HEALTH = 1000;
    private bool _shielded = false;
    private float _shieldTimer = 0f;
    private static float MAX_SHIELD_TIME = 3f;
    
    [SerializeField] private GameObject explosionEffect;

    private void Awake() {
	    _hud = GameObject.FindWithTag("HUD").GetComponent<Hud>();
    }

    void Start() {
		_health = MAX_HEALTH;
		shipSpriteRenderer.color = damageColorGradient.Evaluate(0);
    }

	void Update(){
		if(_health == 0){
			Die();
		}

		if (_shieldTimer > 0)
		{
			_shieldTimer -= Time.deltaTime;
		}
	}

	public void Hurt(int damage) {
		var percent = damage / (float) MAX_HEALTH;
		_health -= damage;
		_hud.healthBar.SetNormalizedValue(_health / (float) MAX_HEALTH);
		shipSpriteRenderer.color = damageColorGradient.Evaluate(1f - _health / (float) MAX_HEALTH);
		hurtSound.Play();
		RunStats.Current.DamageTaken += percent;
		textPopup.DisplayPopup($"-{damage / (float) MAX_HEALTH * 100f} HEALTH", Color.red);
	}

	public void Heal(int amount)
	{
		var percent = amount / (float) MAX_HEALTH;
		_health += amount;
		_hud.healthBar.SetNormalizedValue(_health / (float) MAX_HEALTH);
		shipSpriteRenderer.color = damageColorGradient.Evaluate(1f - _health / (float) MAX_HEALTH);
		textPopup.DisplayPopup($"+{amount / (float) MAX_HEALTH * 100f}% HEALTH", Color.green);
	}

	public void Die() {
		if (!_shielded && _shieldTimer <= 0) {
			RunStats.Current.DamageTaken += _health / (float)MAX_HEALTH;
			_health = 0;
			_hud.healthBar.SetNormalizedValue(0);
			shipSpriteRenderer.color = damageColorGradient.Evaluate(1);
			_hud.PlayerDied();
			GetComponent<PlayerMovement>().kill();
			explosionEffect.SetActive(true);
		}
		else
		{
			setShield(false);
			startShieldTimer();
		}
		
	}

	public void setShield(bool val)
	{
		_shielded = val;
	}

	void startShieldTimer()
	{
		_shieldTimer = MAX_SHIELD_TIME;
	}

	public int GetHealth()
	{
		return _health;
	}
	
}
