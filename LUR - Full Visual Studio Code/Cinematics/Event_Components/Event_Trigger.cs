﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Trigger : MonoBehaviour
{
    private PlayerController playerController;

    public GameObject cinematicCamera;

    //public Dialogue conversation;

    private Camera cameraComponent;

    private Event_Cam eventCamController;

    GameObject[] playerUI;

    private bool firstInteraction;
    public bool interactionStarted;
    public bool alwaysInteract;
    public bool interactableShots;
    private bool sceneStarted;

    private void Start()
    {
        playerUI = new GameObject[GameObject.FindGameObjectsWithTag("Player_UI").Length];
        playerUI = GameObject.FindGameObjectsWithTag("Player_UI");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        eventCamController = cinematicCamera.GetComponent<Event_Cam>();
        cameraComponent = cinematicCamera.GetComponent<Camera>();

        cameraComponent.enabled = false;
        firstInteraction = true;
        interactionStarted = false;
        sceneStarted = false;
    }

    public Event_Cam GetEventCam()
    {
        return eventCamController;
    }

    private void Update()
    {

        if (interactionStarted && !eventCamController.startScene && sceneStarted)
        {
            StartCoroutine(EndTime());
        }
    }


    //Kicks off a new cinematic event based on whether or not this Event_Trigger had already been activated and could reactivate the same cinematic event multiple times
    public void InitiateEvent()
    {
        if (firstInteraction || alwaysInteract)
        {
            if(interactableShots)
            {
                eventCamController.isInteractable = true;
            }
            eventCamController.startScene = true;
            interactionStarted = true;

            StartCoroutine(ShowTime());
        }
    }


    //Tells the Event_Cam associated with this trigger to begin looping through camera shots based on their own parameters while locking the player's input temporarily
    IEnumerator ShowTime()
    {
        eventCamController.startScene = true;
        interactionStarted = true;
        sceneStarted = true;


        if (eventCamController.startScene && interactionStarted)
        {

            playerController.HorizontalInput = 0;
            playerController.VerticalInput = 0;
            playerController.HeadbuttInput = 0;

            playerController.eventActive = true;


            //playerController.camControl.myCamera.enabled = false;

            foreach (GameObject ui in playerUI)
            {
                ui.SetActive(false);
            }

            //playerController.camControl.enabled = false;
            cameraComponent.enabled = true;
        }

        yield return null;
    }

    //Reactivates the player's main camera as well as resuming player input
    IEnumerator EndTime()
    {

        cameraComponent.enabled = false;
        //playerController.camControl.enabled = true;

        foreach (GameObject ui in playerUI)
        {
            ui.SetActive(true);
        }

        //playerController.camControl.myCamera.enabled = true;

        playerController.eventActive = false;

        playerController.HorizontalInput = Input.GetAxis(GameController.Instance.horizontalInput);
        playerController.VerticalInput = Input.GetAxis(GameController.Instance.verticalInput);
        playerController.HeadbuttInput = Input.GetAxis(GameController.Instance.headbuttInput);
        firstInteraction = false;
        sceneStarted = false;
        interactionStarted = false;

        yield return new WaitForEndOfFrame();
    }
}
