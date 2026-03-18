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
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead) { 
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                player.GetComponent<CharacterController>().enabled = true; // enable controls on restart
            }
        }
        
        //any movement clears the text if the game hasn't ended
        if (!isDead && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)) {
            statusText.text = "";
        }
        
        // Don't fall to death if already dead/won
        if (!isDead && player.position.y < startingHeight - fallThreshold) {
            gameOver();
        }

    }
    public void gameOver() {
        isDead = true;
        statusText.text = "You died! Press R to restart.";
        statusText.color = Color.red; 
        player.GetComponent<CharacterController>().enabled = false; //disable controls on death
    }
    public void win() {
        isDead = true; // 
        statusText.text = "You win! Press R to restart.";
        statusText.color = Color.green;
        player.GetComponent<CharacterController>().enabled = false; //disable controls on win
    }
}
