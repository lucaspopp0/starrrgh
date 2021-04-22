﻿using System.Collections;
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

        //Spawn a feature for the number specified, and then add any planets to the player movement script
        for (int i = 0; i < numObjects; i++)
        {
            //Spawning at some random vector
            float distance = Random.Range(minSpawnRadius, spawnRadius);
            Vector3 v = Random.insideUnitCircle.normalized * distance;
            //Maybe rotate prefabs to some random direction
            GameObject o = Instantiate(randomPrefab(), v, Quaternion.identity);
            spawnedObjects[i] = o;

            //Adding all planets in the feature to the player movement script
            foreach(Transform child in o.transform)
            {
                //Probably don't have to do this check since we do it in addPlanet
                if(child.gameObject.GetComponent<Planet>() != null)
                {
                    movement.addPlanet(child.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if planets are still within spawn distance of the ship

        //If a planet is outside that range, despawn it
        //Then spawn a new planet in the direction the ship is moving
    }

    /*
     * Returns a random object from our prefab list
     */
    private GameObject randomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }
}