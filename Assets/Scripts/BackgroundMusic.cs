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
        var random = new Random();
        var trackNum = random.Next(0, trackOptions.Length);
        _src.clip = trackOptions[trackNum];
        _src.loop = true;
        _src.Play();
    }

}