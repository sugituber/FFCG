using UnityEngine;

public class FinishLineColission : MonoBehaviour
{
    public static FinishLineColission instance;
    public Transform car;
    // public Transform leftflag;
    // public Transform rightflag;
    private Collider finishblock;
    public TimeLogic TimerOn;
    private bool finished = false;
    // private float lastSide = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        // finishblock = GetComponet<Collider>();
        // lastSide = CrossedLine(car.position);
    }

    // Update is called once per frame
    void Update()
    {
        // if (finished) return;

        
        // float curr = CrossedLine(car.position);
        // // if (car == null || leftflag == null || rightflag == null)
        // //     return;

        // if (Mathf.Sign(lastSide) != Mathf.Sign(curr))
        // {
        //     TimerOn.TimeRunning = false;
        //     finished = true;
        //     Debug.Log("FINISH!");
        // }
        // lastSide = curr;

    }

    public void CrossFinish()
    {
        TimerOn.TimeRunning = false;
        finished = true;
        Debug.Log("FINISH!");
    }
    // float CrossedLine(Vector3 carPos)
    // {
    //     Vector3 A = leftflag.position;
    //     Vector3 B = rightflag.position;

    //     Vector3 AB = B - A;
    //     Vector3 AC = carPos - A;
    //     float t = (AB.x * AC.z) - (AB.z * AC.x);
    //     return t;
    // }
    
}
