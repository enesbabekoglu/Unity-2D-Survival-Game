using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeState : MonoBehaviour
{
    public bool isAlive = false;

    public Animator animator;
    public float maxHealth = 100.0f;
    public float health;
    public Slider healthBar;


    public PlayerState pState;

    public InventorySystem inventorySystem;

    public float respawnTime;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {

        GameObject player = GameObject.FindWithTag("player");
        pState = player.GetComponent<PlayerState>();

        inventorySystem = GameObject.FindWithTag("inventorySystem").GetComponent<InventorySystem>();

        health = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;

    }

    // Update is called once per frame
    void Update()
    {

        healthBar.value = health;

        if(isAlive){
            timer = respawnTime;
            animator.SetBool("isAlive", true);
        }else{
            animator.SetBool("isAlive", false);

            timer -= Time.deltaTime;
            if(timer <= 0){
                isAlive = true;
                health = maxHealth;
            }
        }
    }
}
