using UnityEngine;
using TMPro;

public class SongDisplay: MonoBehaviour {

    [SerializeField] private TMP_Text titleField;
    [SerializeField] private TMP_Text artistField;
    private Song _song;

    public void SetSong(Song song) {
        _song = song;
        titleField.text = song.title;
        artistField.text = song.artist;
    }

}
