using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private bool isOpening = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Key" && !isOpening) 
        {
            isOpening = true; // Prevent multiple coroutines running at once
            StartCoroutine(openDoor());
        }
    }

    IEnumerator openDoor() 
    {
        Vector3 startPos = transform.position;
        // Move the door straight up by 3 units
        Vector3 endPos = startPos + new Vector3(0, 3f, 0); 
        float duration = 1.5f; // Takes 1.5 seconds to fully open
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Smoothly move the door upwards frame-by-frame
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        
        // Ensure it ends perfectly at the final position
        transform.position = endPos;
    }
}
