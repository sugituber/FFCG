using UnityEngine;
using System.Collections;

public class RaceStart : MonoBehaviour
{
    
    public TMPro.TextMeshProUGUI countdownText;
    public TMPro.TextMeshProUGUI timerText;
    private float raceTime = 0f;
    public bool raceStarted = false;
    public CarController car;

    void Start()
    {
        Debug.Log("RaceStart Start() is running!");
        car.enabled = false;
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
        raceStarted = true;
        car.enabled = true;

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (raceStarted)
        {
            raceTime += Time.deltaTime;
            int min = (int) (raceTime/60f);
            int sec = (int) (raceTime % 60f);
            int ms = (int) ((raceTime * 1000f) % 1000);
            timerText.text = $"{min:00}:{sec:00}.{ms:000}";
        }
    }
}