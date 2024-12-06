using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    public TextMeshPro timerText;

    void Start()
    {
        if (timerText == null)
        {
            timerText = GameObject.Find("GameTimer").GetComponent<TextMeshPro>();
        }
    }

    void Awake()
    {
        //timerText = GameObject.Find("TimerText");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    // Display the time in minutes and seconds
    public void DisplayTime(float timeToDisplay)
    {

        timeToDisplay = Mathf.Clamp(timeToDisplay, 0, Mathf.Infinity);

        int seconds = Mathf.FloorToInt(timeToDisplay);

        if (timerText != null)
        {
          
            int minutes = Mathf.FloorToInt(seconds / 60); // Calculate minutes
            int second = Mathf.FloorToInt(seconds % 60); // Calculate seconds
            timerText.text = $"Time Left: {minutes:D2}:{second:D2}";
            //timerText.text = "Time Left:" + seconds.ToString();
        }
    }


}

