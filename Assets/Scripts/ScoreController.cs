using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = _score.ToString();
    }

    public void SetScore(int score)
    {
        _score = score;
    }
    
    public void AddScore(int val)
    {
        _score += val;
    }
    
    public void SubScore(int val)
    {
        _score -= val;
    }
}
