﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

//README: Class for navigating the Pause Menu UI. Displays

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject quitOption;
    public GameObject settingsOption;
    public GameObject acornBoy;
    public GameObject post;
    //public GameObject pauseEventSystem;
    public GameObject decoration;
    //public Button resumeButton;
    //public Button quitButton;

    //public RawImage acornDisplay;
    public TextMeshProUGUI acornText;
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityValue;

    public RawImage angelBottle;
    public RawImage starBottle;
    public RawImage willowBottle;

    GameController gameController;

    public bool paused;

    private void Start()
    {
        paused = false;

        gameController = GameController.Instance;
        gameController.sensitivityBar = sensitivitySlider;
        sensitivitySlider.onValueChanged.AddListener(delegate { gameController.SensitivityValueCheckPause(); });
        gameController.pauseMenu = GetComponent<PauseMenu>();
    }


    //In the future the Pause() function should be tied specifically to a keyboard/gamepad button press rather than a continuously checked function in Update.
    private void Update()
    {
        Pause();
    }


    //The functions here (with the exception of Pause) are similarly independent and referenced by UI buttons in the game. They are a bit more complicated due to the need

    //To juggle UI elements being active at certain parts of the game, specifically the bottles which become visible and/or change appearance depending on world conditions.

    public void Pause()
    {
        //THIS RIGHT HERE IS A JANK SOLUTION. Pausing keeps deleting the 3rd bottle reference in SelfReport (which right now is the green bottle but used to be the yellow bottle
        //before I put in the NONE enum. NEED to come back and fix this at a later date but for now this is working.
        gameController.greenBottle = starBottle;
        // Debug.Log("Active?");
        

        if (Input.GetButton(gameController.pauseInput))
            {
            AudioManager.Instance.Play_UI_Click_PauseMenu();
            if (!paused)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    if (pauseMenu != null)
                    {
                        pauseMenu.SetActive(true);
                        decoration.SetActive(true);
                    // acornDisplay.enabled = true;
                        acornBoy.SetActive(true);
                        post.SetActive(true);
                        acornText.enabled = true;
                        if(gameController.mulchant_GivenBottles)
                        {
                            if(!gameController.angelTreeAwake)
                            angelBottle.enabled = true;
                            if(!gameController.starTreeAwake)
                            starBottle.enabled = true;
                            if(!gameController.willowTreeAwake)
                            willowBottle.enabled = true;
                        }
                    }
                    Time.timeScale = 0;
                    //paused = true;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    if (pauseMenu != null)
                    {
                        pauseMenu.SetActive(false);
                        quitOption.SetActive(false);
                        decoration.SetActive(false);
                        acornText.enabled = false;
                        settingsOption.SetActive(false);
                    //resumeButton.interactable = true;
                    //quitButton.interactable = true;
                }
                    Time.timeScale = 1;
                    //paused = false;
                }
            }

            if (Input.GetButtonUp(gameController.pauseInput))
            {
                if (!paused)
                    paused = true;
                else if (paused)
                {
                //Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;
                //acornDisplay.enabled = false;
                acornText.enabled = false;
                if (gameController.mulchant_GivenBottles)
                {
                    angelBottle.enabled = false;
                    starBottle.enabled = false;
                    willowBottle.enabled = false;
                }
                paused = false;
                }
            }
        
    }

    public void Resume()
    {
        AudioManager.Instance.Play_Stinger_Start_MainMenu();
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        decoration.SetActive(false);
        //acornDisplay.enabled = false;
        acornText.enabled = false;
        if (gameController.mulchant_GivenBottles)
        {
            angelBottle.enabled = false;
            starBottle.enabled = false;
            willowBottle.enabled = false;
        }
        Time.timeScale = 1;
        paused = false;
    }

    public void Settings()
    {
        AudioManager.Instance.Play_UI_Click_MainMenu();
        settingsOption.SetActive(true);
        pauseMenu.SetActive(false);
        acornBoy.SetActive(false);
        post.SetActive(false);
        angelBottle.enabled = false;
        starBottle.enabled = false;
        willowBottle.enabled = false;
        acornText.enabled = false;

    }

    public void Back()
    {
        AudioManager.Instance.Play_UI_Click_MainMenu();
        pauseMenu.SetActive(true);
        settingsOption.SetActive(false);
        acornBoy.SetActive(true);
        post.SetActive(true);
        acornText.enabled = true;
        if(gameController.mulchant_GivenBottles)
        {
            if (gameController.angelTreeAwake)
                angelBottle.enabled = true;
            if (gameController.starTreeAwake)
                starBottle.enabled = true;
            if (gameController.willowTreeAwake)
                willowBottle.enabled = true;
        }
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play_UI_Click_MainMenu();
        //pauseMenu.SetActive(false);
        //resumeButton.interactable = false;
        //quitButton.interactable = false;
        quitOption.SetActive(true);
        pauseMenu.SetActive(false);
        //pauseEventSystem.SetActive(false);
    }

    public void QuitToMenu()
    {
        AudioManager.Instance.Play_Stinger_Back_MainMenu();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        
    }

    public void CancelQuit()
    {
        AudioManager.Instance.Play_UI_Click_MainMenu();
        pauseMenu.SetActive(true);
        quitOption.SetActive(false);
        //resumeButton.interactable = true;
        //quitButton.interactable = true;
        //pauseEventSystem.SetActive(true);
    }

    public void QuitApplication()
    {
        AudioManager.Instance.Play_Stinger_Back_MainMenu();
        Debug.Log("Quit");
        Application.Quit();
        
    }


}
