using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//README:
//An example of one of many cinematic events in the game; this one controls the awakening of the Angel Tree in the level Tree Seat

public class Mulchant_AngelTree_Event : Event_Type
{
    //grabs the Animator Controllers for the AngelTree and the iteration of the Mulchant character specially placed for the event, as well as the Mesh Renderer for his bottle

    public Animator treeAnim;
    public Animator mulchantStandInAnim;
    public SkinnedMeshRenderer eventMulchant;
    public GameObject mulchantBottle;

    [Range(1f, 20f)]
    public float mulchantDelay;
    [Range(1f, 20f)]
    public float treeDelay;
    //[Range(1f, 20f)]
    //public float endSceneDelay;

    [Space(10), Header("Mulchant Bottle Particle")]
    public ParticleSystem mulchantBottle_ParticleSystem;
    [Range(1f, 20f)]
    public float particleSystemDelay;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        eventMulchant.enabled = false;
    }

    public override void StartEvent()
    {
        base.StartEvent();

        StartCoroutine(TreeAwakening());
        if (mulchantBottle_ParticleSystem != null)
        {
            StartCoroutine(MulchantParticle());
        }
    }


    //Makes the special version of the Mulchant appear and play his "bottle shaking" animation, which then wakes up the tree

    //Timed delays are placed within the code as WaitForSeconds() to allow the camera time to shift into place

    //The appropriate booleans marking the completion of relevant world events are then flipped for world continuity going forward, stored in the level's GameController 
    //(ex.gameController.angelTreeAwake at line 72)

    IEnumerator TreeAwakening()
    {
        eventMulchant.enabled = true;

        yield return new WaitForSeconds(mulchantDelay);

        AudioManager.Instance.Play_Tree_EyeOpening();
        mulchantStandInAnim.SetTrigger("AwakenTree");
        mulchantBottle.SetActive(true);

        yield return new WaitForSeconds(treeDelay);

        treeAnim.SetBool("Awake", true);
        mulchantBottle.SetActive(false);
        eventMulchant.enabled = false;
        gameController.angelTreeAwake = true;

        //yield return new WaitForSeconds(endSceneDelay);
    }

    IEnumerator MulchantParticle()
    {
        yield return new WaitForSeconds(particleSystemDelay);
        if (!mulchantBottle_ParticleSystem.isPlaying)
        {
            mulchantBottle_ParticleSystem.Play();
        }
    }
}
