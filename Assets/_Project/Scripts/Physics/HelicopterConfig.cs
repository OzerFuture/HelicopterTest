using UnityEngine;

[CreateAssetMenu(fileName = "HelicopterConfig", menuName = "Scriptable Objects/HelicopterConfig")]
public class HelicopterConfig : ScriptableObject
{
    [Header("Rigidbody")]
    public float helicopterMass = 1000f;
    public float angularDamping = 3f;
    public Vector3 centerOfMassOffset = new Vector3(0f, -1f, 0f);
    public bool useCustomInertia = true;
    public Vector3 customInertiaTensor = new Vector3(150f, 55f, 120f);

    [Header("Control & Movement")]
    public float climbAcceleration = 8f;     
    public float yawAcceleration = 0.1f;    
    
    [Header("Engine & Rotor")]
    public float engineAcceleration = 0.3f;
    public float engineDeceleration = 0.3f; 
    public float maxRpm = 10f;             

    [Header("Aerodynamics & Drag")]
    public float forwardDrag = 0.3f;     
    public float sideDrag = 1.5f;         
    public float verticalDrag = 1f;    

    [Header("Attitude & Pitch/Roll")]
    public float maxTiltAngle = 35f;      
    public float tiltAcceleration = 1f;        
    public float autoStabilizationSpeed = 0.7f; 

    [Header("Atmosphere & Altitude limit")]
    public float minCeiling = 50f;       
    public float maxCeiling = 100f;        
}
