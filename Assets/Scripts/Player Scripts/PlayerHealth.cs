using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private Hud _hud;
    private int _health;
    private static int MAX_HEALTH = 1000;
    [SerializeField] private GameObject explosionEffect;

    void Start() {
		_health = MAX_HEALTH;
	}

	void Update(){
		if(_health == 0){
			Die();
		}
	}

	public void Hurt(int damage) {
		_health -= damage;
		_hud.healthBar.SetNormalizedValue(_health / (float)MAX_HEALTH);
	}

	public void Die() {
		_health = 0;
		_hud.healthBar.SetNormalizedValue(0);
		_hud.PlayerDied();
		GetComponent<PlayerMovement>().kill();
		explosionEffect.SetActive(true);
	}
	
}
