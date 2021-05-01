
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPopup: MonoBehaviour {

    [SerializeField] private TMP_Text statsText;

    private void Awake() {
        Close();
    }

    public void Open() {
        var text = "";
        
        text += $"Duration: {RunStats.Current.Duration:F2}s\n";
        text += $"Score: {RunStats.Current.Score}\n";
        text += $"Damage Taken: {(RunStats.Current.DamageTaken * 100):F}%\n";
        text += $"Power-Ups Collected: {RunStats.Current.PowerupsCollected}\n";
        text += $"Cargo Ships Looted: {RunStats.Current.CargoShipsLooted}\n";
        text += $"Cargo Ships Destroyed: {RunStats.Current.CargoShipsDestroyed}\n";
        text += $"Police Ships Destroyed: {RunStats.Current.PoliceShipsDestroyed}";

        statsText.text = text;
        
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void PlayAgain() {
        Close();
        SceneManager.LoadScene(SceneId.MainGame);
    }

    public void Quit() {
        SceneManager.LoadScene(SceneId.MainMenu);
    }

}