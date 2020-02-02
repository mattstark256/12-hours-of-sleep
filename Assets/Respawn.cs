using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    private float respawnHeight = 1;

    private Vector3 respawnPoint;
    Rigidbody2D playerRB;


    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position + Vector3.up * respawnHeight;
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Checkpoint checkpoint = collision.GetComponent<Checkpoint>();
        if (checkpoint != null)
        {
            if(!checkpoint.IsLit())
            {
                AudioManager.Instance.Play("checkpoint");
                checkpoint.SetLit(true);
                respawnPoint = checkpoint.transform.position + Vector3.up * respawnHeight;
            }
        }
    }

    public void RespawnAtCheckpoint()
    {
        AudioManager.Instance.Play("hit");
        gameObject.SetActive(false);
        transform.position = respawnPoint;
        playerRB.velocity = Vector3.zero;
        gameObject.SetActive(true);
    }
}
