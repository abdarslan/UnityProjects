using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public TextMeshProUGUI statusText;

    public float fallThreshold = 0.3f;
    private float startingHeight; 

    private bool isDead = false;

    void Start()
    {
        startingHeight = player.position.y;

        statusText.text = "Find the key, Gather with E, find the door, and escape!";
        statusText.color = Color.green;
        guards[0] = GameObject.Find("DogGuard-1").transform;
        guards[1] = GameObject.Find("DogGuard-2").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead) {
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        //any movement clears the text
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            statusText.text = "";
        }
        if (player.position.y < startingHeight - fallThreshold) {
            isDead = true;
            statusText.text = "You died! Press R to restart.";
            statusText.color = Color.red; // Red
        }

    }
}
