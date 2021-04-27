using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    // Start is called before the first frame update	
	public ParticleSystem deathEffect;

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
		Destroy(this.gameObject.GetComponent<Collider>());
		Destroy(Instantiate(deathEffect.gameObject,transform.position,Quaternion.identity) as GameObject,deathEffect.startLifetime);

		yield return new WaitForSeconds(0.1f);
		
		Destroy(this.gameObject);
	}
}
