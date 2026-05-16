using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreSFX : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource;
    public Rigidbody car;
    float maxPitch = 1.0f;
    float minPitch = 0.5f;
    float maxVolume = 1.1f;
    float minVolume = 0.1f;
    public float driftFactor = 4000f;
    public float maxSpeed = 100f;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // when drift starts play the sound and dynamically change the pitch based on the speed of the car
    // make start and end of the drift sound more smooth by fading in and out the volume
    void Update()
    {
        if (audioSource.isPlaying)
        {
            float carAngle = Vector3.Angle(car.velocity, car.transform.forward);
            float carSpeed = car.velocity.magnitude;
            //dont use car angle for pitch
            audioSource.pitch = Mathf.Max(carSpeed / maxSpeed * maxPitch, minPitch);
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, carSpeed * carAngle / driftFactor);
        }
    }
    private void OnEnable() {
        Controller.OnPlayerDriftStart += HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd += HandlePlayerDriftEnd;
    }
    private void OnDisable() {
        Controller.OnPlayerDriftStart -= HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd -= HandlePlayerDriftEnd;
    }
    private void HandlePlayerDriftStart()
    {
        audioSource.Play();
    }
    private void HandlePlayerDriftEnd()
    {
        audioSource.Stop();
    }
}
