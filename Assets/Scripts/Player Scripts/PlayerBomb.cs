using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    private int bombAmount = 0;

    public void AddBombs(int amount)
    {
        bombAmount += amount;
    }

    public void UseBomb()
    {
        bombAmount--;
    }
}
