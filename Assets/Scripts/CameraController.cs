using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject car;
    private Vector3 _currentPositionVelocity = Vector3.zero;
    private float _currentRotationVelocity = 0f;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            car.transform.position,
            ref _currentPositionVelocity,
            0.1f
        );

        float angleDelta = Quaternion.Angle(transform.rotation, car.transform.rotation);

        if (angleDelta > 0f)
        {
            float t = Mathf.SmoothDampAngle(angleDelta, 0.0f, ref _currentRotationVelocity, 0.2f);
            t = 1.0f - (t / angleDelta);
            transform.rotation = Quaternion.Slerp(transform.rotation, car.transform.rotation, t);
        }
    }
}
