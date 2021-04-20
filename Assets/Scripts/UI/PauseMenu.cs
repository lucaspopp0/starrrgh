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
    }

    public void Close() {
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
