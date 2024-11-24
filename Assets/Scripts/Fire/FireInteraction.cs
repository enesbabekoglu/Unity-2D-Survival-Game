using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireInteraction : MonoBehaviour
{
    


    public FireControl fireControl;


    



    public Color unavailableColor;
    public Color availableColor;


    private bool mouseNear = false;



    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(transform.position, fireControl.pState.position) < fireControl.proximityRange){
            fireControl.playerNear = true;
            fireControl.text.color = availableColor;
            if(fireControl.health > 0.0f){
                fireControl.setUIVisibility(true);
            }else{
                fireControl.setUIVisibility(false);
            }
        }else{
            fireControl.playerNear = false;
            fireControl.text.color = unavailableColor;
            fireControl.setUIVisibility(false);
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    

        if(Math.Abs(mousePos.x - transform.position.x) < fireControl.hoverRadius && Math.Abs(mousePos.y - transform.position.y) < fireControl.hoverRadius){
            if(fireControl.health > 0.0f){
                fireControl.setUIVisibility(true);
            }else{
                fireControl.setUIVisibility(false);
            }
            mouseNear = true;
        }else if(!fireControl.playerNear){
            fireControl.setUIVisibility(false);

            mouseNear = false;
        }
    
        if(fireControl.playerNear && mouseNear && Input.GetMouseButtonDown(0) && fireControl.pState.canFire && !fireControl.isBurning){
            fireControl.lightFire();
        }

        


    }

   


    
    void OnTriggerEnter2D(Collider2D collider){

        
        
    }

    void OnTriggerExit2D(Collider2D collider){

        
        
    }

}
