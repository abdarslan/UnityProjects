using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMeshPro

public class SpeedometerUI : MonoBehaviour
{
    [Header("Car Reference")]
    public Rigidbody carRigidbody;

    [Header("Digital Speedometer")]
    public TextMeshProUGUI speedText;
    public bool useMPH = false; // Toggle between km/h and mph

    [Header("Analog Needle (Optional)")]
    public RectTransform needle;
    public float maxSpeed = 50f; // Max speed of the car in m/s
    public float minNeedleAngle = 135f; // Angle when stopped (bottom left)
    public float maxNeedleAngle = -135f; // Angle at max speed (bottom right)

    void Update()
    {
        if (carRigidbody == null) return;

        // 1. Get the absolute speed in Unity physics units (meters per second)
        // Using magnitude here so it tracks speed regardless of drifting sideways
        float currentSpeedMS = carRigidbody.velocity.magnitude; 

        // 2. Update Digital Text
        if (speedText != null)
        {
            float displaySpeed = useMPH ? currentSpeedMS * 2.237f : currentSpeedMS * 3.6f;
            string unit = useMPH ? " MPH" : " KM/H";
            
            // Mathf.RoundToInt removes decimal places for a clean UI
            speedText.text = Mathf.RoundToInt(displaySpeed).ToString() + unit;
        }

        // 3. Update Analog Needle
        if (needle != null)
        {
            // Find percentage of max speed (clamped so needle doesn't break limits)
            float speedRatio = Mathf.Clamp01(currentSpeedMS / maxSpeed);
            
            // Calculate rotation angle
            float currentAngle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, speedRatio);
            
            // Apply rotation to the Z axis
            needle.localEulerAngles = new Vector3(0, 0, currentAngle);
        }
    }
}