using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField] private int bombAmount = 0;
    [SerializeField] private GameObject _bombPrefab;
    
    public void AddBombs(int amount)
    {
        bombAmount += amount;
    }

    public void UseBomb()
    {
        GameObject bomb = Instantiate(_bombPrefab);
        bomb.transform.position = gameObject.transform.position;
        bomb.GetComponent<Bomb>().EnableFuse();
        bombAmount--;
    }
}