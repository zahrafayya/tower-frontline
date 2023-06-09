using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    [SerializeField] public GameObject scoreUI;
    [SerializeField] public Spawner spawner;
    
    private TextMeshProUGUI scoreText;
    private int score = 0;
    void Start()
    {
        if (scoreUI)
            scoreText = scoreUI.GetComponent<TextMeshProUGUI>();
    }

    public void IncreaseScore()
    {
        score++;
        
        if(scoreText)
            scoreText.text = score.ToString();  
        
        if (score == 3) spawner.IncreaseMaxCoin(15);
        if (score == 7) spawner.IncreaseMaxCoin(20);
        if (score == 15) spawner.IncreaseMaxCoin(30);
        if (score == 21) spawner.IncreaseMaxCoin(35);
    }
}
