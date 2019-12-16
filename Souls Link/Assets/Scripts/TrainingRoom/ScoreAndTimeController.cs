using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreAndTimeController : MonoBehaviour
{
    private float score = 0;
    public TextMeshProUGUI scoreUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScore()
    {
        score++;
        if(score < 10)
        {
            scoreUI.text = "0" + score.ToString();
        }
        else
        {
            scoreUI.text = score.ToString();
        }
        
    }
}
