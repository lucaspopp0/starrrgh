using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public void Open() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        GameState.shared.paused = true;
    }

    public void Close() {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        GameState.shared.paused = false;
    }

    public void Resume() {
        Close();
    }

    public void Restart() {
        Close();
        SceneManager.LoadScene(SceneId.MainGame);
    }

    public void Quit() {
        SceneManager.LoadScene(SceneId.MainMenu);
    }
    
}
