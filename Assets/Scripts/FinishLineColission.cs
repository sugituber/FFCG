using UnityEngine;

public class FinishLineColission : MonoBehaviour
{
    public TimeLogic TimerOn;
    public float timeToBeat = 24.006f;
    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish line triggered");
            finished = true;
            TimerOn.StopTimer();
            FinishMenu.instance.ShowFinishScreen(timeToBeat);
        }
    }
}
