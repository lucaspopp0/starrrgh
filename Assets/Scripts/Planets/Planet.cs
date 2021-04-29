using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private float mass;

    public float getMass()
    {
        return mass;
    }

    public void setMass(float m)
    {
        mass = m;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerHealth>().Die();
        }
    }
}
