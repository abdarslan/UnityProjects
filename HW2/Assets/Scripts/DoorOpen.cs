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
            GameObject.Find("GameManager").GetComponent<GameManager>().win();
        }
    }

    IEnumerator openDoor() 
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0, 2f); 
        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }
}
