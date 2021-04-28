using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField] private AudioMixerSnapshot normalVolumes;
    [SerializeField] private AudioMixerSnapshot pausedVolumes;    

    public void Open() {
        pausedVolumes.TransitionTo(0.01f);
        gameObject.SetActive(true);
        Time.timeScale = 0;
        GameState.shared.paused = true;
    }

    public void Close() {
        normalVolumes.TransitionTo(0.01f);
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
