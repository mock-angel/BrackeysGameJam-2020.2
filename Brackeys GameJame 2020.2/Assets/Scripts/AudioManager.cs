using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public class Sound{
    public string name;
    
    public AudioClip clip;

    [Range(0, 1)] 
    public float volume = 1;

    [Range(0.1f, 3f)] 
    public float pitch = 1;

    public bool loop;

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

    public bool PlayTitleMusic;
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
        
        for(int i = 0; i< sounds.Length; i++){
            if(sounds[i].name == name) print("found key");
            print(sounds[i].name);
        }

        if(s == null)
            Debug.LogWarning(string.Format("Sound: {0} not found.", name));
        
        else s.source.Play();
        
        //audioSourceSounds.PlayOneShot(s.clip, s.volume);
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
    /*
    public void PlayTitleMusic()
    {
        //isMusicMuted = false;
        //IsSoundsMuted = false;
        audioSourceMusic.clip = soundInfo.titleMusicClip;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();
    }

    public void PlayMainMusic()
    {
        //isMusicMuted = false;
        //IsSoundsMuted = false;
        audioSourceMusic.clip = soundInfo.mainMusicClip;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();

        //isMusicMuted = false;
        //IsSoundsMuted = false;
        audioSourceNatureMusic.clip = soundInfo.natureMusicClip;
        audioSourceNatureMusic.loop = true;
        audioSourceNatureMusic.Play();

    }

    public void MuteMainMusic()
    {
        isMusicMuted = true;
        //IsSoundsMuted = true;
        audioSourceMusic.Pause();

        isMusicMuted = true;
        //IsSoundsMuted = true;
        audioSourceNatureMusic.Pause();


    }

    /*
    public void UpdateMusicState()
    {        
        if (isMusicMuted) PlayMainMusic();

        else MuteMainMusic();
    }*/

    /*
    public void PlaySheepSoundAudio()
    {
        if (numbOfPlayingSounds < maxNumberOfSounds)
        {
            numbOfPlayingSounds++;
            int rand = Random.Range(0, soundInfo.sheepSoundClips.Length);
            if (rand == 1)
            {
                audioSourceSounds.PlayOneShot(soundInfo.sheepSoundClips[rand], soundInfo.sheepSoundVolume);
            }
            else
            {
                audioSourceSounds.PlayOneShot(soundInfo.sheepSoundClips[rand], soundInfo.sheepSoundVolume);
            }
        }
    }

    public void PlaySheepDiedAudio()
    {
        if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.sheepDiedClip, soundInfo.sheepDiedVolume);
        }
    }

    public void PlayWolfSoundAudio()
    {
        if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.wolfSoundClip, soundInfo.wolfSoundVolume);
        }
    }

    public void PlayWolfDiedAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.wolfDiedClip, soundInfo.wolfDiedVolume);
        }
    }

    public void PlayWolfHurtAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.wolfHurtClip, soundInfo.wolfHurtVolume);
        }
    }

    public void PlayWolfAttackAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.wolfAttackClip, soundInfo.wolfAttackVolume);
        }
    }

    public void PlayThrowWeaponAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.throwWeaponClip, soundInfo.throwWeaponVolume);
        }
    }

    public void PlayHitWeaponAudio()
    {
        if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.hitWeaponClip, soundInfo.hitWeaponVolume);
        }
    }

    public void PlayBuildTowerAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.buildTowerClip, soundInfo.buildTowerVolume);
        }
    }

    public void PlayDestroyTowerAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.destroyTowerClip, soundInfo.destroyTowerVolume);
        }
    }

    public void PlayUpgradeTowerAudio()
    {
        if (!IsSoundsMuted)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.upgradeTowerClip, soundInfo.upgradeTowerVolume);
        }
    }

    public void PlayHunterDiedAudio()
    {
        if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
        {
            numbOfPlayingSounds++;
            audioSourceSounds.PlayOneShot(soundInfo.hunterDiedClip, soundInfo.hunterDiedVolume);
        }
    }

    public IEnumerator PlayIdelSheepSound()
    {
        while (true)
        {
            PlaySheepSoundAudio();
            yield return new WaitForSeconds(10);
        }
    }

    public IEnumerator PlayIdelWolfSound()
    {
        while (true)
        {
            PlayWolfSoundAudio();
            yield return new WaitForSeconds(13);
        }
    }

    //public void PlayBladHtAudio()
    //{
    //    if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
    //    {
    //        numbOfPlayingSounds++;
    //        int rand = Random.Range(0, soundInfo.bladHitClips.Length);
    //        if (rand == 1)
    //        {
    //            audioSourceSounds.PlayOneShot(soundInfo.bladHitClips[rand], .1f);
    //        }
    //        else
    //        {
    //            audioSourceSounds.PlayOneShot(soundInfo.bladHitClips[rand], soundInfo.bladHitVolume);
    //        }
    //    }
    //}




    //public void PlayShieldHitAudio()
    //{
    //    if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
    //    {
    //        numbOfPlayingSounds++;
    //        audioSourceSounds.PlayOneShot(soundInfo.shieldHitClip, soundInfo.shieldHitVolume);
    //    }
    //}

    //public void PlaySpearAudio()
    //{
    //    if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
    //    {
    //        numbOfPlayingSounds++;
    //        audioSourceSounds.PlayOneShot(soundInfo.spearClip, soundInfo.spearVolume);
    //    }
    //}

    //public void PlaySpellAudio()
    //{
    //    if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
    //    {
    //        numbOfPlayingSounds++;
    //        audioSourceSounds.PlayOneShot(soundInfo.spellClip, soundInfo.spellVolume);
    //    }
    //}

    //public void PlayChianAudio()
    //{
    //    if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
    //    {
    //        numbOfPlayingSounds++;
    //        audioSourceSounds.PlayOneShot(soundInfo.chainClip, soundInfo.chainVolume);
    //    }
    //}

    //public void PlayWorkerAxeAudio()
    //{
    //    if (!IsSoundsMuted && numbOfPlayingSounds < maxNumberOfSounds)
    //    {
    //        numbOfPlayingSounds++;
    //        audioSourceSounds.PlayOneShot(soundInfo.workerAxeClips[Random.Range(0, soundInfo.bladHitClips.Length)], soundInfo.workerAxeVolume);
    //    }
    //}
}
*/