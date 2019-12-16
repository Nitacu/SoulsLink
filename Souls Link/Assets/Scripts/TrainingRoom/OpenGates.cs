using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenGates : MonoBehaviour
{
    public gameStartController leftEntrance;
    public gameStartController rightEntrance;
    public GameObject scoreQuimera1;
    public GameObject scoreQuimera2;
    public GameObject timer;
    public GameObject timerLabel;
    public GameObject labelQuimera1;
    public GameObject labelQuimera2;
    public GameObject startTimer;
    private bool hasStarted = false;
    private float timeToStart = 3;
    private float timeTracker = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeTracker = timeToStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            if(leftEntrance.isSteppingOnEntrance && rightEntrance.isSteppingOnEntrance)
            {
                startTimer.SetActive(true);
                timeTracker -= 1 * Time.deltaTime;
                startTimer.GetComponent<TextMeshProUGUI>().text = timeTracker.ToString("0");
                if(timeTracker <= 0)
                {
                    startGame();
                    hasStarted = true;
                }
            }
            else
            {
                timeTracker = timeToStart;
            }
        }
    }

    private void startGame()
    {
        startTimer.SetActive(false);
        scoreQuimera1.SetActive(true);
        scoreQuimera2.SetActive(true);
        timer.SetActive(true);
        timerLabel.SetActive(true);
        labelQuimera1.SetActive(true);
        labelQuimera2.SetActive(true);
        leftEntrance.entrance.SetActive(false);
        rightEntrance.entrance.SetActive(false);
    }
}
