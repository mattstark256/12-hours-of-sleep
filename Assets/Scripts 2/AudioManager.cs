using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField]
    Sound[] sounds;



    public void Play(string name)
    {
        foreach(Sound s in sounds)
        {
            if(s.name == name)
            {
                s.source.Play();
            }
        }
        Debug.LogError(name + " sound effect does not exist");
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            instance.Play("test");
        }
    }
}
