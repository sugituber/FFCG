using UnityEngine;

public class FinishLineColission : MonoBehaviour
{
    public static FinishLineColission instance;
    public Transform car;
    private Collider finishblock;
    public TimeLogic TimerOn;
    private bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
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
        Debug.Log("FINISH!");
    }
    
}
