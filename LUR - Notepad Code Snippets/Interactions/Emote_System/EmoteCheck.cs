﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmoteType { NONE, SLEEPING, WAITING, EXCLAMATION, ANGRY, SHOCKED, CONFUSED, DIZZY, HAPPY }

//README: Uses the enum EmoteType to determine which Emote should be played on this line of a conversation according to its context.
//If any EmoteType other than NONE is chosen, the script will automatically play the chosen Emote

public class EmoteCheck : MonoBehaviour
{
    
    public Dialogue dialogue;
    public EmoteType Emote;
  
    [Space(5)]

    [Header("'PLAY' IS NOT A TOY! LEAVE UNCHECKED!")]
    [HideInInspector]
    public bool play = false;
  

    private void OnEnable()
    {
        if(Emote != EmoteType.NONE)
        {
            play = true;
        }
    }

    private void OnDisable()
    {
        play = false;
    }
    public void Play_Emote(EmoteType emoteType)
    {
        if (play == true)
        {

            switch (emoteType)
            {
                case EmoteType.SLEEPING:
                 
                    break;
                case EmoteType.WAITING:
                  
                    break;
                case EmoteType.EXCLAMATION:
                    AkSoundEngine.PostEvent("emo_shocked", gameObject);
                    break;
                case EmoteType.ANGRY:
                    AkSoundEngine.PostEvent("emo_angry", gameObject);
                    break;
                case EmoteType.SHOCKED:
                    AkSoundEngine.PostEvent("emo_shocked", gameObject);
                    break;
                case EmoteType.CONFUSED:
                    AkSoundEngine.PostEvent("emo_confused", gameObject);
                    break;
                case EmoteType.DIZZY:

                    break;
                case EmoteType.HAPPY:
                    AkSoundEngine.PostEvent("emo_happy", gameObject);
                    break;
            }
        }
    }
}
