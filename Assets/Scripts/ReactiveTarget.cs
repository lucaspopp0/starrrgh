using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    // Start is called before the first frame update	
	public GameObject deathEffect;

	void Start(){

		//anim = GetComponent<Animator>();
	}


	public void ReactToHit() {
		WanderingAI behavior = GetComponent<WanderingAI>();
		if (behavior != null) {
			behavior.SetAlive(false);
		}
		StartCoroutine(Die());
	}

	private IEnumerator Die() {
		// this.transform.Rotate(-75, 0, 0);
		// anim.SetInteger("zombieToState", 2);

		var explosion = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(explosion, 4f);
		yield return null;
		Destroy(this.gameObject.GetComponent<Collider>());
		Destroy(this.gameObject);
	}
}
