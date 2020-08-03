using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    #region VARIABLES

    //Health
    [Header("HEALTH")]
    public float currentHealthPercentage;
    public int maxHealthAmount;
    public int currentHealthAmount;

    //Life
    [Header("LIFE")]
    public int maxLifeAmount;
    public int currentLifeAmount;

    //Health UI
    [Header("HEALTH UI")]
    public Slider healthSlider;
    public TMP_Text healthText;

    //Life UI
    [Header("LIFE UI")]
    public GameObject lifeIconContainer;
    public GameObject lifeIconPrefab;
    public List<GameObject> lifeHeartsList;
    public TMP_Text lifeText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //List
        lifeHeartsList = new List<GameObject>();

        //Reset Health (refill Health)
        ResetHealthLife();

        //Instantiate Life Icons
        InstantiateLifeIcons();

        //Changes the Health UI
        ChangeHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeHealth(-1);
        }
    }

    public void ResetHealthLife()
    {
        //Reset Health
        #region RESET HEALTH

        currentHealthAmount = maxHealthAmount;
        currentLifeAmount = maxLifeAmount;

        #endregion
    }

    public void InstantiateLifeIcons()
    {
        //Instantiates Heart Icons based on maxLifeHeartAmount and adds them to list
        #region INSTANTIATE & ADD TO LIST

        if (lifeIconPrefab != null && lifeIconContainer != null)
        {
            //Heart are deleted
            for (int i = 0; i < lifeHeartsList.Count; i++)
            {
                Destroy(lifeHeartsList[i].gameObject);
            }

            //Hearts are instantiated
            for (int i = 0; i < maxLifeAmount; i++)
            {
                //instantiate
                GameObject InstantiatedLifeIcon = Instantiate(lifeIconPrefab, transform.position, Quaternion.identity);

                //Set Parent
                InstantiatedLifeIcon.transform.SetParent(lifeIconContainer.transform, false);

                if (i < currentLifeAmount)
                {
                    //Change Color
                    InstantiatedLifeIcon.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
                else if (i >= currentLifeAmount)
                {
                    //Change Color
                    InstantiatedLifeIcon.GetComponent<Image>().color = new Color32(25, 0, 0, 255);
                }

                //Add to List
                lifeHeartsList.Add(InstantiatedLifeIcon);
            }
        }

        #endregion
    }

    public void ChangeHealth(int changeHealthAmount)
    {
        //Changes Health Amount
        #region CHANGE HEALTH AMOUNT

        //If gaining Health
        if (changeHealthAmount > 0)
        {
            if (changeHealthAmount + currentHealthAmount < maxHealthAmount)
            {
                currentHealthAmount += changeHealthAmount;
            }
            else
            {
                currentHealthAmount = maxHealthAmount;
            }
        }

        //If loosing Health
        else if (changeHealthAmount < 0)
        {
            if (changeHealthAmount + currentHealthAmount > 0)
            {
                currentHealthAmount += changeHealthAmount;
            }
            else
            {
                #region PLAYER LOOSES LIFE

                //Player Has Zero Health
                currentHealthAmount = 0;

                //Removes One Life
                ChangeLife(-1);

                //Restore Health
                if(currentLifeAmount > 0)
                {
                    currentHealthAmount = maxHealthAmount;
                }
                else
                {
                    //GAME OVER
                    Debug.Log("Game Over (Lost all Lifes)");
                }

                #endregion
            }
        }

        InstantiateLifeIcons();

        #endregion

        //Changes the Health UI
        ChangeHealthUI();
    }

    public void ChangeLife(int changeLifeAmount)
    {
        //Changes Life Amount
        #region CHANGE LIFE AMOUNT

        //If gainig Life
        if (changeLifeAmount > 0)
        {
            if (changeLifeAmount + currentLifeAmount <= maxLifeAmount)
            {
                currentLifeAmount += changeLifeAmount;
            }
            else
            {
                currentLifeAmount = maxLifeAmount;
            }
        }

        //If loosing Life
        else if (changeLifeAmount < 0)
        {
            if (changeLifeAmount + currentLifeAmount >= 0)
            {
                currentLifeAmount += changeLifeAmount;
            }
            else
            {
                currentLifeAmount = 0;
            }
        }

        #endregion

        //Changes the Health UI
        ChangeHealthUI();
    }

    float CalculateHealth()
    {
        //Calculate Health
        currentHealthPercentage = (float)currentHealthAmount / (float)maxHealthAmount;
        return currentHealthPercentage;
    }

    public void ChangeHealthUI()
    {
        //Change Health UI
        #region CHANGE HEALTH SLIDER

        //Change HealthSlider
        if (healthSlider != null)
        {
            //Change Health Slider Value
            healthSlider.value = CalculateHealth();
        }
        
        //Change Health Text
        if (healthText != null)
        {
            //Change Health Text
            healthText.text = currentHealthAmount + " / " + maxHealthAmount;
        }

        #endregion
    }
}