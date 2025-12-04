using UnityEngine;
using TMPro;
using System.Collections;

public class RaceStart : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    private float raceTime = 0f;
    private bool raceStarted = false;
    public CarController car;

    void Start()
    { 
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
            timerText.text = raceTime.ToString("F2");
        }
    }
}
