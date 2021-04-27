﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField] private int bombAmount = 0;
    [SerializeField] private GameObject _bombPrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && bombAmount > 0)
        {
            GameObject bomb = Instantiate(_bombPrefab);
            bomb.transform.position = gameObject.transform.position;
            bomb.GetComponent<Bomb>().EnableFuse();
            UseBomb();

        }
    }

    public void AddBombs(int amount)
    {
        bombAmount += amount;
    }

    public void UseBomb()
    {
        bombAmount--;
    }
}