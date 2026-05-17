using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour //singleton pattern for game manager, we will use this to manage game states and other global variables in the future
{   

    private static GameManager instance;

    // get game over event from plane and manage game over state here
    // Start is called before the first frame update
    public GameObject car;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if there is press on R key, restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            // reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnEnable()
    {
        GameOverTrigger.OnPlayerOutOfBounds += HandleGameOver;
    }

    private void OnDisable()
    {
        GameOverTrigger.OnPlayerOutOfBounds -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        car.SetActive(false);
    }
}
