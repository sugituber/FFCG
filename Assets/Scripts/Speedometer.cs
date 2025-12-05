using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public Rigidbody car;
    public TMPro.TextMeshProUGUI speedometer;
    public float speedScale = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float forwardSpeed = Vector3.Dot(car.linearVelocity, car.transform.forward);
        float speedValue = forwardSpeed * speedScale;
        speedometer.text = Mathf.FloorToInt(speedValue).ToString();
    }
}
