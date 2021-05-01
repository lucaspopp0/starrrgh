using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    private Hud _hud;

    private int _score;
    // Start is called before the first frame update
    void Start() {
        _hud = GameObject.FindWithTag("HUD").GetComponent<Hud>();
    }
    
    public void AddScore(int val)
    {
        _score += val;
        _hud.SetScore(_score);
        RunStats.Current.Score += val;
    }
    
    public void SubScore(int val)
    {
        _score -= val;
        _hud.SetScore(_score);
        RunStats.Current.Score -= val;
    }
}
