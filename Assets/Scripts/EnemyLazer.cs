using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start () {
        // Kill me in one second
        Destroy (gameObject, 3.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 12f * Time.deltaTime, 0);
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("hit somthing");
		PlayerHealth player = other.GetComponent<PlayerHealth>();
		if (player != null) {
			player.Hurt(50);
		}
        if(other.GetComponent<ReactiveTarget>()){
            other.GetComponent<ReactiveTarget>().ReactToHit();
        }
		Destroy(this.gameObject);
	}
}
