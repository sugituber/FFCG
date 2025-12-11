using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class TimeLogic : MonoBehaviour
{
    public TMPro.TextMeshProUGUI countdownText;
    public bool TimeRunning = false;
    private float elapsedTime = 0f;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("RaceStart Start() is running!");
        if (CarController.instance != null)
        {
            rb = CarController.instance.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        startPosition = rb.transform.position;
        startRotation = rb.transform.localRotation;
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
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        StartTimer();

        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeRunning)
            elapsedTime += Time.deltaTime;
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

    public void RestartTrack(InputAction.CallbackContext ctx)
    {
        if (TimeRunning)
        {
            TimeRunning = false;
            elapsedTime = 0f;
            rb.transform.position = startPosition;
            rb.transform.localRotation = startRotation;
            rb.isKinematic = true;
            countdownText.gameObject.SetActive(true);
            
            StartCoroutine(StartCountdown());
        }
    }
}