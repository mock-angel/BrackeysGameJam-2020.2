﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rewind : MonoBehaviour
{

    #region VARIABLES

    PlatformerMovement platformermovementscript;

    //Rewind
    [Header("REWIND MECHANIC")]
    [Range(0, 1)]
    public float rewindCurveCap = 0.2f;
    public float maxRewindAmount;
    public float currentRewindAmount;
    public float currentRewindPercentage;

    [Range(0, 1)]
    public float startRewindAmountPercentage;

    //UI
    [Header("UI")]
    public bool isPlayer; //FOR UI
    public Slider rewindSlider;
    public TMP_Text rewindText;
    public TMP_Text characterText;


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
        //The Rewind Mechanic (Curve and Math)
        RewindMechanic();

        //Changes the Stages
        ChangePlayerStage();
    }

    private void RewindMechanic()
    {
        //The Rewind Mechanic (Curve and Math)
        #region REWIND MECHANIC

        //If Player is moving
        if (platformermovementscript.moveInput != 0 && currentRewindAmount <= maxRewindAmount)
        {
            //Does not go below certain float
            if (Mathf.Abs(CalculateRewind() - 1) < rewindCurveCap / 1.25f)
            {
                ChangeRewind((Mathf.Abs(rewindCurveCap) / 1.25f));
            }
            else
            {
                ChangeRewind((Mathf.Abs((CalculateRewind() - 1) / 1.5f) * Mathf.Abs(platformermovementscript.moveInput)));
            }
        }

        //If Player is standing still
        else if (platformermovementscript.moveInput == 0 && currentRewindAmount >= 0)
        {
            //Does not go below certain float
            if (CalculateRewind() < rewindCurveCap)
            {
                ChangeRewind(-rewindCurveCap);
            }
            else
            {
                ChangeRewind(-CalculateRewind());
            }
        }

        #endregion
    }

    public void ChangePlayerStage()
    {
        #region CHANGE PLAYER STAGE

        //Stage 3
        if (currentRewindPercentage > 0.7f && currentRewindPercentage < 1.0f)
        {
            platformermovementscript.characterStage = 3;
        }
        //Stage 2
        else if (currentRewindPercentage > 0.4 && currentRewindPercentage < 0.7f)
        {
            platformermovementscript.characterStage = 2;
        }
        //Stage 1
        else if (currentRewindPercentage > 0.1 && currentRewindPercentage < 0.4f)
        {
            platformermovementscript.characterStage = 1;
        }
        //Stage 0
        else if (currentRewindPercentage > 0 && currentRewindPercentage < 0.1f)
        {
            platformermovementscript.characterStage = 0;
        }

        #endregion
    }

    public void ResetRewind()
    {
        //Reset Rewind
        #region RESET REWIND

        currentRewindAmount = maxRewindAmount * startRewindAmountPercentage;

        #endregion
    }

    public void ChangeRewind(float ChangeRewindAmount)
    {
        #region CHANGE REWIND

        if (ChangeRewindAmount > 0)
        {
            if (ChangeRewindAmount + currentRewindAmount < maxRewindAmount)
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
            if (ChangeRewindAmount + currentRewindAmount > 0)
            {
                currentRewindAmount += ChangeRewindAmount;
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
        currentRewindPercentage = currentRewindAmount / maxRewindAmount;
        return currentRewindPercentage;
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
            rewindText.text = currentRewindAmount.ToString("0.0") + " / " + maxRewindAmount;

            //Change Character Text
            if(platformermovementscript.characterStage == 0)
            {
                characterText.text = "You rewinded too much (Looses Life)";
            }
            else if(platformermovementscript.characterStage == 1)
            {
                characterText.text = "Caveman";
            }
            else if (platformermovementscript.characterStage == 2)
            {
                characterText.text = "Teenager";
            }
            else if (platformermovementscript.characterStage == 3)
            {
                characterText.text = "Cyborg";
            }
        }

        #endregion
    }
}