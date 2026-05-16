using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightManager : MonoBehaviour
{
    public Light leftHeadlight;
    public Light rightHeadlight;
    public float maxIntensity = 2f;
    public float maxRange = 14f;
    public float minRange = 2f;
    public float decreaseRate = 0.1f;
    public float increaseMultiplier = 0.2f;
    // Start is called before the first frame update
    private bool isDrifting = false;
    Controller controller;
    void Start()
    {
        controller = FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrifting)
        {
            leftHeadlight.intensity = Math.Max(0, leftHeadlight.intensity - Time.deltaTime * decreaseRate);
            rightHeadlight.intensity = Math.Max(0, rightHeadlight.intensity - Time.deltaTime * decreaseRate);
            leftHeadlight.range = Math.Max(minRange, leftHeadlight.range - Time.deltaTime * decreaseRate);
            rightHeadlight.range = Math.Max(minRange, rightHeadlight.range - Time.deltaTime * decreaseRate);
        } else
        {
            float intensity = controller.driftIntensity;
            leftHeadlight.intensity += Math.Min(intensity * increaseMultiplier*Time.deltaTime, maxIntensity-leftHeadlight.intensity);
            rightHeadlight.intensity += Math.Min(intensity * increaseMultiplier*Time.deltaTime, maxIntensity-rightHeadlight.intensity);
            leftHeadlight.range += Math.Min(intensity * increaseMultiplier*Time.deltaTime, maxRange-leftHeadlight.range);
            rightHeadlight.range += Math.Min(intensity * increaseMultiplier*Time.deltaTime, maxRange-rightHeadlight.range);
        }
    }

    private void OnEnable() {
        Controller.OnPlayerDriftEnd += HandlePlayerDriftEnd;
        Controller.OnPlayerDriftStart += HandlePlayerDriftStart;
    }
    private void OnDisable() {
        Controller.OnPlayerDriftEnd -= HandlePlayerDriftEnd;
        Controller.OnPlayerDriftStart -= HandlePlayerDriftStart;
    }
    private void HandlePlayerDriftEnd()
    {
            isDrifting = false;
    }
    private void HandlePlayerDriftStart()
    {
            isDrifting = true;
    }
}
