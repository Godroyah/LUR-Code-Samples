using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Though tiny, this class is significant since all other custom built events designed to be triggered by cinematic cameras

//inherit from this class, therefore creating a single universal reference that could be used to trigger any cinematic event

//regardless of its specific construction.

public class Event_Type : MonoBehaviour
{
    public string eventName;

    public virtual void StartEvent()
    {
        //This method is meant to be overwritten
        Debug.Log(eventName + " has been activated.");

    }

}
