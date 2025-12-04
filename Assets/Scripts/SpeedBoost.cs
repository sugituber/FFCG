using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 1f;

    private void OnTriggerEnter(Collider other)
    {
        CarController car = other.GetComponent<CarController>();
        if (car != null)
        {
            StartCoroutine(BoostCoroutine(car));
        }
    }

    private IEnumerator BoostCoroutine(CarController car)
    {
        float originalAccelForce = car.accelForce;
        float originalAccelSpeed = car.accelSpeed;

        car.accelForce *= boostMultiplier;
        car.accelSpeed *= boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        car.accelForce = originalAccelForce;
        car.accelSpeed = originalAccelSpeed;
    }
}
