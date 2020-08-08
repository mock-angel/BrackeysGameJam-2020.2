using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScript : MonoBehaviour
{
    public void OnPlayClickButtonAudio(){
        AudioManager.Instance.Play("Select");
    }
}
