using System;
using System.Collections;
using UnityEngine;

public class Thruster : MonoBehaviour {
    
    private const float MAX_THRUSTER_SOUND = 0.8f;

    [SerializeField] private ParticleSystem forwardParticles;
    [SerializeField] private ParticleSystem reverseParticles;
    
    private AudioSource _as;
    private float _intensity; // 0-1

    private bool _hasForwardParticles;
    private bool _hasReverseParticles;
    private bool _hasSound;
    
    private void Awake() {
        _as = GetComponent<AudioSource>();
        _hasForwardParticles = forwardParticles != null;
        _hasReverseParticles = reverseParticles != null;
        _hasSound = _as != null;
    }

    private void Start() {
        SetIntensity(0f);
    }

    public void SetIntensity(float intensity) {
        _intensity = intensity;

        if (_hasForwardParticles) {
            if (intensity > 0f) {
                var main = forwardParticles.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(0.3f * intensity, 0.4f * intensity);

                var emission = forwardParticles.emission;
                emission.rateOverTime = 200f * intensity;
            } else {
                var emission = forwardParticles.emission;
                emission.rateOverTime = 0f;
            }
        }

        if (_hasReverseParticles) {
            if (intensity < 0f) {
                var main = reverseParticles.main;
                main.startColor = new Color(1f, 1f, 1f, -intensity);

                var emission = reverseParticles.emission;
                emission.rateOverTime = 200f * -intensity;
            } else {
                var emission = reverseParticles.emission;
                emission.rateOverTime = 0f;
            }
        }

        if (_hasSound) {
            _as.volume = Mathf.Abs(intensity) * MAX_THRUSTER_SOUND;
        }
    }
}
