using UnityEngine;

public class FinishLineColission : MonoBehaviour
{
    public Transform car;
    public Transform leftflag;
    public Transform rightflag;
    public TimeLogic TimerOn;
    private bool finished = false;
    private float lastSide = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSide = CrossedLine(car.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (finished) return;

        float curr = CrossedLine(car.position);
        // if (car == null || leftflag == null || rightflag == null)
        //     return;

        if (Mathf.Sign(lastSide) != Mathf.Sign(curr))
        {
            TimerOn.TimeRunning = false;
            finished = true;
            Debug.Log("FINISH!");
        }
        lastSide = curr;

    }

    float CrossedLine(Vector3 carPos)
    {
        Vector3 A = leftflag.position;
        Vector3 B = rightflag.position;

        Vector3 AB = B - A;
        Vector3 AC = carPos - A;
        float t = (AB.x * AC.z) - (AB.z * AC.x);
        return t;
    }
        // if (leftflag.position.x == rightflag.position.x)
        // {
        //     if (car.position.x > leftflag.position.x && car.position.x < rightflag.position.x || car.position.x < leftflag.position.x && car.position.x > rightflag.position.x)
        //         TimerOn.TimeRunning = false;
        // }
        // else if (leftflag.position.z == rightflag.position.z)
        // {
        //     if (car.position.z > leftflag.position.z && car.position.z < rightflag.position.z || car.position.z < leftflag.position.z && car.position.z > rightflag.position.z)
        //         TimerOn.TimeRunning = false;
        // }


    
}
