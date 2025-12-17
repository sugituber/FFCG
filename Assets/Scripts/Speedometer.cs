using UnityEngine;

public class Speedometer : MonoBehaviour

{
    public TMPro.TextMeshProUGUI speedtext;
    [Tooltip("Assign the Rigidbody you want to track.")]
    public Rigidbody rb;

    [Tooltip("Current speed in units per second.")]
    public float currentSpeed;

    void Update()
    {
        if (rb != null)
        {
            currentSpeed = rb.linearVelocity.magnitude;
            speedtext.text = Mathf.RoundToInt(currentSpeed*5).ToString();
        }
        else
        {
            currentSpeed = 0f;
        }
    }
}
