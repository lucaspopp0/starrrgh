using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float fuseTimer = 3f;
    private bool fuseEnabled = false;

    private void Update()
    {
        if (fuseEnabled)
        {
            fuseTimer -= Time.deltaTime;
        }


        if (fuseTimer <= 0)
        {
            Detonate();
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

    public void EnableFuse()
    {
        fuseEnabled = true;
    }

    private void Detonate()
    {
        Debug.Log("boom");
        //TODO: Check for collision on children here after enabling them
        var child = transform.Find("Explosion Radius");
        child.gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
