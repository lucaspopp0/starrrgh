using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private Hud _hud;
    private int _health;
    private static int MAX_HEALTH = 1000;

    private bool _shielded = false;
    private float _shieldTimer = 0f;
    private static float MAX_SHIELD_TIME = 3f;
    
    [SerializeField] private GameObject explosionEffect;


    void Start() {
		_health = MAX_HEALTH;
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
		_health -= damage;
		_hud.healthBar.SetNormalizedValue(_health / (float)MAX_HEALTH);
	}

	public void Heal(int amount)
	{
		_health += amount;
		_hud.healthBar.SetNormalizedValue(_health /(float)MAX_HEALTH);
	}

	public void Die() {
		if (!_shielded && _shieldTimer <= 0)
		{
			_health = 0;
			_hud.healthBar.SetNormalizedValue(0);
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
	
}
