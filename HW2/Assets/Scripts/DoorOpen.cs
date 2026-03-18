using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private bool isOpening = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Key") && !isOpening) 
        {
            isOpening = true; 
            StartCoroutine(openDoor());
        }
    }

    IEnumerator openDoor() 
    {
        Vector3 startPos = transform.position;
        // Move the door straight left by 3 units
        Vector3 endPos = startPos + new Vector3(0, 0, -3f); 
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Smoothly move the door leftwards frame-by-frame
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.position = endPos;
    }
}
