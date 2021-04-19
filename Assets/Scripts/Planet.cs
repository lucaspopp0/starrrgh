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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
