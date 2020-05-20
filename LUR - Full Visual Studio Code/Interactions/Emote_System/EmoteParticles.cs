using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README: Attached to each NPC the player can talk to.

//Creates public references to all of that character's emotes which can be played during dialogue.

//The Interactable reference also works with cinematic cam events for when the Dialogue class needs

//to reach back through the Emote system to turn off the conversation. This is only required when cinematic

//events are changing the camera angles line by line.

public class EmoteParticles : MonoBehaviour
{
    public Interactable interactable;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    [Header("Emote Particles")]
    #region Emote Particles
    public ParticleSystem Sleeping;
    public ParticleSystem Waiting;
    public ParticleSystem Exclamation;
    public ParticleSystem Angry;
    public ParticleSystem Shocked;
    public ParticleSystem Confused;
    public ParticleSystem Dizzy;
    public ParticleSystem Happy;
    #endregion

}
