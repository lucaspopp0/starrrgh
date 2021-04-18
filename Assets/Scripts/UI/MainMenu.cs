using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private MainMenuPanel instructionsMenu;
    [SerializeField] private MainMenuPanel leaderboardMenu;

    private void Awake() {
        instructionsMenu.Close();
        leaderboardMenu.Close();
    }

    public void StartGame() {
        SceneManager.LoadScene(SceneId.MainGame);
    }

    public void OpenLeaderboard() {
        leaderboardMenu.Open();
    }

    public void OpenInstructions() {
        instructionsMenu.Open();
    }

    public void Quit() {
        Application.Quit();
    }

}
