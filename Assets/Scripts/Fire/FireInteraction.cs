using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireInteraction : MonoBehaviour
{
   public TextMeshProUGUI text;
    public float hoverRadius = 1.0f;

    public Animator animator;

    public PlayerState pState;

    public Slider healthBar;
    public float maxHealth = 100.0f;

    public float damagePerHit = 20.0f;

    

    public Color unavailableColor;
    public Color availableColor;

    private float health;

    private bool playerNear = false;
    private bool mouseNear = false;

    private bool isBurning = false;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        setUIVisibility(false);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    

        if(Math.Abs(mousePos.x - transform.position.x) < hoverRadius && Math.Abs(mousePos.y - transform.position.y) < hoverRadius){
            if(health > 0.0f){
                setUIVisibility(true);
            }else{
                setUIVisibility(false);
            }
            mouseNear = true;
        }else if(!playerNear){
            setUIVisibility(false);

            mouseNear = false;
        }
    
        if(playerNear && mouseNear && Input.GetMouseButtonDown(0) && pState.canFire){
            lightFire();
        }

        if(isBurning){
            health -= Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth); 
            healthBar.value = health;
            if(health == 0.0f){
                putOutFire();
            }
        }


    }

    void lightFire(){
        isBurning = true;
        animator.SetBool("burning", true);
    }

    void putOutFire(){
        isBurning = false;
        animator.SetBool("burning", false);
    }


    void setUIVisibility(bool flag){
            text.gameObject.SetActive(flag);
            healthBar.gameObject.SetActive(flag);
    }

    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.CompareTag("player")){
            playerNear = true;
            text.color = availableColor;
            if(health > 0.0f){
                setUIVisibility(true);
            }else{
                setUIVisibility(false);
            }

        }
        
    }

    void OnTriggerExit2D(Collider2D collider){

        if(collider.gameObject.CompareTag("player")){
            playerNear = false;
            text.color = unavailableColor;
            setUIVisibility(false);
            
        }
        
    }

}
