using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCatch : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        //calculate distance only towards the direction of the guard's forward vector with some width and height
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) 
        {
            gameManager.GetComponent<GameManager>().gameOver();
        }
    }
}
