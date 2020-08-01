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

        //Change the Health UI Slider
        ChangeHealthSlider();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetHealth()
    {
        currentHealthAmount = maxHealthAmount;
    }

    public void ChangeHealth(int changeHealthAmount)
    {
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

        ChangeHealthSlider();
    }

    float CalculateHealth()
    {
        return currentHealthAmount / maxHealthAmount;
    }

    public void ChangeHealthSlider()
    {
        healthSlider.value = CalculateHealth();

        healthText.text = currentHealthAmount + " / " + maxHealthAmount;
    }
}