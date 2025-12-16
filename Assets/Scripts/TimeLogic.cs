using UnityEngine;
using System.Collections;

public class TimeLogic : MonoBehaviour
{
    public TMPro.TextMeshProUGUI countdownText;
    public TMPro.TextMeshProUGUI timertext;
    public bool TimeRunning = false;
    private float elapsedTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "START!";
        TimeRunning = true;
        StartTimer();

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeRunning)
            elapsedTime += Time.deltaTime;
            timertext.text = TimeFormat();
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        TimeRunning = true;
    }

    public void StopTimer()
    {
        TimeRunning = false;
    }

    public string TimeFormat()
    {
        int min = (int) (elapsedTime/60f);
        int sec = (int) (elapsedTime % 60f);
        int ms = (int) ((elapsedTime * 1000f) % 1000);
        return $"{min:00}:{sec:00}:{ms:000}";
    }
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
