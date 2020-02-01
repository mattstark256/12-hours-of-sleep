using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    [SerializeField]
    private float deathDelay = 0.2f;

    private Respawn respawn;

    private bool dying = false;
    private float timeSinceDeath = 0;

    private void Awake()
    {
        respawn = GetComponent<Respawn>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("entered collider");
        if (collision.tag == "Death")
        {
            Debug.Log("dying");
            timeSinceDeath = 0;
            dying = true;
        }
    }

    private void Update()
    {
        if (dying)
        {
            timeSinceDeath += Time.deltaTime;

            if (timeSinceDeath> deathDelay)
            {
                dying = false;
                respawn.RespawnAtCheckpoint();
            }
        }
    }
}
