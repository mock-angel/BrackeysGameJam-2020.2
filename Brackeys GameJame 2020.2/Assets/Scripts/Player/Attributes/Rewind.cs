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

    //Rewind
    [Header("REWIND MECHANIC")]
    [Range(0, 1)]
    public float rewindCurveCap = 0.2f;
    public float maxRewindAmount;
    public float currentRewindAmount;
    public float currentRewindPercentage;

    //UI
    [Header("UI")]
    public bool isPlayer; //FOR UI
    public Slider rewindSlider;
    public TMP_Text rewindText;


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

        //Changes the Rewind UI
        ChangeRewindUI();
    }

    private void RewindMechanic()
    {
        //The Rewind Mechanic (Curve and Math)
        #region REWIND MECHANIC

        //If Player is moving
        if (platformermovementscript.moveInput != 0 && currentRewindAmount <= maxRewindAmount)
        {
            //Does not go below certain float
            if (Mathf.Abs(CalculateRewind() - 1) < rewindCurveCap)
            {
                currentRewindAmount += Mathf.Abs(rewindCurveCap);
            }
            else
            {
                currentRewindAmount += Mathf.Abs(CalculateRewind() - 1);
            }
        }

        //If Player is standing still
        else if (platformermovementscript.moveInput == 0 && currentRewindAmount >= 0)
        {
            //Does not go below certain float
            if (CalculateRewind() < rewindCurveCap)
            {
                currentRewindAmount -= rewindCurveCap;
            }
            else
            {
                currentRewindAmount -= CalculateRewind();
            }
        }

        #endregion
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
            rewindText.text = currentRewindAmount + " / " + maxRewindAmount;
        }

        #endregion
    }
}