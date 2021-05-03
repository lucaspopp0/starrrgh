using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Planet : MonoBehaviour {

    private static Random spriteIndexGenerator = new Random();

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer minimapIcon;
    
    [SerializeField] private float mass;
    [SerializeField] private Sprite[] sprites;

    private void Start() {
        var index = spriteIndexGenerator.Next(sprites.Length);
        renderer.sprite = sprites[index];
        minimapIcon.sprite = sprites[index];
    }

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().Die();
        }    }
}
