using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextPopup: MonoBehaviour {

    private TMP_Text _text;
    private bool _currentlyActive;
    private Color _currentColor;
    private IEnumerator _currentCoroutine;
    private float _duration;

    private void Start() {
        _text = GetComponent<TMP_Text>();
        _text.color = Color.clear;
    }

    public void DisplayPopup(string message, Color color, float duration = 0.5f) {
        _currentColor = color;

        Debug.Log(message);

        if (_currentlyActive) {
            StopCoroutine(_currentCoroutine);
        }

        _duration = duration;
        _text.text = message;
        _currentCoroutine = AnimatePopup();
        _currentlyActive = true;
        StartCoroutine(_currentCoroutine);
    }

    private IEnumerator AnimatePopup() {
        var progress = 0f;
        var color = _currentColor;
        
        while (progress < _duration) {
            color.a = _currentColor.a * (progress / _duration);
            _text.color = color;
            progress += Time.deltaTime;
            yield return null;
        }

        _text.color = Color.clear;
        _currentlyActive = false;
    }

}