using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player;
    public Rigidbody playerRigidbody;

    [Header("Position Settings")]
    public float distance = 6f;
    public float height = 3f;
    public float positionDamping = 5f;
    public float rotationDamping = 3f;

    [Header("Zoom / FOV Settings")]
    public float baseFOV = 60f;
    public float maxFOV = 85f;
    public float minFOV = 50f;
    public float speedForMaxFOV = 30f; // Velocity magnitude where FOV is maximum
    public float fovDamping = 5f;

    private Camera cam;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (player != null && playerRigidbody == null)
        {
            playerRigidbody = player.GetComponent<Rigidbody>();
        }

        cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.fieldOfView = baseFOV;
        }
    }

    private Vector3 positionVelocity = Vector3.zero;
    private float currentSpeed;

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Loose Rotation Tracking
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = player.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, rotationDamping * Time.deltaTime);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        // 2. Position Tracking - using SmoothDamp to eliminate physics jitter
        Vector3 desiredPosition = player.position - (rotation * Vector3.forward * distance);
        desiredPosition.y = player.position.y + height;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionVelocity, 1f / positionDamping);

        // 3. Look at Target
        transform.LookAt(player.position + Vector3.up * (height * 0.3f));

        // 4. FOV Zoom based on Velocity
        if (cam != null && playerRigidbody != null)
        {
            // Smooth the raw velocity reading to prevent FOV vibration when wheels bounce
            float rawSpeed = playerRigidbody.velocity.magnitude;
            currentSpeed = Mathf.Lerp(currentSpeed, rawSpeed, Time.deltaTime * 2f);

            float targetFOV = Mathf.Lerp(baseFOV, maxFOV, currentSpeed / speedForMaxFOV);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovDamping * Time.deltaTime);
        }
    }
}
