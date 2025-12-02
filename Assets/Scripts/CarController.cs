using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// Credits to Toyful Games' "Very Very Valet" single Rigidbody raycast approach
// https://www.youtube.com/watch?v=CdPYlj5uZeI
public class CarController : MonoBehaviour
{
    private Vector2 moveInput; // Movement input
    private float currentSteerAngle;

    [Header("Refs")]
    public Rigidbody rb; // Car Rigidbody

    public Transform FrontLeftWheel; // Wheel raycast transforms
    public Transform FrontRightWheel;
    public Transform BackLeftWheel;
    public Transform BackRightWheel;

    public GameObject FrontLeftWheelObj; // Wheel mesh objects
    public GameObject FrontRightWheelObj;
    public GameObject BackLeftWheelObj;
    public GameObject BackRightWheelObj;

    [Header("Suspension")]
    public Vector3 centerOfMass = new(0f, -0.5f, 0f); // Center of mass in local space
    public float suspensionRestDistance = 0.5f; // Suspension rest point
    public float springStrength = 1000f; // Suspension force strength
    public float springDamper = 100f; // Springy/sticky suspension

    [Header("Steering")]
    public float maxSteeringAngle = 30f; // Maximum turning angle
    public float steeringSpeed = 200f; // Gradual steering speed
    public float tireGripFactor = 0.5f; // Slippery/sticky tire grip
    public float tireMass = 10f; // tire weight

    [Header("Engine")]
    public float accelSpeed = 20f; // Maximum velocity
    public float accelForce = 150f; // Torque force

    [Header("Debug")]
    // Gizmo debug
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
        rb.centerOfMass = centerOfMass; // Debug update center of mass
        ApplyWheelForces(FrontLeftWheel);
        ApplyWheelForces(FrontRightWheel);
        ApplyWheelForces(BackLeftWheel);
        ApplyWheelForces(BackRightWheel);
        ApplyWheelRotation(FrontLeftWheel);
        ApplyWheelRotation(FrontRightWheel);
    }

    void ApplyWheelRotation(Transform tire)
    {
        // Gradual steering
        float desiredAngle = moveInput.x * maxSteeringAngle;
        currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, desiredAngle, steeringSpeed * Time.fixedDeltaTime);
        
        // Apply rotation in local space for each front wheel
        FrontLeftWheel.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
        FrontRightWheel.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
        FrontLeftWheelObj.transform.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
        FrontRightWheelObj.transform.localRotation = Quaternion.Euler(0, currentSteerAngle, 0);
    }

    void ApplyWheelForces(Transform tire, bool applyAccel = true)
    {
        // Debug show forces on each wheel
        WheelDebugInfo debugInfo = new WheelDebugInfo();
        debugInfo.suspensionForce = Vector3.zero;
        debugInfo.steeringForce = Vector3.zero;
        debugInfo.accelForce = Vector3.zero;

        RaycastHit hit;
        bool grounded = Physics.Raycast(tire.position, -tire.up, out hit, suspensionRestDistance);

        if (!grounded) return; // Only apply forces when grounded

        // START SUSPENSION
        Vector3 springDirection = tire.up;
        Vector3 tireWorldVelocity = rb.GetPointVelocity(tire.position);

        // Hooke's Law https://en.wikipedia.org/wiki/Hooke%27s_law
        // Calculate how much force to apply based on the current spring offset from
        // resting position and current velocity in the direction of the spring
        float offset = suspensionRestDistance - hit.distance;
        float velocity = Vector3.Dot(springDirection, tireWorldVelocity);
        float force = (offset * springStrength) - (velocity * springDamper);

        rb.AddForceAtPosition(springDirection * force, tire.position);
        // END SUSPENSION

        // START TIRE FRICTION
        Vector3 tireRight = tire.right;
        
        // Find lateral friction on the wheel
        float tireSidewaysVelocity = Vector3.Dot(tireRight, tireWorldVelocity);
        float desiredVelocityChange = -tireSidewaysVelocity * tireGripFactor;
        float steerForce = desiredVelocityChange / Time.fixedDeltaTime * tireMass;

        rb.AddForceAtPosition(tireRight * steerForce, tire.position);
        // END TIRE FRICTION

        // START ACCELERATION
        // Apply force in the forward direction relative to each wheel
        Vector3 tireForward = tire.forward;
        Vector3 forwardForce = tireForward * moveInput.y * accelForce;
        
        if (moveInput.y != 0f && applyAccel) rb.AddForceAtPosition(forwardForce, tire.position);
        // END ACCELERATION

        debugInfo.isGrounded = true;
        debugInfo.rayHitPoint = hit.point;
        debugInfo.suspensionForce = springDirection * force;
        debugInfo.steeringForce = tireRight * steerForce;
        debugInfo.accelForce = forwardForce;
        if (wheelDebugs.ContainsKey(tire))
            wheelDebugs[tire] = debugInfo;
        else
            wheelDebugs.Add(tire, debugInfo);
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
