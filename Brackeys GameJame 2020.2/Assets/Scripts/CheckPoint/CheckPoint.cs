using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    #region VARIABLES

    public GameObject checkPointGameObject;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToCheckPoint(GameObject player)
    {
        player.transform.position = checkPointGameObject.transform.position;

        PlatformerMovement.Instance.CheckpointParticles.Play();
    }
}
