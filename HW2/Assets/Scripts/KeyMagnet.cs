using UnityEngine;

public class KeyMagnet : MonoBehaviour
{
   
    public float pushRadius = 0.8f;      
    public float pushForceMultiplier = 1.3f;
    
   
    public Vector3 carryOffset = new Vector3(0f, 1f, 1f); 
    public float carrySpeed = 10f;       
    private Rigidbody rb;
    private Transform player;
    private Vector3 lastPlayerPos;
    private bool isCarrying = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.drag = 2f; 
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            player = playerObj.transform;
            lastPlayerPos = player.position;
        }
    }

    void Update()
    {
        if (player == null || rb == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isCarrying) {
                isCarrying = true;
                rb.isKinematic = true;

            } else {
                isCarrying = false;
                rb.isKinematic = false;
            }
        }

        if (isCarrying) {
            Vector3 targetPosition = player.position + (player.rotation * carryOffset);
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, carrySpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, carrySpeed * Time.deltaTime);
            lastPlayerPos = player.position; 
            return;
        }
        Vector3 playerVelocity = (player.position - lastPlayerPos) / Time.deltaTime;
        lastPlayerPos = player.position;

        float distance = Vector3.Distance(transform.position, player.position);
        
        // 
        if (distance < pushRadius)
        {
            Vector3 pushDir = transform.position - player.position;
            pushDir.y = 0;
            pushDir.Normalize();

            float pushSpeed = Vector3.Dot(playerVelocity, pushDir);
            
            // If the player is actively moving towards the key
            if (pushSpeed > 0)
            {
                Vector3 targetVelocity = pushDir * (pushSpeed * pushForceMultiplier);
                rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
            }
        }
    }
}