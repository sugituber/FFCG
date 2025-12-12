using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float boostAmount = 0.2f;
    public float boostDuration = 1f;
    public float fovIncrease = 15f;
    public float fovSpeed = 5f;

    private void OnTriggerEnter(Collider other)
    {
        CarController car = other.GetComponent<CarController>();

        if (car != null)
        {
            StartCoroutine(ApplyBoost(car));
            StartCoroutine(FOVEffect(boostDuration));

        }
    }

IEnumerator ApplyBoost(CarController car)
{
    //we save the original values
    float originalForce = car.accelForce;
    float originalModifier = car.accelModifier;

    car.accelForce *= 1.05f;
    car.accelModifier *= 1.02f;

    yield return new WaitForSeconds(boostDuration);

    //we restore the values
    car.accelForce = originalForce;
    car.accelModifier = originalModifier;
}
    IEnumerator FOVEffect(float duration)
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;

        float startFOV = cam.fieldOfView;
        float targetFOV = startFOV + fovIncrease;

        float t = 0f;
        while (t < 1f)
        {
            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            t += Time.deltaTime * fovSpeed;
            yield return null;
        }
        cam.fieldOfView = targetFOV;

        // Stay zoomed for boost duration
        yield return new WaitForSeconds(duration);

        t = 0f;
        while (t < 1f)
        {
            cam.fieldOfView = Mathf.Lerp(targetFOV, startFOV, t);
            t += Time.deltaTime * fovSpeed;
            yield return null;
        }
        cam.fieldOfView = startFOV;
    }
}
