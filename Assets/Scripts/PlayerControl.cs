using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody player;
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
     void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        player.AddForce(movement * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
