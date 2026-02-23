using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScript : MonoBehaviour
{
    private Rigidbody rb;    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time > 6 && Time.time < 8)
        {
            rb.AddForce(0, 0, 13);
        }else
        {

            rb.AddForce(0, 10, 0);
        }
    }
}
