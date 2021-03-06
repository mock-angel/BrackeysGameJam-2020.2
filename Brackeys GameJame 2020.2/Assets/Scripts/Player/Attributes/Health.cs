﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    #region VARIABLES

    //Components & Scripts
    CheckPoint checkpointscript;

    //General
    [Header("GENERAL")]
    public bool isPlayer;

    //Health
    [Header("HEALTH")]
    public float maxHealthAmount = 10;
    public float currentHealthPercentage;
    public float currentHealthAmount;

    public float invincibleTime = 0.5f;
    public float isInvincibleCounter;

    //Life
    [Header("LIFE")]
    public int maxLifeAmount = 1;
    public int currentLifeAmount;

    //Health UI
    [Header("HEALTH UI")]
    public GameObject healthIconContainer;
    public GameObject healthIconPrefab;
    public List<GameObject> healthIconList;
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

        if (isPlayer)
        {
            //Assign Components & Scripts
            checkpointscript = gameObject.GetComponent<CheckPoint>();

            //List
            lifeHeartsList = new List<GameObject>();
        }

        //Reset Health (refill Health)
        ResetHealthLife();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ChangeHealth(-1);
            }
        }

        //Reset invincibility
        if(isInvincibleCounter > 0)
        {
            isInvincibleCounter -= Time.deltaTime;
        }
    }

    public void ResetHealthLife()
    {
        //Reset Health
        #region RESET HEALTH
        currentHealthAmount = maxHealthAmount;
        currentLifeAmount = maxLifeAmount;

        if (isPlayer)
        {
            ChangeHealthUI();
            InstantiateLifeIcons();
        }

        #endregion
    }

    public void InstantiateLifeIcons()
    {
        if (lifeIconContainer != null && lifeIconPrefab != null)
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

                //Clear list
                lifeHeartsList.Clear();

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
    }

    public void ChangeHealth(int changeHealthAmount)
    {
        //Changes Health Amount
        #region CHANGE HEALTH AMOUNT

        //If gaining Health
        if (changeHealthAmount > 0)
        {
            //Change Health
            if (changeHealthAmount + currentHealthAmount < maxHealthAmount)
            {
                currentHealthAmount += changeHealthAmount;
            }
            else
            {
                currentHealthAmount = maxHealthAmount;
            }

            PlatformerMovement.Instance.GainHealthParticles.Play();
        }

        //If loosing Health
        else if (changeHealthAmount < 0 && isInvincibleCounter <= 0)
        {
            //Player is invincible
            isInvincibleCounter = invincibleTime;

            //Change Health
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
                
                PlatformerMovement.Instance.LoseLifeParticles.Play();
                #endregion
            }


        }

        InstantiateLifeIcons();

        #endregion

        if (isPlayer)
        {
            //Changes the Health UI
            ChangeHealthUI();
        }
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

            PlatformerMovement.Instance.GainLifeParticles.Play();
        }

        //If loosing Life
        else if (changeLifeAmount < 0)
        {
            if (changeLifeAmount + currentLifeAmount > 0)
            {
                currentLifeAmount += changeLifeAmount;
            }
            else
            {
                //Player Dies
                PlayerDies();
            }

            PlatformerMovement.Instance.LoseLifeParticles.Play();
        }

        #endregion

        if (isPlayer)
        {
            //Changes the Health UI
            ChangeHealthUI();
        }
    }

    public void PlayerDies()
    {
        #region PLAYER DIES

        currentLifeAmount = 0;

        if (isPlayer)
        {
            //Move To CheckPoint
            checkpointscript.MoveToCheckPoint(gameObject);

            //Reset Health
            ResetHealthLife();

            //GAME OVER
            Debug.Log("Game Over (Lost all Lifes)");
        }

        #endregion
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
        if (healthIconContainer != null && healthIconPrefab != null)
        {
            //Instantiates Heart Icons based on maxHealthHeartAmount and adds them to list
            #region INSTANTIATE & ADD TO LIST

            if (healthIconPrefab != null && healthIconContainer != null)
            {
                //Heart are deleted
                for (int i = 0; i < healthIconList.Count; i++)
                {
                    Destroy(healthIconList[i].gameObject);
                }

                //Clear list
                healthIconList.Clear();

                //Health is instantiated
                for (int i = 0; i < maxHealthAmount / 2; i++)
                {
                    //instantiate
                    GameObject InstantiatedHealthIcon = Instantiate(healthIconPrefab, transform.position, Quaternion.identity);

                    //Set Parent
                    InstantiatedHealthIcon.transform.SetParent(healthIconContainer.transform, false);

                    if (i < (currentHealthAmount / 2 - 0.5f))
                    {
                        //Change Color
                        InstantiatedHealthIcon.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                    else if (i < currentHealthAmount / 2)
                    {
                        //Change Color
                        InstantiatedHealthIcon.GetComponent<Image>().color = new Color32(125, 150, 0, 255);
                    }
                    else
                    {
                        InstantiatedHealthIcon.SetActive(false);
                    }

                    //Add to List
                    healthIconList.Add(InstantiatedHealthIcon);
                }
            }

            #endregion
        }
    }
}