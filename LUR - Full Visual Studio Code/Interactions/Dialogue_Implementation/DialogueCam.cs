﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README: A class attached to a special camera which remains with the player and activates when talking to an NPC,
//snapping to a pre-determined location for each character. This is NOT part of the cinematic camera system and is a standalone.

public class DialogueCam : MonoBehaviour
{
    public PlayerController playerController;

    public Transform restPosition;

    public Camera thisCamera;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        if(restPosition != null)
        {
            transform.position = restPosition.position;
            transform.rotation = restPosition.rotation;
        }
    }


    //Turns the camera on and positions it where it needs to go when the player chooses to engage conversation with an NPC.
    public void TalkPosition()
    {
        Debug.Log("Start Talking");
        if(playerController.currentTarget.dialogueViewPoint != null)
        {
            transform.position = playerController.currentTarget.dialogueViewPoint.position;
            transform.rotation = playerController.currentTarget.dialogueViewPoint.rotation;

            thisCamera.enabled = true;
        }
    }

    //Deactivates the camera and moves it back to its position by the player until it needs to be called again.
    public void RestPosition()
    {
        Debug.Log("Stop Talking");
        if (playerController.currentTarget.dialogueViewPoint != null)
        {
            thisCamera.enabled = false;

            transform.position = restPosition.position;
            transform.rotation = restPosition.rotation;
        }
    }

}
