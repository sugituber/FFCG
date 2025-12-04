using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    private Vector2 moveInput; 
    private float currentSteerAngle;

    [Header("Refs")]
    public Rigidbody rb;
    public Transform FrontLeftWheel, FrontRightWheel;
    public Transform BackLeftWheel, BackRightWheel;
    public GameObject FrontLeftWheelObj, FrontRightWheelObj;
    public GameObject BackLeftWheelObj, BackRightWheelObj;

    [Header("Suspension")]
    public Vector3 centerOfMass = new(0f, -0.1f, 0f);
    public float suspensionRestDistance = 0.25f;
    public float springStrength = 30000f;
    public float springDamper = 2500f;

    [Header("Steering & Grip")]
    public float lowSpeedSteerAngle = 30f; // Tight turning at low speed
    public float highSpeedSteerAngle = 5f; // Gentle turning at high speed
    public float speedForMinSteer = 10f;    // At 50 speed, we only use highSpeedSteerAngle
    public float steeringSpeed = 100f;
    [Range(0f, 1f)] public float tireGripFactor = 0.8f; // 0.8 is usually a sweet spot
    public float tireMass = 100f; // Ensure this is high enough!

    [Header("Engine")]
    public float accelSpeed = 30f; // Max Speed
    public float accelForce = 2000f; 
    [Range(0f, 1f)] public float airDrag = 0.02f; // Stops car from coasting forever

    [Header("Drive Type")]
    public bool useAllWheelDrive = true;
    public bool useRearWheelDrive = false; 
    // If both false, it defaults to Front Wheel Drive

    [Header("Debug")]
    public bool showGizmos = true;
    public float forceVisualScale = 2000f;

    // Debug Struct
    private struct WheelDebugInfo
    {
        public bool isGrounded;
        public Vector3 rayHitPoint;
        public Vector3 suspensionForce;
        public Vector3 steeringForce;
        public Vector3 accelForce;
    }
    private Dictionary<Transform, WheelDebugInfo> wheelDebugs = new Dictionary<Transform, WheelDebugInfo>();

    void Start()
    {
        rb.centerOfMass = centerOfMass;
    }
    
    void FixedUpdate()
    {
        // 1. Determine Drive Type
        bool frontPower = useAllWheelDrive || (!useRearWheelDrive); // Default to FWD if nothing selected
        bool rearPower = useAllWheelDrive || useRearWheelDrive;

        // 2. Apply Forces
        ApplyWheelForces(FrontLeftWheel, frontPower);
        ApplyWheelForces(FrontRightWheel, frontPower);
        ApplyWheelForces(BackLeftWheel, rearPower);
        ApplyWheelForces(BackRightWheel, rearPower);

        // 3. Apply Steering
        ApplyWheelRotation(FrontLeftWheel, FrontLeftWheelObj);
        ApplyWheelRotation(FrontRightWheel, FrontRightWheelObj);
        
        // (Optional) Update Back Wheel Visuals just to follow the car body
        // You can add visual rotation logic here if you want them to spin

        // 4. Apply Air Drag (This fixes the "floating in space" feeling)
        if (rb.linearVelocity.magnitude > 0.1f)
        {
             // Uses velocity squared for natural air resistance
             rb.AddForce(-rb.linearVelocity * rb.linearVelocity.magnitude * airDrag);
        }

        Debug.Log(rb.linearVelocity);
    }

    void ApplyWheelRotation(Transform tireRaycast, GameObject tireMesh)
    {
        // 1. Calculate current forward speed (absolute value so reversing works too)
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float speedFactor = Mathf.InverseLerp(0f, speedForMinSteer, Mathf.Abs(currentSpeed));

        // 2. Determine the dynamic max angle based on speed
        // If speed is 0, we get lowSpeedSteerAngle. If speed is high, we get highSpeedSteerAngle.
        float currentMaxAngle = Mathf.Lerp(lowSpeedSteerAngle, highSpeedSteerAngle, speedFactor);

        // 3. Calculate target angle
        float desiredAngle = moveInput.x * currentMaxAngle;
        
        // 4. Smoothly rotate towards that angle
        currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, desiredAngle, steeringSpeed * Time.fixedDeltaTime);
        
        // Rotate the Raycast Point (Physics)
        tireRaycast.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
        
        // Rotate the Mesh (Visual)
        if(tireMesh != null)
            tireMesh.transform.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
    }

    void ApplyWheelForces(Transform tire, bool applyAccel)
    {
        WheelDebugInfo debugInfo = new WheelDebugInfo();
        RaycastHit hit;

        // Check if tire is on the ground
        bool grounded = Physics.Raycast(tire.position, -tire.up, out hit, suspensionRestDistance);

        if (!grounded) 
        {
            // Stop car rotation if braking
            if (moveInput.y < 0) rb.angularVelocity = new(0,0,0);
            // If in air, clear debugs and return
            if (wheelDebugs.ContainsKey(tire)) wheelDebugs.Remove(tire);
            return; 
        }

        // --- A. SUSPENSION ---
        Vector3 springDirection = tire.up;
        Vector3 tireWorldVelocity = rb.GetPointVelocity(tire.position);

        float offset = suspensionRestDistance - hit.distance;
        float velocity = Vector3.Dot(springDirection, tireWorldVelocity);
        float force = (offset * springStrength) - (velocity * springDamper);
        
        Vector3 suspensionForce = springDirection * force;
        rb.AddForceAtPosition(suspensionForce, tire.position);

        // --- B. STEERING / GRIP ---
        Vector3 tireRight = tire.right;
        float tireSidewaysVelocity = Vector3.Dot(tireRight, tireWorldVelocity);
        
        float desiredVelocityChange = -tireSidewaysVelocity * tireGripFactor;
        float steerForceVal = desiredVelocityChange / Time.fixedDeltaTime * tireMass;
        
        Vector3 steeringForce = tireRight * steerForceVal;
        rb.AddForceAtPosition(steeringForce, tire.position);

        // --- C. ACCELERATION (With Speed Limit) ---
        Vector3 tireForward = tire.forward;
        Vector3 accelForceVector = Vector3.zero;

        if (applyAccel)
        {
            float currentSpeed = Vector3.Dot(tireWorldVelocity, tireForward);
            bool canAccelerate = false;

            if (moveInput.y > 0) // Moving Forward
                canAccelerate = currentSpeed < accelSpeed;
            else if (moveInput.y < 0) // Reversing
                canAccelerate = currentSpeed > -accelSpeed;

            if (canAccelerate)
            {
                accelForceVector = tireForward * moveInput.y * accelForce;
                rb.AddForceAtPosition(accelForceVector, tire.position);
            }
        }

        // Save Debug Data
        debugInfo.isGrounded = true;
        debugInfo.rayHitPoint = hit.point;
        debugInfo.suspensionForce = suspensionForce;
        debugInfo.steeringForce = steeringForce;
        debugInfo.accelForce = accelForceVector;

        if (wheelDebugs.ContainsKey(tire)) wheelDebugs[tire] = debugInfo;
        else wheelDebugs.Add(tire, debugInfo);
    }

    public void GetInputs(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnDrawGizmos()
    {
        if (!showGizmos || rb == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rb.worldCenterOfMass, 0.2f);

        DrawWheelGizmos(FrontLeftWheel);
        DrawWheelGizmos(FrontRightWheel);
        DrawWheelGizmos(BackLeftWheel);
        DrawWheelGizmos(BackRightWheel);
    }

    void DrawWheelGizmos(Transform tireTransform)
    {
        if (tireTransform == null || !wheelDebugs.ContainsKey(tireTransform)) return;

        WheelDebugInfo info = wheelDebugs[tireTransform];
        Vector3 origin = tireTransform.position;

        Gizmos.color = info.isGrounded ? Color.red : Color.white;
        Gizmos.DrawLine(origin, origin - (tireTransform.up * suspensionRestDistance));

        if (info.isGrounded)
        {
            Gizmos.color = Color.green; // Susp
            Gizmos.DrawRay(origin, info.suspensionForce / forceVisualScale);

            Gizmos.color = Color.red; // Grip
            Gizmos.DrawRay(origin, info.steeringForce / forceVisualScale);

            Gizmos.color = Color.blue; // Accel
            Gizmos.DrawRay(origin, info.accelForce / forceVisualScale);
        }
    }
}