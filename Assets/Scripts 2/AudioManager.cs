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
    Sound[] soundEffects;
    
    [SerializeField]
    Sound[] music;

    int currentMusicTrack = 0;

    //[SerializeField]
    //Sound cymbal;


    public void Play(string name)
    {
        foreach(Sound s in soundEffects)
        {
            if(s.name == name)
            {
                s.source.Play();
                return;
            }
        }
        foreach(Sound m in music)
        {
            if(m.name == name)
            {
                m.source.Play();
                return;
            }
        }
        Debug.LogError(name + " sound effect does not exist");
    }




    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;

            s.source.loop = s.loop;

        } 
        
        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;

            m.source.loop = m.loop;

        }
       // cymbal.source = gameObject.AddComponent<AudioSource>();
       // cymbal.source.clip = cymbal.clip;
       // cymbal.source.volume = cymbal.volume;
       // cymbal.source.loop = cymbal.loop;
        Play(music[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            instance.PlayNextMusic();
        }
    }
    public void PlayNextMusic()
    {
        if (currentMusicTrack < music.Length-1)
        {
            float currentTimeInTrack = music[currentMusicTrack].source.time;
            music[currentMusicTrack + 1].source.time = currentTimeInTrack;
            music[currentMusicTrack].source.Stop();
            music[currentMusicTrack + 1].source.Play();
            currentMusicTrack++;
        }
    }
}
