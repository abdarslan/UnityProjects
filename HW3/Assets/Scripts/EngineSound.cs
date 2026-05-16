using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource engineAudioSource;
    public float minPitch = 0.6f;
    public float maxPitch = 1.8f;

    [Header("Engine & Gear Settings")]
    public Rigidbody carRigidbody;
    public float maxSpeed = 30f; // Top speed of the car in m/s
    public int totalGears = 1;

    void Update()
    {
        CalculateEnginePitch();
    }

    void CalculateEnginePitch()
    {
        if (carRigidbody == null || engineAudioSource == null) return;

        // 1. Get current forward speed
        float currentSpeed = Mathf.Abs(Vector3.Dot(carRigidbody.velocity, transform.forward));

        // 2. Determine what percentage of top speed we are currently traveling
        float speedRatio = currentSpeed / maxSpeed;

        // 3. Calculate current gear and the RPM within that gear
        float gearCoverage = 1f / totalGears; // e.g., 5 gears means each gear covers 20% (0.2) of total speed
        
        // Find which gear interval we are currently in
        int currentGear = Mathf.FloorToInt(speedRatio / gearCoverage);
        currentGear = Mathf.Clamp(currentGear, 0, totalGears - 1);

        // 4. Calculate simulated RPM (0.0 to 1.0) within the current gear range
        float gearMinSpeed = currentGear * gearCoverage;
        float gearMaxSpeed = (currentGear + 1) * gearCoverage;
        
        // This gives us a 0-1 value of where the speed is *inside* the current gear
        float engineRpmFactor = Mathf.InverseLerp(gearMinSpeed, gearMaxSpeed, speedRatio);

        // 5. Interpolate pitch based on the current gear's RPM
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, engineRpmFactor);

        // 6. Apply pitch to the audio source
        engineAudioSource.pitch = targetPitch;
    }
}