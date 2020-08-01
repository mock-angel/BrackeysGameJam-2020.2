using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    #region VARIABLES

    public int maxHealthAmount;
    public int currentHealthAmount;

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    }
}