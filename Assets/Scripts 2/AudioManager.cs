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
    //Sound symbol;


    public void Play(string name)
    {
        foreach(Sound s in soundEffects)
        {
            if(s.name == name)
            {
                if (!s.source.isPlaying)
                {
                    s.source.Play();
                }
                return;
            }
        }
        foreach(Sound m in music)
        {
            if(m.name == name)
            {
                if (!m.source.isPlaying)
                {
                   m.source.Play();
                }
                return;
            }
        }
        Debug.LogError(name + " sound effect does not exist");
    }
    public void Stop(string name)
    {
        foreach (Sound s in soundEffects)
        {
            if (s.name == name)
            {
                s.source.Stop();
                return;
            }
        }
        foreach (Sound m in music)
        {
            if (m.name == name)
            {
                m.source.Stop();
                return;
            }
        }
       // Debug.LogError(name + " sound effect does not exist");
    }

    public bool IsPlaying(string name)
    {
        foreach (Sound s in soundEffects)
        {
            if (s.name == name)
            {
                return s.source.isPlaying;
                
            }
        }
        foreach (Sound m in music)
        {
            if (m.name == name)
            {
                return m.source.isPlaying;
            }
        }
       // Debug.LogError(name + " sound effect does not exist");
        return false;
    }

    public void SetLoopingAndPlay(string name)
    {
        foreach (Sound s in soundEffects)
        {
            if (s.name == name)
            {
                s.loop = true;
                if (!s.source.isPlaying)
                {
                    s.source.Play();
                }
                return;
            }
        }
       // Debug.LogError(name + " sound effect does not exist");
    }

    public void StopLooping(string name)
    {
        foreach (Sound s in soundEffects)
        {
            if (s.name == name)
            {
                s.loop = false;
               
                return;
            }
        }
       // Debug.LogError(name + " sound effect does not exist");
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
        if(music.Length > 0)
        {
            //Play(music[0].name);
        }
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
