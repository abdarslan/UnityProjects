using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConstruct : MonoBehaviour
{
    //spawn construct prefab with button p
    public GameObject constructPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(constructPrefab, new Vector3(-2.75f, 0.78f, 0.09f), Quaternion.identity);
        }
    }
}
