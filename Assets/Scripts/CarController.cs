using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// Credits to Toyful Games' "Very Very Valet" single Rigidbody raycast approach
public class CarController : MonoBehaviour
{
    private Vector2 moveInput; // Movement input
    private float currentSteerAngle;

    [Header("Refs")]
    public Rigidbody rb; // Car Rigidbody
    public Transform FrontLeftWheel;
    public Transform FrontRightWheel;
    public Transform BackLeftWheel;
    public Transform BackRightWheel;

    [Header("Suspension")]
    public Vector3 centerOfMass = new(0f, -0.5f, 0f); // Center of mass in local space
    public float suspensionRestDistance = 0.5f; // Suspension rest point
    public float springStrength = 1000f; // Suspension force strength
    public float springDamper = 100f; // Springy/sticky suspension

    [Header("Steering")]
    public float maxSteeringAngle = 30f; // Maximum turning angle
    public float steeringSpeed = 200f;
    public float tyreGripFactor = 0.5f; // Slippery/sticky tyre grip
    public float tyreMass = 10f; // Tyre weight

    [Header("Engine")]
    public float accelSpeed = 20f; // Maximum velocity
    public float accelForce = 150f; // Torque force

    [Header("Debug")]
    public bool showGizmos = true;
    public float forceVisualScale = 1000f;
    
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
        rb.centerOfMass = centerOfMass;
        ApplyWheelForces(FrontLeftWheel);
        ApplyWheelForces(FrontRightWheel);
        ApplyWheelForces(BackLeftWheel);
        ApplyWheelForces(BackRightWheel);
        ApplyWheelRotation(FrontLeftWheel);
        ApplyWheelRotation(FrontRightWheel);
    }

    void ApplyWheelRotation(Transform tyre)
    {
        float desiredAngle = moveInput.x * maxSteeringAngle;
        currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, desiredAngle, steeringSpeed * Time.fixedDeltaTime);

        FrontLeftWheel.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
        FrontRightWheel.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
    }

    void ApplyWheelForces(Transform tyre)
    {
        WheelDebugInfo debugInfo = new WheelDebugInfo();
        debugInfo.suspensionForce = Vector3.zero;
        debugInfo.steeringForce = Vector3.zero;
        debugInfo.accelForce = Vector3.zero;

        RaycastHit hit;
        bool grounded = Physics.Raycast(tyre.position, -tyre.up, out hit, suspensionRestDistance);

        if (!grounded) // Only apply forces when grounded
        {
            return;
        }

        debugInfo.isGrounded = true;
        debugInfo.rayHitPoint = hit.point;

        // START SUSPENSION
        Vector3 springDirection = tyre.up;
        Vector3 tyreWorldVelocity = rb.GetPointVelocity(tyre.position);

        // Hooke's Law
        float offset = suspensionRestDistance - hit.distance;
        float velocity = Vector3.Dot(springDirection, tyreWorldVelocity);
        float force = (offset * springStrength) - (velocity * springDamper);

        rb.AddForceAtPosition(springDirection * force, tyre.position);

        debugInfo.suspensionForce = springDirection * force;
        // END SUSPENSION

        // START TYRE FRICTION
        Vector3 tyreRight = tyre.right;
        float tyreSidewaysVelocity = Vector3.Dot(tyreRight, tyreWorldVelocity);

        float desiredVelocityChange = -tyreSidewaysVelocity * tyreGripFactor;
        float steerForce = desiredVelocityChange / Time.fixedDeltaTime * tyreMass;

        rb.AddForceAtPosition(tyreRight * steerForce, tyre.position);

        debugInfo.steeringForce = tyreRight * steerForce;
        // END TYRE FRICTION

        // START ACCELERATION
        Vector3 tyreForward = tyre.forward;

        if (moveInput.y != 0f)
        {
            Vector3 forwardForce = tyreForward * moveInput.y * accelForce;
            rb.AddForceAtPosition(forwardForce, tyre.position);

            debugInfo.accelForce = forwardForce;
        }
        // END ACCELERATION

        if (wheelDebugs.ContainsKey(tyre))
            wheelDebugs[tyre] = debugInfo;
        else
            wheelDebugs.Add(tyre, debugInfo);
    }

    // Get PlayerInput as Vector2(x: steering, y: acceleration)
    public void GetInputs(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

        // --- GIZMOS ---
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        // 1. Draw Center of Mass
        if (rb != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(rb.worldCenterOfMass, 0.2f);
            Gizmos.DrawWireSphere(rb.worldCenterOfMass, 0.21f); // outline
        }

        // 2. Draw Wheel Forces
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

        // Draw Raycast Limit (White = Air, Red = Hit Ground)
        Gizmos.color = info.isGrounded ? Color.red : Color.white;
        Gizmos.DrawLine(origin, origin - (tireTransform.up * suspensionRestDistance));

        if (info.isGrounded)
        {
            // Draw Hit Point
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(info.rayHitPoint, 0.05f);

            // Draw Suspension Force (Green)
            // We scale the length down because forces are usually 5000+ units
            if (info.suspensionForce.magnitude > 1f)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, info.suspensionForce / forceVisualScale);
            }

            // Draw Steering Force (Red)
            if (info.steeringForce.magnitude > 1f)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, info.steeringForce / forceVisualScale);
            }

            // Draw Acceleration Force (Blue)
            if (info.accelForce.magnitude > 1f)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(origin, info.accelForce / forceVisualScale);
            }
        }
    }
}
