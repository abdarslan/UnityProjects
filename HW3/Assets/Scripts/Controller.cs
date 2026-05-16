using System;   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;
    public float enginePower = 150f;
    public float turnPower = 50f;
    public float grip = 1f;
    public float driftThresholdAngle = 10f;
    public float driftFactor = 0.5f;
    public int driftIntensity = 0;

    public static event Action OnPlayerDriftStart;
    public static event Action OnPlayerDriftEnd;
    
    [Header("Visuals")]
    //There are trail renderers and particle system attached to both left tyre child and right tyre child of the car prefab, we will assign them in the inspector to these variables so we can control when to turn them on and off in the script
    public TrailRenderer leftDriftTrail;
    public TrailRenderer rightDriftTrail;
    public ParticleSystem leftDriftParticles;
    public ParticleSystem rightDriftParticles;
    
    private bool isDrifting = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // try auto-assigning components from children if they weren't set in Inspector
        if (leftDriftTrail == null || rightDriftTrail == null)
        {
            var trails = GetComponentsInChildren<TrailRenderer>(true);
            foreach (var t in trails)
            {
                var n = t.gameObject.name.ToLower();
                if (leftDriftTrail == null && n.Contains("left")) leftDriftTrail = t;
                else if (rightDriftTrail == null && n.Contains("right")) rightDriftTrail = t;
            }
            if (leftDriftTrail == null && trails.Length > 0) leftDriftTrail = trails[0];
            if (rightDriftTrail == null && trails.Length > 1) rightDriftTrail = trails[1];
        }

        if (leftDriftParticles == null || rightDriftParticles == null)
        {
            var parts = GetComponentsInChildren<ParticleSystem>(true);
            foreach (var p in parts)
            {
                var n = p.gameObject.name.ToLower();
                if (leftDriftParticles == null && n.Contains("left")) leftDriftParticles = p;
                else if (rightDriftParticles == null && n.Contains("right")) rightDriftParticles = p;
            }
            if (leftDriftParticles == null && parts.Length > 0) leftDriftParticles = parts[0];
            if (rightDriftParticles == null && parts.Length > 1) rightDriftParticles = parts[1];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (leftDriftTrail != null) leftDriftTrail.emitting = false;
        if (rightDriftTrail != null) rightDriftTrail.emitting = false;
        if (leftDriftParticles != null) leftDriftParticles.Stop();
        if (rightDriftParticles != null) rightDriftParticles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrifting)
        {
            if (leftDriftTrail != null) leftDriftTrail.emitting = true;
            if (rightDriftTrail != null) rightDriftTrail.emitting = true;
            if (leftDriftParticles != null && !leftDriftParticles.isPlaying) leftDriftParticles.Play();
            if (rightDriftParticles != null && !rightDriftParticles.isPlaying) rightDriftParticles.Play();
        }
        else
        {
            if (leftDriftTrail != null) leftDriftTrail.emitting = false;
            if (rightDriftTrail != null) rightDriftTrail.emitting = false;
            if (leftDriftParticles != null && leftDriftParticles.isPlaying) leftDriftParticles.Stop();
            if (rightDriftParticles != null && rightDriftParticles.isPlaying) rightDriftParticles.Stop();
        }
    }
    void FixedUpdate()
    {
        // this is for driving car physics
        // horizontal input is directly rotating the object on the y axis towards counterclockwise or clockwise direction
        // vertical input is applying force to the object in the forward direction of the object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Rotate(0, horizontalInput * turnPower * Time.fixedDeltaTime, 0);
        rb.AddRelativeForce(Vector3.forward * verticalInput * enginePower);

        float currentSpeed = rb.velocity.magnitude;
        Vector3 targetVelocity = transform.forward * currentSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, grip * Time.fixedDeltaTime);

        // angle of the car is ange between the forward direction of the car and the velocity vector of the car
        float angle = Vector3.Angle(transform.forward, rb.velocity);
        // angle times drift factor times the current speed of the car is the rate we are gonna broadcast to light manager and score manager to calculate the score and the intensity of the drift light effects
        // if the angle is greater than the drift threshold angle, we are in a drift state and brodcast the drift event to the listeners
        if (angle > driftThresholdAngle && verticalInput > 0 && currentSpeed > 2) // we only want to start drifting if the player is pressing the gas pedal, otherwise we might get false positives when the player is just turning in place or reversing
        {
            if (!isDrifting)
            {
                OnPlayerDriftStart?.Invoke();
                isDrifting = true;
            }
            // ange should affect exponentially. 
            this.driftIntensity = Mathf.RoundToInt(Mathf.Pow(angle, 2) * driftFactor * currentSpeed);
        }
        else
        {
            if (isDrifting)
            {
                OnPlayerDriftEnd?.Invoke();
                isDrifting = false;
            }
            this.driftIntensity = 0;
        }
    }
    void manageTire() { }
}
