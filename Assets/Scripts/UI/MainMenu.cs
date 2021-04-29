using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour {

    [SerializeField] private float StarsMovementSpeed = 0.3f;

    [SerializeField] private RectTransform stars;
    [SerializeField] private MainMenuPanel instructionsMenu;
    [SerializeField] private MainMenuPanel optionsMenu;
    [FormerlySerializedAs("leaderboardMenu")] [SerializeField] private MainMenuPanel creditsMenu;

    private float _starsLoopProgress = 0f;

    private void Awake() {
        instructionsMenu.Close();
        optionsMenu.Close();
        creditsMenu.Close();
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

    public void OpenOptions() {
        optionsMenu.Open();
    }

    public void OpenCredits() {
        creditsMenu.Open();
    }

    public void OpenInstructions() {
        instructionsMenu.Open();
    }

    public void Quit() {
        Application.Quit();
    }

}
