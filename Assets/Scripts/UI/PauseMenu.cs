using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField] private AudioMixerSnapshot normalVolumes;
    [SerializeField] private AudioMixerSnapshot pausedVolumes;    

    public void Open() {
        PauseGameState();
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
        UnpauseGameState();
    }

    private void PauseGameState() {
        pausedVolumes.TransitionTo(0.01f);
        Time.timeScale = 0;
        GameState.shared.paused = true;
    }

    private void UnpauseGameState() {
        normalVolumes.TransitionTo(0.01f);
        Time.timeScale = 1;
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
        Close();
        SceneManager.LoadScene(SceneId.MainMenu);
    }
    
}
