using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    #region VARIABLES

    //Components & Scripts
    private PlatformerMovement platformermovementscript;


    //Variables
    public bool isRewinding = false;
    public List<Vector3> positions;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region COMPONENTS & SCRIPTS

        platformermovementscript = gameObject.GetComponent<PlatformerMovement>();

        #endregion

        positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //Start to Rewind
            StartRewind();
        }
        else if(Input.GetKeyUp(KeyCode.Return))
        {
            //Stop to Rewind
            StopRewind();
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            //Rewind
            Rewind();

            //Player Movement Script is not Working when Rewinding
            platformermovementscript.isRewinding = true;
            platformermovementscript.moveInput = 0;
        }
        else
        {
            //Record
            Record();

            //Player Movement Script works when not Rewinding
            platformermovementscript.isRewinding = false;
        }
    }

    void Rewind()
    {
        if(positions.Count > 0)
        {
            //Rewind Position
            transform.position = positions[0];

            //Remove from List
            positions.RemoveAt(0);
        }
    }

    void Record()
    {
        //If list is empty
        if(positions.Count <= 0)
        {
            //Add current Position to List
            positions.Insert(0, transform.position);
        }

        else if (gameObject.transform.position != positions[0])
        {
            //Add current Position to List
            positions.Insert(0, transform.position);
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
