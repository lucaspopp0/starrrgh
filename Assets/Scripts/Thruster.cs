using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Thruster : MonoBehaviour {
    
    private ParticleSystem _ps;
    private float _intensity; // 0-1

    private void Awake() {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Start() {
        SetIntensity(0f);
    }

    public void SetIntensity(float intensity) {
        _intensity = intensity;

        var main = _ps.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.3f * intensity, 0.4f * intensity);
        
        var emission = _ps.emission;
        emission.rateOverTime = 200f * intensity;
    }
}
