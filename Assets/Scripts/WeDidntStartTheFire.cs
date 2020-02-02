using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeDidntStartTheFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Activate()
    {
        Debug.Log("True");
        gameObject.SetActive(true);
    }
}
