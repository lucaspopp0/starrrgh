using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] private Gradient damageColorGradient;
	[SerializeField] private SpriteRenderer shipSpriteRenderer;
		
	private Hud _hud;
    private int _health;
    private static int MAX_HEALTH = 1000;

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
	}

	public void Hurt(int damage) {
		_health -= damage;
		_hud.healthBar.SetNormalizedValue(_health / (float)MAX_HEALTH);
		shipSpriteRenderer.color = damageColorGradient.Evaluate(1f - _health / (float) MAX_HEALTH);
	}

	public void Die() {
		_health = 0;
		_hud.healthBar.SetNormalizedValue(0);
		shipSpriteRenderer.color = damageColorGradient.Evaluate(1);
		_hud.PlayerDied();
		GetComponent<PlayerMovement>().kill();
	}
	
}
