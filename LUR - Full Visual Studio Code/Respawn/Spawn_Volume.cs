using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType { NONE, START, RESPAWN, KILL}

//README: Provides options for creating a starting spawn position for the player, spawning checkpoints in the event of player death, and a kill volume for dangerous areas.
//The enum above serves as a means of labeling each placed volume for this purpose.

public class Spawn_Volume : MonoBehaviour
{
    public SpawnType spawnType; // This gives a drop down menu of the spawn types

    public Transform spawnPoint;
    private Transform playerSpawn;
    public bool activeCorridor;
    public PlayerController playerController;

    private void Awake()
    {
        if (spawnType == SpawnType.START)
        {
            activeCorridor = true;
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            playerController.currentSpawn = spawnPoint;
            playerSpawn = playerController.currentSpawn;

        }
    }

    // This probably would have been better to place within an OnTrigger as well as I don't believe there was a need to have these conditions update continuously.
    void Update()
    {
        if ((spawnType == SpawnType.START || spawnType == SpawnType.RESPAWN && activeCorridor && playerSpawn != spawnPoint))
        {
            activeCorridor = false;
            playerSpawn = spawnPoint;
        }
    }


    //Either reassigns the player's respawn point if the spawnType enum is marked RESPAWN, or turns the collider used for player detection into a killbox instead.
    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            if (playerController == null)
            {
                playerController = player.GetComponentInParent<PlayerController>();
            }
            
            if (spawnType == SpawnType.RESPAWN)
            {
                //player.GetComponent<PlayerController>().currentSpawn = spawnPoint;
                playerSpawn = playerController.currentSpawn;
                playerController.currentSpawn = spawnPoint.transform;
                activeCorridor = true;
            }
            else if(spawnType == SpawnType.KILL)
            {
                playerController.KillPlayer();
            }
        }
    }
}
