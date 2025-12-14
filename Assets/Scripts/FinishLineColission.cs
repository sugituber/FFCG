using UnityEngine;

public class FinishLineColission : MonoBehaviour
{
    public TimeLogic TimerOn;
    public float timeToBeat = 24.006f;

    public Camera finishCamera;
    private bool finished = false;

    void Start()
    {
        finishCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (finished) return;

        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            finished = true;
            TimerOn.StopTimer();
            FinishMenu.instance.ShowFinishScreen(timeToBeat);
        }

    }
}
