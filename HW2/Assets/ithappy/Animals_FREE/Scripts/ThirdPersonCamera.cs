using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Drag your Cat here

    [Header("Camera Positioning")]
    public Vector3 offset = new Vector3(0f, 2.5f, -4f); // X, Y (Height), Z (Distance back)
    public float smoothSpeed = 5f; // How quickly it catches up

    void LateUpdate()
    {
        // Don't do anything if we haven't assigned the cat yet
        if (target == null) return;

        // 1. Calculate where the camera SHOULD be
        // THE FIX: Multiplying by target.rotation forces the offset to turn WITH the cat
        Vector3 desiredPosition = target.position + (target.rotation * offset);

        // 2. Smoothly glide the camera to that position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 3. Make sure the camera is always looking at the cat (slightly above its feet)
        transform.LookAt(target.position + Vector3.up * 1f); 
    }
}