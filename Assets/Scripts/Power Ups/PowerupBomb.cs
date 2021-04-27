﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBomb : MonoBehaviour
{
    public int bombAmount = 3;//How long in seconds the player has infinite fuel

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBomb>().AddBombs(bombAmount);
            Destroy(gameObject);
        }
    }

}
