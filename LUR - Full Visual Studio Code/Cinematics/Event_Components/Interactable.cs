using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README: Designed primarily as the top of the hierachy for interactable objects in the game world, w/Interact() serving as source of customizable function by object.

public class Interactable : MonoBehaviour
{
    protected PlayerController playerController;
    protected ObjectPreferences objPrefs;
    protected AudioSource audioSource;
    protected DialogueManager dialogueManager;

    public Event_Trigger eventTrigger;

    public GameObject billboard_UI;

    public Transform dialogueViewPoint;

    //Determines whether or not interactable can activate a cutscene and how the cutscene should be activated through this interactable
    public bool usesCamEvent;
    public bool headButtActivation;
    public bool dialogueActivation;
    public bool triggerActivation;
    public bool doneTalking;
   

    private void Awake()
    {
        if (usesCamEvent && headButtActivation && eventTrigger == null)
        {
            eventTrigger = GetComponent<Event_Trigger>();
            if (eventTrigger == null)
            {
                Debug.LogWarning("This object is marked as using a Cam Event! Either mark 'usesCamEvent' as false or attach an Event_Trigger script to this object!");
            }
        }

        objPrefs = GetComponent<ObjectPreferences>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Interact()
    {
        // This method is meant to be overwritten
        Debug.Log(gameObject.name + " has been interacted with.");

        //Interactable cutscene activation. Added in order to facilitate the activation of the Event_Trigger for cinematic events.

        //The means of activation can be customized by marking it for activation by headbutt, tripping a trigger volume, or initiating dialogue.

        if (usesCamEvent)
        {
            if(headButtActivation || triggerActivation)
            {
                eventTrigger.InitiateEvent();
            }
            else if (dialogueActivation)
            {
                Debug.Log("Prep scene");
                dialogueManager.prepCamEvent = true;
            }
        }

        if (objPrefs != null && audioSource != null)
            audioSource.Play();
    }
}

