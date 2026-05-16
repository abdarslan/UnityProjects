using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyreMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    void Start()
    {
        // we need the rigidbody of the car to calculate the speed of the car, which will affect how fast the tyres spin
        rb = GetComponentInParent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float steer = Input.GetAxis("Horizontal");
        transform.localRotation = Quaternion.Euler(0, steer * 30, 0);
    }
    private void FixedUpdate() {
        // according to speed tyres should spin faster or slower
        // we need rigidbody velocity to calculate speed, do we need the speed of the car or tyre

        float speed = rb.velocity.magnitude;
        transform.localRotation *= Quaternion.Euler(speed * 360 * Time.fixedDeltaTime, 0, 0);
    }
}
