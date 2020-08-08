using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

using MyAttributes;

[System.Serializable]
public class Sound{
    public string name;
    
    public AudioClip clip;

    [Range(0, 1)] 
    public float volume = 1;

    [Range(0.1f, 3f)] 
    public float pitch = 1;

    public bool loop;

    public bool activateFade = false;

    [ConditionalField("activateFade")]
    [Range(0, 10)] public float fadeInDuration = 3;
    [ConditionalField("activateFade")]
    [Range(0, 10)] public float fadeOutDuration = 3;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance {get; private set;}
    
    //[SerializeField] private int maxNumberOfSounds;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSourceSounds;
    [SerializeField] private AudioSource audioSourceMusic;

    public bool PlayTitleMusic = false;
    public bool PlayBackGroundMusic = false;

    public bool PlaySequenceMusic = false;
    //[SerializeField] private SoundInfo soundInfo;



    //private static bool isSoundsMuted = false;

    //public bool IsSoundsMuted
    //{
    //    get => isSoundsMuted;
    //    set => isSoundsMuted = value;
    //}

    //private static bool isMusicMuted;

    //private bool isMainMusicPlaying;
    //private bool isStartMusicPlaying;
    //private int numbOfPlayingSounds;

    /*
    [System.Serializable]
    public class AudioEnum{
        NONE,
        JUMPING,
        PLAYER_GOT_SHOT,
        GAME_ENDED
    }*/

    

    [Header("Music Info")]
    //public Sound[] musicAudioInfos;

    public string mainMenuMusicName = "MainMenu Music";

    [Header("Sounds Info")]
    public Sound[] sounds;

    

    void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = s.loop;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        if(PlayTitleMusic) Play(mainMenuMusicName);
        
        if(PlayBackGroundMusic) Play("BackgroundMusic");

        if(PlaySequenceMusic) {
            Play(cyborgSoundName);
            Play(teenagerSoundName);
            Play(monkeySoundName);
        }

        //PlayTitleMusic();
    }

    private void Start()
    {  
        Instance = this;
    }

    #region playSoundOrMusicMethods

    public void Play(string name)
    {
        //numbOfPlayingSounds++;
        Sound s = GetSound(name);

        if(s == null)
            Debug.LogWarning(string.Format("Sound: {0} not found.", name));
        
        else s.source.Play();
        
        //audioSourceSounds.PlayOneShot(s.clip, s.volume);
    }

    private Sound[] currentActiveCustomSounds;

    public void PlayCustomFadeTrack(string name){
        Sound s = GetSound(name);

        if(s == null)
            Debug.LogWarning(string.Format("Sound: {0} not found.", name));
        
        else {
            StopCoroutine("StartFadeLogic");

            StartCoroutine(StartFadeLogic(s));
        }
    }

    IEnumerator StartFadeLogic(Sound fadeInSound){

        //currentActiveCustomSound = fadeInSound;
        yield return FadeOutAllSounds();;
        
        if(fadeInSound != null)
            yield return StartFade(fadeInSound.source, fadeInSound.fadeInDuration, 1);

        yield break;
    }

    [Header("Stored Default Strings")]
    public string cyborgSoundName = "Cyborg";
    public string teenagerSoundName = "Teenager";
    public string monkeySoundName = "Monkey";

    IEnumerator FadeOutAllSounds(){

        Sound cyborg = GetSound(cyborgSoundName);
        Sound teenager = GetSound(teenagerSoundName);
        Sound monkey = GetSound(monkeySoundName);

        float cyborgDuration = cyborg.volume * cyborg.fadeOutDuration;
        float teenagerDuration = teenager.volume * teenager.fadeOutDuration;
        float monkeyDuration = monkey.volume * monkey.fadeOutDuration;

        IEnumerator fadeoutSoundsIEnumerator1 = StartFade(cyborg.source, cyborgDuration, 0);
        IEnumerator fadeoutSoundsIEnumerator2 = StartFade(teenager.source, teenagerDuration, 0);
        IEnumerator fadeoutSoundsIEnumerator3 = StartFade(monkey.source, monkeyDuration, 0);

        while(true){
            bool a = fadeoutSoundsIEnumerator1.MoveNext();
            bool b = fadeoutSoundsIEnumerator2.MoveNext();
            bool c = fadeoutSoundsIEnumerator3.MoveNext();
            
            if(a || b || c) yield return null;

            else yield break;
        }
    }

    IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        if(audioSource == null) yield break;

        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    #endregion playSoundOrMusicMethods

    

    

    #region customDefinedMusicCalls
    /*
    public void PlayMainMusic()
    {

        audioSourceMusic.clip = GetSound(mainMenuMusicName).clip;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();

        //audioSourceNatureMusic.clip = soundInfo.natureMusicClip;
        //audioSourceNatureMusic.loop = true;
        //audioSourceNatureMusic.Play();

    }
    */
    #endregion customDefinedMusicCalls




    #region InternalSearchMethods

    private Sound GetSound(string name){
        
        return Array.Find(sounds, sound => sound.name == name);
    }


    #endregion InternalSearchMethods
}