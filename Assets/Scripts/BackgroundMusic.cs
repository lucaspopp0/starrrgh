using System;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic: MonoBehaviour {

    [SerializeField] private AudioClip[] trackOptions;
    private AudioSource _src;

    private void Awake() {
        _src = GetComponent<AudioSource>();
    }

    private void Start() {
        var ms = DateTime.Now.Millisecond / 1000f;
        var trackNum = Mathf.FloorToInt(ms * (trackOptions.Length - 0.02f));
        _src.clip = trackOptions[trackNum];
        _src.loop = true;
        _src.Play();
    }

}