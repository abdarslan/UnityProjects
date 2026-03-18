using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    private Rigidbody rb;
    public float timeToFall = 1f;
    public float timeToReset = 2f;

    // Variables to save the perfect starting state
    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Save exactly where the floor is before it drops
        originalPos = transform.position;
        originalRot = transform.rotation;
        
        StartCoroutine(fallTracker());
    }

    IEnumerator fallTracker() 
    {
        while (true) // Loop forever so it acts like a repeating trap
        {
            yield return new WaitForSeconds(timeToFall);
            
            rb.isKinematic = false;
            
            yield return new WaitForSeconds(timeToReset);
            
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;        // Stop leftover downward momentum
            
            transform.position = originalPos;
            transform.rotation = originalRot;
        }
    }
}
