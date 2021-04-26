using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject policePrefab;
    [SerializeField] private int numInitialPolice;
    [SerializeField] private GameObject cargoshipPrefab;
    [SerializeField] private int numInitialCargo;

    private GameObject playerObject;
	private GameObject _enemy;

	Vector3 checkNotHit;
	Collider[] intersecting;
	void Start() {
        playerObject = GameObject.Find("Player");
		Vector3 diff = playerObject.transform.position;
        int[] posOrNeg = {-1,1};
        float xPos;
        float yPos;

		int num_police = 0;
		while (num_police < numInitialPolice) {
            xPos = posOrNeg[Random.Range(0,2)] * Random.Range(5,30);
            yPos = posOrNeg[Random.Range(0,2)] * Random.Range(5,30);
			checkNotHit = new Vector3(xPos, yPos, 0); //these should be changed to map bounds

			intersecting = Physics.OverlapSphere(checkNotHit, 1.5f);
			if (intersecting.Length < 1) {
				spawn(checkNotHit, policePrefab);
				num_police++;
			} 
		}

        int num_cargo = 0;
        while (num_cargo < numInitialCargo) {
            xPos = posOrNeg[Random.Range(0,2)] * Random.Range(5,30);
            yPos = posOrNeg[Random.Range(0,2)] * Random.Range(5,30);
			checkNotHit = new Vector3(xPos, yPos, 0); //these should be changed to map bounds
			intersecting = Physics.OverlapSphere(checkNotHit, 1.5f);
			if (intersecting.Length < 1) {
				spawn(checkNotHit, cargoshipPrefab);
				num_cargo++;
			} 
		}
	}

    void spawn(Vector3 location, GameObject prefebToSpawn){
        _enemy = Instantiate(prefebToSpawn) as GameObject;
		_enemy.transform.position = location;
		float angle = Random.Range(0,360);
		_enemy.transform.Rotate(0,0, angle);
		_enemy.GetComponent<WanderingAI>().SetAlive(true);

    }

}
