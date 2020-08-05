using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaueMenu : MonoBehaviour
{
    #region VARIABLES

    public GameObject pauseMenu;
    public GameObject pauseButton;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void PauseMenu()
    {
        //Game is already paused
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;

            pauseMenu.SetActive(false);
        }

        //Game is not paused yet
        else if(Time.timeScale == 1)
        {
            Time.timeScale = 0;

            pauseMenu.SetActive(true);
        }
    }
}
