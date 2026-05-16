using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Required for Actions

public class GameOverTrigger : MonoBehaviour
{
    // Define the event
    public static event Action OnPlayerOutOfBounds;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            
            // Broadcast the event. 
            // The "?" safely checks if any managers are actually listening before firing.
            OnPlayerOutOfBounds?.Invoke();
        }
    }
}