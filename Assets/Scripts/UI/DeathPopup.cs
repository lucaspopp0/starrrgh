
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPopup: MonoBehaviour {

    private void Awake() {
        Close();
    }

    public void Open() {
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