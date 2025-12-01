using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    // Define front and rear axels
    public enum Axel
    {
        Front,
        Rear
    }

    // Create Wheel struct for meshes, wheelColliders, and axels
    [Serializable]
    public struct Wheel
    {
        public GameObject wheelMesh;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    // Define default acceleration force and turning amount
    public float maxAccel = 30f;
    public float breakForce = 50f;
    public float maxSteeringRadius = 30f;
    public float steeringSens = 1f;

    // Get references to car wheels
    public List<Wheel> wheels;

    // x: steering, y: acceleration
    private Vector2 moveInput;

    // Reference to car Rigidbody
    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyAcceleration();
        ApplySteering();
    }
    
    void ApplyAcceleration()
    {
        // Apply force to each wheel (all-wheel drive)
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = -moveInput.y * maxAccel * Time.fixedDeltaTime; 
        }
    }

    void ApplySteering()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = moveInput.x * steeringSens * maxSteeringRadius;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    public void GetInputs(InputAction.CallbackContext ctx)
    {
        // New player input system using events
        moveInput = ctx.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }
}
