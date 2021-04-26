using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CargoShip: MonoBehaviour {

    private SpriteRenderer _renderer;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite raidedSprite;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void RaidComplete() {
        _renderer.sprite = raidedSprite;
    }

}