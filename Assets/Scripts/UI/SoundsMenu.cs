using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioLevels))]
public class SoundsMenu: MonoBehaviour {

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        private AudioLevels _levels;

        private void Awake() {
                _levels = GetComponent<AudioLevels>();
                musicSlider.value = _levels.GetMusicVolume();
                sfxSlider.value = _levels.GetSfxVolume();
        }

}