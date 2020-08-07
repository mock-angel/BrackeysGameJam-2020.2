using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance {get; private set;}
    
    //[SerializeField] private int maxNumberOfSounds;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSourceSounds;
    [SerializeField] private AudioSource audioSourceMusic;

    
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

    [System.Serializable]
    public class AudioInfo{
        public string audioName;
        //public AudioEnum audioEnum;
        public AudioClip audioClip;
        [Range(0, 100)] public float audioVolume;
    }

    [Header("Music Info")]
    public List<AudioInfo> musicAudioInfos;

    public string mainMenuMusicName = "MainMenu Music";

    [Header("Sounds Info")]
    public List<AudioInfo> soundAudioInfos;

    void Awake()
    {
        Instance = this;
        //PlayTitleMusic();
    }

    private void Start()
    {  
        Instance = this;
    }

    #region playSoundOrMusicMethods

    public void PlaySound(string audioName)
    {
        //numbOfPlayingSounds++;
        AudioInfo searchedAudioInfo = GetAudioInfoSound(audioName);

        audioSourceSounds.PlayOneShot(searchedAudioInfo.audioClip, searchedAudioInfo.audioVolume);
    }

    public void PlayMusic(string audioName)
    {
        //numbOfPlayingSounds++;
        AudioInfo searchedAudioInfo = GetAudioInfoMusic(audioName);

        audioSourceSounds.PlayOneShot(searchedAudioInfo.audioClip, searchedAudioInfo.audioVolume);
    }

    #endregion playSoundOrMusicMethods

    

    

    #region customDefinedMusicCalls
    public void PlayMainMusic()
    {

        audioSourceMusic.clip = GetAudioInfoMusic(mainMenuMusicName).audioClip;
        audioSourceMusic.loop = true;
        audioSourceMusic.Play();

        //audioSourceNatureMusic.clip = soundInfo.natureMusicClip;
        //audioSourceNatureMusic.loop = true;
        //audioSourceNatureMusic.Play();

    }

    #endregion customDefinedMusicCalls




    #region InternalSearchMethods

    private AudioInfo GetAudioInfoSound(string audioName){
        AudioInfo searchedAudioInfo = null;

        for(int i = 0; i < soundAudioInfos.Count; i++) {
            if(soundAudioInfos[i].audioName == audioName){
                searchedAudioInfo = soundAudioInfos[i];

                break;
            }
        }

        return searchedAudioInfo;
    }

    private AudioInfo GetAudioInfoMusic(string audioName){
        AudioInfo searchedAudioInfo = null;

        for(int i = 0; i < musicAudioInfos.Count; i++) {
            if(musicAudioInfos[i].audioName == audioName){
                searchedAudioInfo = musicAudioInfos[i];

                break;
            }
        }

        return searchedAudioInfo;
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