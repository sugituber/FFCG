using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float boostMultiplier = 1.5f;
    public float boostDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            StartCoroutine(ApplyBoost(rb));
        }
    }

    private IEnumerator ApplyBoost(Rigidbody rb)
    {
        //we store the original velocity
        Vector3 originalVelocity = rb.linearVelocity; 

        //we apply a forward boost
        rb.linearVelocity += rb.transform.forward * rb.linearVelocity.magnitude * (boostMultiplier - 1f);

        yield return new WaitForSeconds(boostDuration);

    }
}
