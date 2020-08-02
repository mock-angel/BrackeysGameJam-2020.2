using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rewind : MonoBehaviour
{

    #region VARIABLES

    PlatformerMovement platformermovementscript;

    //UI
    public bool isPlayer; //FOR UI
    public Slider rewindSlider;
    public TMP_Text rewindText;

    //Rewind
    public float maxRewindAmount;
    public float currentRewindAmount;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region ASSIGN COMPONENTS

        platformermovementscript = gameObject.GetComponent<PlatformerMovement>();

        #endregion

        //Reset Rewind (refill Rewind)
        ResetRewind();

        //Changes the Rewind UI
        ChangeRewindUI();
    }

    // Update is called once per frame
    void Update()
    {

        //If Player is moving
        if(platformermovementscript.moveInput != 0  && currentRewindAmount <= maxRewindAmount)
        {
            currentRewindAmount += Mathf.Abs(platformermovementscript.moveInput) / 2;
        }

        //If Player is standing still
        else if (platformermovementscript.moveInput == 0 && currentRewindAmount >= 0)
        {
            currentRewindAmount -= 1;
        }

        //Changes the Rewind UI
        ChangeRewindUI();
    }

    public void ResetRewind()
    {
        //Reset Rewind
        #region RESET REWIND

        currentRewindAmount = maxRewindAmount;

        #endregion
    }

    public void ChangeRewind(int ChangeRewindAmount)
    {
        #region CHANGE REWIND

        if (ChangeRewindAmount > 0)
        {
            if (ChangeRewindAmount + currentRewindAmount <= maxRewindAmount)
            {
                currentRewindAmount += ChangeRewindAmount;
            }
            else
            {
                currentRewindAmount = maxRewindAmount;
            }
        }
        else if (ChangeRewindAmount < 0)
        {
            if (ChangeRewindAmount - currentRewindAmount >= 0)
            {
                currentRewindAmount -= ChangeRewindAmount;
            }
            else
            {
                currentRewindAmount = 0;
            }
        }

        #endregion

        //Changes the Rewind UI
        ChangeRewindUI();
    }

    float CalculateRewind()
    {
        //Calculate Rewind
        return currentRewindAmount / maxRewindAmount;
    }

    public void ChangeRewindUI()
    {
        //Change Rewind UI
        #region CHANGE REWIND SLIDER

        if (isPlayer)
        {
            //Change Rewind Slider Value
            rewindSlider.value = CalculateRewind();

            //Change Rewind Text
            rewindText.text = currentRewindAmount + " / " + maxRewindAmount;
        }

        #endregion
    }
}