using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] private Gradient damageColorGradient;
	[SerializeField] private SpriteRenderer shipSpriteRenderer;
	[SerializeField] private AudioSource hurtSound;
	[SerializeField] private TextPopup textPopup;
	[SerializeField] private GameObject shieldEffect;
	[SerializeField] private ParticleSystem healthEffect;
		
	private Hud _hud;
    private int _health;
    public static int MAX_HEALTH = 1000;
    private bool _shielded = false;
    private float _shieldImmunityTimer = 0f;
    private static float MAX_SHIELD_IMMUNITY_TIME = 3f;
    
    [SerializeField] private GameObject explosionEffect;

    private void Awake() {
	    _hud = GameObject.FindWithTag("HUD").GetComponent<Hud>();
    }

    void Start() {
		_health = MAX_HEALTH;
		shipSpriteRenderer.color = damageColorGradient.Evaluate(0);
    }

	void Update(){
		if(_health <= 0){
			Die();
			_health = 0;
		}

		if (_shieldImmunityTimer > 0) {
			_shieldImmunityTimer -= Time.deltaTime;
		} else if (!_shielded) {
			shieldEffect.SetActive(false);
		}
	}

	public void Hurt(int damage) {
		if (!_shielded) {
			var percent = damage / (float) MAX_HEALTH;
			_health -= damage;
			_hud.healthBar.SetNormalizedValue(_health / (float) MAX_HEALTH);
			shipSpriteRenderer.color = damageColorGradient.Evaluate(1f - _health / (float) MAX_HEALTH);
			hurtSound.Play();
			RunStats.Current.DamageTaken += percent;
			textPopup.DisplayPopup($"-{damage / (float) MAX_HEALTH * 100f}% HEALTH", Color.red);
		}
	}

	public void Heal(int amount)
	{
		_health += amount;
		_hud.healthBar.SetNormalizedValue(_health / (float) MAX_HEALTH);
		shipSpriteRenderer.color = damageColorGradient.Evaluate(1f - _health / (float) MAX_HEALTH);
		textPopup.DisplayPopup($"+{amount / (float) MAX_HEALTH * 100f}% HEALTH", Color.green);
		healthEffect.Play();
	}

	public void Die() {
		if (!_shielded && _shieldImmunityTimer <= 0) {
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
		if (!_shielded && val) {
			shieldEffect.SetActive(true);
			startShieldTimer();
		}
		
		_shielded = val;
	}

	void startShieldTimer()
	{
		_shieldImmunityTimer = MAX_SHIELD_IMMUNITY_TIME;
	}

	public int GetHealth()
	{
		return _health;
	}
	
}
