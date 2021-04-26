using System;
using System.Collections;
using UnityEngine;

public class Thruster : MonoBehaviour {
    
    private const float MAX_THRUSTER_SOUND = 0.8f;
    
    private ParticleSystem _ps;
    private AudioSource _as;
    private float _intensity; // 0-1

    private bool _hasParticles;
    private bool _hasSound;
    
    private void Awake() {
        _ps = GetComponent<ParticleSystem>();
        _as = GetComponent<AudioSource>();
        _hasParticles = _ps != null;
        _hasSound = _as != null;
    }

    private void Start() {
        SetIntensity(0f);
    }

    public void SetIntensity(float intensity) {
        _intensity = intensity;

        if (_hasParticles) {
            var main = _ps.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.3f * intensity, 0.4f * intensity);
        
            var emission = _ps.emission;
            emission.rateOverTime = 200f * intensity;
        }

        if (_hasSound) {
            _as.volume = intensity * MAX_THRUSTER_SOUND;
        }
    }
}
