using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawn : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 100.0f;
    [SerializeField] private float minSpawnRadius = 10.0f;
    [SerializeField] private int numObjects = 10;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject player;

    private GameObject[] spawnedObjects;
    private PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = new GameObject[numObjects];
        movement = player.GetComponent<PlayerMovement>();

        for (int i = 0; i < numObjects; i++)
        {
            float distance = Random.Range(minSpawnRadius, spawnRadius);
            Vector3 v = Random.insideUnitCircle.normalized * distance;
            GameObject o = Instantiate(randomPrefab(), v, Quaternion.identity);
            o.GetComponent<Planet>().setMass(7);
            spawnedObjects[i] = o;
            movement.addPlanet(o);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject randomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length - 1)];
    }
}
