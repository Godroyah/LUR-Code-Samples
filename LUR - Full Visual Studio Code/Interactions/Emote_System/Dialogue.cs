using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//README: This script controls the looping of "Frames" (individual lines of a conversation), as designed by my co-programmer.
//I also upgraded it to be able to find and activate character emotes, as well as to interact with the Event_Trigger class for cinematic cam events.

public class Dialogue : MonoBehaviour
{
    //Frame index tracks which line of a conversation the player is on for camera event purposes when the camera angle needs to change line by line.
    //The SceneName string matches up with an enum in DialogueManager that is used to reference specific conversations, which classes such as
    //F_Forest_NPC_Talk grab onto to make sure the correct conversation is being accessed.

    public int frameIndex;
    [Tooltip("Name of the Scene for the DialogueManager to Find")]
    public string SceneName;
    
    //Frames are the individual lines of text in a conversation.
    //The bool array shotChange is checked against the EmoteChecker class on each frame to see if an active cinematic camera should change to the next predetermined angle.

    public GameObject[] Frames;
    [Space(5)]
    [Header("Frame Triggers for Event Cam Shots")]
    public bool[] shotChange;


    [Space(5)]
    [Header("Text Done?")]
    public bool textIsDone;

    [Space(5)]

    //The NPC string checks against the string of a similar name in DialogueManager's serialized class NPC_Emotes. 
    //EmoteCheck references the individual frame to get which kind of emote should be played, if any

    [Header("Emote ID")]
    public string NPC;
    EmoteCheck emoteCheck;
    CamControl camControl;
    DialogueCam dialogueCam;
    Event_Trigger eventTrigger;
    GameController gameController;
    [Space(10)]
    public DialogueManager dialogueManager;

    public TextDisplayer currentTextDisplayer;
    public bool hasFinishedDisplayingText = false;
    public bool isCamEventActive;

    private void Start()
    {
        
        gameController = GameController.Instance;
        camControl = GameObject.Find("Camera_Jig").GetComponent<CamControl>();
        dialogueCam = GameObject.Find("Dialogue_Cam").GetComponent<DialogueCam>();

        for (int i = 0; i < Frames.Length; i++)
        {
           
            Frames[i].SetActive(false);
        }

        if (isCamEventActive)
        {
            if (eventTrigger == null)
            {
                eventTrigger = GetComponent<Event_Trigger>();
            }
        }
    }

    public void StartScene()
    {
        if (!isCamEventActive)
        {
            StartCoroutine(Scene());
        }
        else if (isCamEventActive && eventTrigger.interactionStarted == false)
        {
            StartCoroutine(Scene());
        }
    }

    //When the Coroutine Scene() is accessed, it loops through the Frames (lines) of dialogue.

    //FindEmote() is accessed when it finds a line that is marked to have an emote play.

    //FindEmote() then loops through the DialogueManager's NPC list, checking them against its own NPC string name.

    //Once it finds the correct character via that list, it will then check the EmoteCheck class on that line for

    //which emote it should play, and then access the correct character's attached emotes and play the correct one.

    IEnumerator FindEmote()
    {
        for(int i = 0; i < dialogueManager.npcs.Length; i++)
        {
            if (NPC == dialogueManager.npcs[i].NPC)
            {
                if (emoteCheck.Emote == EmoteType.SLEEPING)
                {
                    dialogueManager.npcs[i].npcEmotes.Sleeping.Play();
                }
                else if (emoteCheck.Emote == EmoteType.WAITING)
                {
                    dialogueManager.npcs[i].npcEmotes.Waiting.Play();
                }
                else if (emoteCheck.Emote == EmoteType.EXCLAMATION)
                {
                    dialogueManager.npcs[i].npcEmotes.Exclamation.Play();
                }
                else if (emoteCheck.Emote == EmoteType.ANGRY)
                {
                    dialogueManager.npcs[i].npcEmotes.Angry.Play();
                }
                else if (emoteCheck.Emote == EmoteType.SHOCKED)
                {
                    dialogueManager.npcs[i].npcEmotes.Shocked.Play();
                }
                else if (emoteCheck.Emote == EmoteType.CONFUSED)
                {
                    dialogueManager.npcs[i].npcEmotes.Confused.Play();
                }
                else if (emoteCheck.Emote == EmoteType.DIZZY)
                {
                    dialogueManager.npcs[i].npcEmotes.Dizzy.Play();
                }
                else if (emoteCheck.Emote == EmoteType.HAPPY)
                {
                    dialogueManager.npcs[i].npcEmotes.Happy.Play();
                } 
            }
        }
        yield return null;
    }


    //Scene() is the primary Coroutine for looping through lines of a conversation when the player talks to a character.

    //I modified this system designed by my co-programmer to, for each frame, check that frame's EmoteCheck class.

    //If the class says an emote should be played here, it then begins FindEmote() to find the correct character

    //and activate the correct emote.

