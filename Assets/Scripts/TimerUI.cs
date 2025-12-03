using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TimeLogic raceTimer;
    public TMPro.TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (raceTimer.TimeRunning)
            timerText.text = raceTimer.TimeFormat();
    }
}
