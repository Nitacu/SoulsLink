using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    private TextMeshProUGUI timer;
    public float maxTimerCountdownMinutes;
    public TextMeshProUGUI scoreChimera1;
    public TextMeshProUGUI scoreChimera2;
    public GameObject winScreen;
    private float secondsToEnd ;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>();
        secondsToEnd = maxTimerCountdownMinutes * 60;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;
        float timeToShow = secondsToEnd - t;
        string minutes = ((int)timeToShow / 60).ToString();
        string seconds = (timeToShow % 60).ToString("0");

        timer.text = minutes + ":" + seconds;

        if(timeToShow <= 0)
        {
            winScreen.SetActive(true);
            if (int.Parse(scoreChimera1.text) > int.Parse(scoreChimera2.text))
            {
                winScreen.GetComponentInChildren<TextMeshProUGUI>().text = "QUIMERA 1 WINS!";
                winScreen.GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
            }
            else if(int.Parse(scoreChimera1.text) < int.Parse(scoreChimera2.text))
            {
                winScreen.GetComponentInChildren<TextMeshProUGUI>().text = "QUIMERA 2 WINS!";
                winScreen.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }
            else if (int.Parse(scoreChimera1.text) == int.Parse(scoreChimera2.text))
            {
                winScreen.GetComponentInChildren<TextMeshProUGUI>().text = "DRAW!";
                winScreen.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
            Destroy(gameObject);
        }
    }
}