    IEnumerator Scene()
    {
        //Debug.Log("Scene Started");
        //interactHandle = gameController.interactInput;

        if (dialogueManager != null)
        {
            //Debug.Log("Still working?");
            dialogueManager.hasActiveDialogue = true;
        }

        for (int i = 0; i < Frames.Length; i++)
        {
            frameIndex = i;
            //  Debug.Log("Depth Level 1");
            if (i != 0)
                Frames[i - 1].SetActive(false);
            Frames[i].SetActive(true);
            emoteCheck = Frames[i].GetComponent<EmoteCheck>();
            if(emoteCheck != null)
            {
                if (emoteCheck.play == true)
                {
                    StartCoroutine(FindEmote());
                }
            }

            yield return new WaitForEndOfFrame();

            // Wait while the text hasn't finished or the player interacts to finish the text
            yield return new WaitUntil(() => (Input.GetButtonDown(gameController.interactInput) || (hasFinishedDisplayingText || currentTextDisplayer == null)));

            if (i == Frames.Length - 1)
            {
                textIsDone = true;
            }

            //      Debug.Log("Depth Level 3");
            if (/*Input.GetButtonDown("Interact") ||*/ hasFinishedDisplayingText || currentTextDisplayer == null)
            {
                
                hasFinishedDisplayingText = false;
                currentTextDisplayer = null;

                // Check if the Frame has a DialogueOption
                Frame tempFrame = Frames[i].GetComponent<Frame>();
                if (tempFrame != null)
                {
                    //Debug.Log("Unlocking Now!");
                    camControl.lockPosition = true;
                    Cursor.lockState = CursorLockMode.None;
                    //Debug.Log("Can You See Me?");
                    Cursor.visible = true;
                    //Debug.Log("Cursor Unlocked");
                    if(tempFrame.dialogueButtons.firstSelectedGameObject != tempFrame.firstButton)
                    {
                        tempFrame.dialogueButtons.firstSelectedGameObject = tempFrame.firstButton;
                    }
                    yield return new WaitWhile(() => tempFrame.Get_ShouldWait() == true);
                    //Debug.Log("Cursor Frozen");
                    camControl.lockPosition = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    if (tempFrame.Get_ShouldContinue() == false)
                    {
                        i = Frames.Length + 2;
                        break;
                    }
                    else
                        continue; // This skips the need to press the interact key again

                }
                else
                {
                    yield return new WaitUntil(() => Input.GetButtonDown(gameController.interactInput));
                }
                
                continue; // This continues to the next frame
            }
            else
            {
                currentTextDisplayer.DisplayFullText();
                //Wait for text to display full text
                yield return new WaitForSeconds(0.1f);

                // Check if the Frame has a DialogueOption
                Frame tempFrame = Frames[i].GetComponent<Frame>();
                if (tempFrame != null)
                {
                    Debug.Log("Unlocking Now!");
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log("Can You See Me?");
                    Cursor.visible = true;
                    Debug.Log("Cursor Unlocked");
                    yield return new WaitWhile(() => tempFrame.Get_ShouldWait() == true);
                    Debug.Log("Cursor Frozen");
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    //tempFrame.Reset_ShouldWait();

                    if (tempFrame.Get_ShouldContinue() == false)
                    {
                        i = Frames.Length + 2;
                        //tempFrame.Reset_ShouldWait();
                        break;
                    }
                    else
                    {
                        tempFrame.Reset_ShouldWait();
                        continue; // This skips the need to press the interact key again
                    }
                }
            }



            yield return new WaitUntil(() => Input.GetButtonDown(gameController.interactInput));


            #region OldCode
            //yield return new WaitForEndOfFrame();
            //while (true)
            //{
            ////    Debug.Log("Depth Level 2");
            //    // TODO: Extremely high polling number for user input
            //    yield return new WaitForSeconds(0.00001f);
            //    if (Input.GetButtonDown("Interact"))
            //    {
            //  //      Debug.Log("Depth Level 3");
            //        if (hasFinishedDisplayingText || currentTextDisplayer == null)
            //        {
            //            hasFinishedDisplayingText = false;
            //            currentTextDisplayer = null;
            //            break;
            //        }
            //        else
            //        {
            //            currentTextDisplayer.DisplayFullText();
            //            //Wait for text to display full text
            //            yield return new WaitForSeconds(0.1f);


            //            while (true)
            //            {
            //    //            Debug.Log("Depth Level 4");
            //                // TODO: Extremely high polling number for user input
            //                yield return new WaitForSeconds(0.00001f);
            //                if (Input.GetButtonDown("Interact"))
            //                {
            //      //              Debug.Log("Depth Level 5");
            //                    break;
            //                }
            //            }

            //            break;
            //        }

            //    }
            //}
            #endregion
        }

        hasFinishedDisplayingText = false;
        Frames[Frames.Length - 1].SetActive(false);


        //The for loop below is a bit of a back-door solution to allow the player to properly exit a cinematic event which changes the camera angles per line of dialogue.

        //I needed a reference all the way back to the character the player was actually talking to in the moment to make this work. Though not designed originally to

        //perform this function, the Emote system's architecture allowed me access back to the character that would have otherwise necessitated the creation of additional functionality.

        for (int i = 0; i < dialogueManager.npcs.Length; i++)
        {
            if (NPC == dialogueManager.npcs[i].NPC)
            {
                dialogueManager.npcs[i].npcEmotes.interactable.doneTalking = true;
            }
        }

                //Calls on Event_Trigger to start a cam event
                //KNOWN BUG: Currently is causing the next (or last) frame of dialogue to repeat during cam event.
                //Debug.Log("Dialogue is done!");
                if (dialogueManager.prepCamEvent && isCamEventActive)
        {
            //dialogueManager.hasActiveDialogue = false;
            eventTrigger.InitiateEvent();
            yield return new WaitUntil(() => eventTrigger.GetEventCam().startScene == false);
        }

        if (dialogueManager != null)
        {
            dialogueManager.hasActiveDialogue = false;
        }

        dialogueCam.RestPosition();

        //Camera.main.orthographicSize = tempNum;
        yield return null;
    }
}

/*
// Check if the Frame has a DialogueOption
Frame tempFrame = Frames[i].GetComponent<Frame>();
                        if (tempFrame != null)
                        {
                            yield return new WaitWhile(() => tempFrame.Get_ShouldWait() == true);

                            if (tempFrame.Get_ShouldContinue() == false)
                            {
                                i = Frames.Length + 2;
                                break;
                            }

                        }
    */
