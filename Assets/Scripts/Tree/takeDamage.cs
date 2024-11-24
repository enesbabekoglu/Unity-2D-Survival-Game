using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class takeDamage : MonoBehaviour
{

    public TextMeshProUGUI text;
    public float hoverRadius = 1.0f;
    public PlayerState pState;

    public float proximityRange;
    public TreeState treeState;

    
    

    public float damagePerHit = 20.0f;

    public int woodYield = 4;

    public Color unavailableColor;
    public Color availableColor;


    private bool playerNear = false;
    private bool mouseNear = false;


    // Start is called before the first frame update
    void Start()
    {

        setUIVisibility(false);

    }

    // Update is called once per frame
    void Update()
    {

        
        if(Vector3.Distance(transform.position, pState.position) < proximityRange){
            playerNear = true;
            text.color = availableColor;
            if(treeState.health > 0.0f){
                setUIVisibility(true);
            }else{
                setUIVisibility(false);
            }
        }else{
            playerNear = false;
            text.color = unavailableColor;
            setUIVisibility(false);
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    

        if(Math.Abs(mousePos.x - transform.position.x) < hoverRadius && Math.Abs(mousePos.y - transform.position.y) < hoverRadius){
            if(treeState.health > 0.0f){
                setUIVisibility(true);
            }else{
                setUIVisibility(false);
            }
            mouseNear = true;
        }else if(!playerNear){
            setUIVisibility(false);

            mouseNear = false;
        }else{
            mouseNear = false;
        }
    
        if(playerNear && mouseNear && Input.GetMouseButtonDown(0) && pState.canChop && treeState.isAlive){
            takeDmg();
        }
    


    }

    void takeDmg(){
        treeState.health -= damagePerHit;
        treeState.health = Mathf.Clamp(treeState.health, 0, treeState.maxHealth); 

        if(treeState.health == 0.0f){
            treeState.isAlive = false;
            treeState.inventorySystem.AddItem("Wood", woodYield);
        }
    }

    void setUIVisibility(bool flag){
            text.gameObject.SetActive(flag);
            treeState.healthBar.gameObject.SetActive(flag);
    }

    void OnTriggerEnter2D(Collider2D collider){
    
    }

    void OnTriggerExit2D(Collider2D collider){

    
        
    }
}
