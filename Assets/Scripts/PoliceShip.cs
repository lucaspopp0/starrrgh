using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PoliceShip: MonoBehaviour {

    private SpriteRenderer _renderer;

    private Sprite _currentSprite;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite semiDamagedSprite;
    [SerializeField] private Sprite veryDamagedSprite;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        SetSprite(normalSprite);
    }

    private void SetSprite(Sprite sprite) {
        _currentSprite = sprite;
        _renderer.sprite = sprite;
    }

    public void TakeDamage() {
        if (_currentSprite == normalSprite) {
            SetSprite(semiDamagedSprite);
        } else {
            SetSprite(veryDamagedSprite);
        }
    }

}