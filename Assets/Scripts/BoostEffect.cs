using System;
using System.Diagnostics;
using UnityEngine;

public class BoostEffect: MonoBehaviour {

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private ParticleSystem glows;

        public void Play() {
                audioSource.Play();
                particles.Play();
                glows.Play();
        }

        public void Stop() {
                particles.Stop();
                glows.Stop();
        }

}