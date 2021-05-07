using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class CargoShip: MonoBehaviour {

    private SpriteRenderer _renderer;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite raidedSprite;

    [SerializeField] private GameObject canLootPopup;
    [SerializeField] private GameObject lootingPopup;
    [SerializeField] private Slider lootProgress;
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        canLootPopup.SetActive(false);
        lootingPopup.SetActive(false);
        lootProgress.normalizedValue = 0f;
    }

    public void RaidComplete() {
        _renderer.sprite = raidedSprite;
    }

    public void CanLoot() {
        canLootPopup.SetActive(true);
        lootingPopup.SetActive(false);
    }

    public void StartLooting() {
        canLootPopup.SetActive(false);
        lootingPopup.SetActive(true);
        lootProgress.normalizedValue = 0f;
    }

    public void SetLootProgress(float normalizedProgress) {
        lootProgress.normalizedValue = normalizedProgress;
    }

    public void ClearLootState() {
        canLootPopup.SetActive(false);
        lootingPopup.SetActive(false);
        lootProgress.normalizedValue = 0f;
    }

}