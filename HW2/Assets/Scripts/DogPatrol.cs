using System.Collections;
using UnityEngine;

public class DogPatrol : MonoBehaviour
{
    public float patrolDistance = 10f; 
    public float patrolSpeed = 2f; 

    private Animator animator;
    private Vector3 startPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
        
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        Vector3 pointA = startPos;
        Vector3 pointB = startPos + (transform.forward * patrolDistance);

        if (animator != null) //Handled and suggested by Gemini 3.1
        {
            animator.SetFloat("Vert", 1f);  
            animator.SetFloat("State", 0f); 
        }//

        while (true) 
        {
            yield return StartCoroutine(MoveToTarget(pointB));
            transform.Rotate(0, 180, 0);
            yield return StartCoroutine(MoveToTarget(pointA));
            transform.Rotate(0, 180, 0);
        }
    }

    IEnumerator MoveToTarget(Vector3 target)
    {
        // Keep moving until we are extremely close to the target
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            // Move smoothly towards the target based on the set speed
            transform.position = Vector3.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
