using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Orbit")]
    public float distance = 4f;
    public float height = 2f;
    public float mouseSensitivity = 180f;
    public float minPitch = -20f;
    public float maxPitch = 60f;
    public float smoothSpeed = 10f;

    private float yaw;
    private float pitch = 15f;

    private void Start()
    {
        var euler = transform.eulerAngles;
        yaw = euler.y;
        pitch = Mathf.Clamp(euler.x, minPitch, maxPitch);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity * Time.deltaTime;
        pitch -= mouseY * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        var rotation = Quaternion.Euler(pitch, yaw, 0f);
        var lookPoint = target.position + Vector3.up * height;
        var desiredPosition = lookPoint - rotation * Vector3.forward * distance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothSpeed * Time.deltaTime);
    }
}