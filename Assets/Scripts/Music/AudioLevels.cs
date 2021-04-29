using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLevels : MonoBehaviour {

    [SerializeField] private AudioMixer masterMixer;    

    public float GetMusicVolume() {
        masterMixer.GetFloat("MusicVolume", out var volume);
        return volume;
    }

    public float GetSfxVolume() {
        masterMixer.GetFloat("SfxVolume", out var volume);
        return volume;
    }

    public void SetMusicVolume(float vol) {
        masterMixer.SetFloat("MusicVolume", vol);
    }

    public void SetSfxVolume(float vol) {
        masterMixer.SetFloat("SfxVolume", vol);
    }
    
}
