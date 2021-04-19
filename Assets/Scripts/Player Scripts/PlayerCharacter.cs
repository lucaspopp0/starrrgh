using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
	[SerializeField] private HealthBar _healthBar;
    private int _health;

	void Start() {
		_health = 1000;
	}

	void Update(){
		if(_health == 0){
			Debug.Log("Die");
		}
	}

	public void Hurt(int damage) {
		_health -= damage;
		Debug.Log("Health: " + _health);
	}
}
