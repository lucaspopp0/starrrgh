using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawn : MonoBehaviour
{
    [SerializeField] private float maxSpawnRadius = 100.0f;
    [SerializeField] private float minSpawnRadius = 10.0f;
    [SerializeField] private int numObjects = 10;
    [SerializeField] private GameObject player;

    [SerializeField] private WeightedItem[] prefabs;

    private HashSet<GameObject> spawnedObjects;
    private PlayerMovement movement;

    //A struct that stores a planetary feature and a weight
    //The weight is how likely we are to spawn that feature
    [System.Serializable]
    private struct WeightedItem
    {
        [SerializeField] private GameObject obj;
        [SerializeField] private float weight;

        public WeightedItem(GameObject obj, float weight)
        {
            this.obj = obj;
            this.weight = weight;
        }

        public GameObject getObject()
        {
            return obj;
        }

        public float getWeight()
        {
            return weight;
        }

        public void setWeight(float weight)
        {
            this.weight = weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnedObjects = new HashSet<GameObject>();
        movement = player.GetComponent<PlayerMovement>();

        //Spawn a feature for the number specified, and then add any planets to the player movement script
        for (int i = 0; i < numObjects; i++)
        {
            //Spawning at some random vector
            float distance = Random.Range(minSpawnRadius - 0.1f * maxSpawnRadius, maxSpawnRadius);
            //Debug.Log(360)
            Vector3 v = Quaternion.AngleAxis((360 / numObjects) * i, player.transform.forward) * player.transform.up * distance + player.transform.position;
            //Maybe rotate prefabs to some random direction
            spawnFeature(v, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] cachedPlanets = new GameObject[spawnedObjects.Count];
        spawnedObjects.CopyTo(cachedPlanets);
        //Check if planets are still within spawn distance of the ship
        for (int i = 0; i < cachedPlanets.Length; i++)
        {
            GameObject p = cachedPlanets[i];
            Vector2 radius = new Vector2(p.transform.position.x - player.transform.position.x, p.transform.position.y - player.transform.position.y);
            //If a planet is outside that range, despawn it
            //Simplify all this logic later
            if (radius.magnitude >= maxSpawnRadius)
            {
                Vector3 v = Quaternion.AngleAxis(Random.Range(-45, 45), player.transform.forward) * (player.transform.up * (maxSpawnRadius - 0.1f * maxSpawnRadius) + player.transform.position);
                bool canSpawn = true;
                //This is very inefficient, if we run into some performance issues, should probably try and fix this
                //This loop just checks all the stored features to see if the desired object can be spawned
                for (int j = 0; j < cachedPlanets.Length; j++)
                {
                    GameObject target = cachedPlanets[j];
                    if((target.transform.position - v).magnitude <= 10.0f)
                    {
                        canSpawn = false;
                    }
                }
                //If there is space to spawn a thing, delete the feature p and create a new feature at v
                if (canSpawn)
                {
                    //Remove the feature p
                    deleteFeature(p);
                    //Then spawn a new planet in the direction the ship is moving (with some random angle applied)
                    //Probably should rotate stuff here in some random direction
                    spawnFeature(v, Quaternion.identity);
                }
            }
        }
    }

    /*
     * Returns a random object from our prefab list
     */
    private GameObject randomPrefab()
    {
        WeightedItem item = prefabs[Random.Range(0, prefabs.Length)];
        return item.getObject();
        //return prefabs[Random.Range(0, prefabs.Length)];
    }

    private void spawnFeature(Vector3 pos, Quaternion rot)
    {
        GameObject o = Instantiate(randomPrefab(), pos, rot);
        spawnedObjects.Add(o);

        //Adding all planets in the feature to the player movement script
        foreach (Transform child in o.transform)
        {
            //Probably don't have to do this check since we do it in addPlanet
            if (child.gameObject.GetComponent<Planet>() != null)
            {
                movement.addPlanet(child.gameObject);
            }
        }
    }

    private void deleteFeature(GameObject p)
    {
        spawnedObjects.Remove(p);
        //Removing the planets from the player
        foreach (Transform child in p.transform)
        {
            //Probably don't have to do this check since we do it in addPlanet
            if (child.gameObject.GetComponent<Planet>() != null)
            {
                //This throws an error since we modify the planet list while we calculate gravity on the player
                movement.removePlanet(child.gameObject);
            }
        }
        Destroy(p);
    }
}
