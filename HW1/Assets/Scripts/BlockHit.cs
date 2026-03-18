using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private AudioSource hit;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hit = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        //when there is a collision with the block the block will play the hit sound, dont do first 1 second of the game
        if (Time.time > 1)
        {
            hit.Play();
        }

    }
}
