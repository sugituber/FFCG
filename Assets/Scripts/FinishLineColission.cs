using UnityEngine;

public class FinishLineColission : MonoBehaviour
{
    public static FinishLineColission instance;
    public Camera carCam;
    public Camera finishCam;
    public TimeLogic TimerOn;
    private bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carCam = CarController.instance.transform.parent.GetChild(0).GetChild(0).GetComponent<Camera>();
        if (instance == null)
        {
            instance = this;
            carCam.enabled = true;
            finishCam.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CrossFinish()
    {
        TimerOn.TimeRunning = false;
        finished = true;
        carCam.enabled = false;
        finishCam.enabled = true;
        Debug.Log("FINISH!");
    }
    
}
