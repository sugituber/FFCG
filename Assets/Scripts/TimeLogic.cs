using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class TimeLogic : MonoBehaviour
{
    public TMPro.TextMeshProUGUI countdownText;
    public bool TimeRunning = false;
    private float elapsedTime = 0f;
    public CarController currCar;
    public Rigidbody rb;
    public Vector3 startPos;
    public Quaternion startRot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject selCar = GameFlow.Instance.selectedCar;
        currCar = selCar.GetComponentInChildren<CarController>();
        CarController.instance.tireGripFactor = currCar.tireGripFactor;
        CarController.instance.lowSpeedSteerAngle = currCar.lowSpeedSteerAngle;
        CarController.instance.highSpeedSteerAngle = currCar.highSpeedSteerAngle;
        CarController.instance.accelSpeed = currCar.accelSpeed;
        CarController.instance.accelForce = currCar.accelForce;
        CarController.instance.accelPenalty = currCar.accelPenalty;
        CarController.instance.useAllWheelDrive = currCar.useAllWheelDrive;
        CarController.instance.useRearWheelDrive = currCar.useRearWheelDrive;

        rb = CarController.instance.GetComponent<Rigidbody>();
        startPos = rb.transform.position;
        startRot = rb.transform.localRotation;

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            rb.isKinematic = true;
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "START!";
        rb.isKinematic = false;
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
            rb.isKinematic = true;
            rb.transform.position = startPos;
            rb.transform.localRotation = startRot;
            countdownText.gameObject.SetActive(true);
            
            StartCoroutine(StartCountdown());
        }
    }
        public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void ExitTrack(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene("Main_Menu");
    }
}