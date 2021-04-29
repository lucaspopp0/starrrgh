using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField] private GameObject graphic;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject explosionRadius;

    
    [SerializeField] private float fuseTimer = .4f;
    private bool fuseEnabled = false;

    private void Start() {
        graphic.SetActive(true);
        effect.SetActive(false);
    }

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
    private void OnTriggerEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().Hurt(500);
        }
        if (other.gameObject.tag == "Police")
        {
            other.gameObject.GetComponent<WanderingAI>().SetAlive(false);
        }
    }
    public void EnableFuse()
    {
        fuseEnabled = true;
    }

    private IEnumerator Detonate()
    {
        graphic.SetActive(false);
        effect.SetActive(true);
        Debug.Log("boom");
        explosionRadius.SetActive(true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
