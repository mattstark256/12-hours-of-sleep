using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("YEET");
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
