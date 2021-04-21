using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    
    // Start is called before the first frame update
    private void Awake() {
        Close();
    }

    public void Open() {
        GameState.shared.paused = true;
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Close() {
        gameObject.SetActive(false);
        GameState.shared.paused = false;
        Time.timeScale = 1;
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
