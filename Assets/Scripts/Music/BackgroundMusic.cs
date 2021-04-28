using System;
using UnityEngine;
using Random = System.Random;

[Serializable]

public class Song {

    public string title;
    public string artist;
    public AudioClip clip;

    public Song(string title, string artist, AudioClip clip) {
        this.title = title;
        this.artist = artist;
        this.clip = clip;
    }

}

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic: MonoBehaviour {

    [SerializeField] private SongDisplay songDisplay;
    [SerializeField] private Song[] songs;
    private AudioSource _src;

    private void Awake() {
        _src = GetComponent<AudioSource>();
    }

    private void Start() {
        var ms = DateTime.Now.Millisecond / 1000f;
        var trackNum = Mathf.FloorToInt(ms * (songs.Length - 0.02f));
        _src.clip = songs[trackNum].clip;
        _src.loop = true;
        _src.Play();

        if (songDisplay != null) {
            songDisplay.SetSong(songs[trackNum]);
        }
    }

}