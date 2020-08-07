using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomSound
{
    CYBORG,
    TEENAGER,
    MONKEY
}



public class CustomMusicTrigger : MonoBehaviour
{
    public CustomSound SoundEnum;

    private string convertEnumToString(){
        string resultString = "";
        switch(SoundEnum){
            case CustomSound.CYBORG:
                resultString = "Cyborg";
                break;

            case CustomSound.TEENAGER:
                resultString = "Teenager";
                break;

            case CustomSound.MONKEY:
                resultString = "Monkey";
                break;
        }

        return resultString;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlayCustomFadeTrack(convertEnumToString());
    }
}
