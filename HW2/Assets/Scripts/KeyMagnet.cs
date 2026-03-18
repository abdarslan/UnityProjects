using UnityEngine;

public class KeyMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetRange = 5f;
    public float followSpeed = 10f;
    
    // Where should the key hover? (X, Y height, Z forward/back)
    // Default is 1.5 units above and 1 unit behind the player
    public Vector3 carryOffset = new Vector3(0f, 1.5f, -1f); 
    
    private Transform player;
    private Rigidbody rb;
    private Collider keyCollider;
    private bool isCollected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        keyCollider = GetComponent<Collider>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update() 
    {
        if (player == null) return;

        // STATE 1: Waiting to be picked up
        if (!isCollected)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < magnetRange)
            {
                isCollected = true;
                
                // Turn off physics so it doesn't push or crash the game
                if (rb != null) rb.isKinematic = true;
                if (keyCollider != null) keyCollider.isTrigger = true;
            }
        }
        // STATE 2: Carried by the player
        else
        {
            // 1. Calculate the exact hover spot, turning WITH the player
            Vector3 targetPosition = player.position + (player.rotation * carryOffset);
            
            // 2. Smoothly glide the key to that hover spot
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            
            // 3. (Optional) Make the key face the same direction as the player
            transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, followSpeed * Time.deltaTime);
        }
    }
}