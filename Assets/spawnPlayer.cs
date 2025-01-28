using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("player");
        if (player != null)
        {
            playerController playerScript = player.GetComponent<playerController>();
            playerScript.teleportPlayer(transform.position);
            gameObject.SetActive(false);
        }
    }
}
