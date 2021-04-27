using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float fuseTimer = 1f;
    private bool fuseEnabled = false;

    private void Update()
    {
        if (fuseEnabled)
        {
            fuseTimer -= Time.deltaTime;
        }

        if (fuseTimer > 0)
        {
            Debug.Log(fuseTimer);
        }
        
        if (fuseTimer <= 0)
        {
            StartCoroutine(Detonate());
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().Hurt(500);
        }
        if (other.gameObject.tag == "Police")
        {
            //TODO: Add police tag
            other.gameObject.GetComponent<WanderingAI>().SetAlive(false);
        }
    }
    private void OnTriggerEnter(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().Hurt(500);
        }
        if (other.gameObject.tag == "Police")
        {
            //TODO: Add police tag
            other.gameObject.GetComponent<WanderingAI>().SetAlive(false);
        }
    }
    public void EnableFuse()
    {
        fuseEnabled = true;
    }

    private IEnumerator Detonate()
    {
        //TODO: Check for collision on children here after enabling them
        //TODO: Maybe it spawns an explosion prefab with its own script
        transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("boom");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
