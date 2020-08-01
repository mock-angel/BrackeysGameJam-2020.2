using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    #region VARIABLES

    //UI
    public bool isPlayer; //FOR UI
    public Slider healthSlider;
    public TMP_Text healthText;

    //Health
    public int maxHealthAmount;
    public int currentHealthAmount;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Reset Health (refill Health)
        ResetHealth();

        //Changes the Health UI
        ChangeHealthUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetHealth()
    {
        //Reset Health
        #region RESET HEALTH

        currentHealthAmount = maxHealthAmount;

        #endregion
    }

    public void ChangeHealth(int changeHealthAmount)
    {
        #region CHANGE HEALTH

        if (changeHealthAmount > 0)
        {
            if (changeHealthAmount + currentHealthAmount <= maxHealthAmount)
            {
                currentHealthAmount += changeHealthAmount;
            }
            else
            {
                currentHealthAmount = maxHealthAmount;
            }
        }
        else if (changeHealthAmount < 0)
        {
            if (changeHealthAmount - currentHealthAmount >= 0)
            {
                currentHealthAmount -= changeHealthAmount;
            }
            else
            {
                currentHealthAmount = 0;
            }
        }

        #endregion

        //Changes the Health UI
        ChangeHealthUI();
    }

    float CalculateHealth()
    {
        //Calculate Health
        return currentHealthAmount / maxHealthAmount;
    }

    public void ChangeHealthUI()
    {
        //Change Health UI
        #region CHANGE HEALTH SLIDER

        if(isPlayer)
        {
            //Change Health Slider Value
            healthSlider.value = CalculateHealth();

            //Change Health Text
            healthText.text = currentHealthAmount + " / " + maxHealthAmount;
        }

        #endregion
    }
}