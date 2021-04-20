﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private const float StarsMovementSpeed = 0.3f;

    [SerializeField] private RectTransform stars;
    [SerializeField] private MainMenuPanel instructionsMenu;
    [SerializeField] private MainMenuPanel leaderboardMenu;

    private float _starsLoopProgress = 0f;

    private void Awake() {
        instructionsMenu.Close();
        leaderboardMenu.Close();
        Debug.Log(stars.rect.height);
        Debug.Log(stars.transform.position.y);
    }

    private void Update() {
        // Loop stars background
        var inset = _starsLoopProgress * stars.rect.height;
        stars.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, stars.rect.height);
        
        _starsLoopProgress += Time.deltaTime * StarsMovementSpeed;
        
        if (_starsLoopProgress >= 0.999f) {
            _starsLoopProgress = 0f;
        }
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
