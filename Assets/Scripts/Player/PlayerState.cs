using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerState : MonoBehaviour
{
    public String ItemOnHand;
    public bool canChop = false;
    public bool canFire = false;

    public float baseSpeed = 5.0f;

    public bool sneakSpeed = false;

    public float speedCharacter = 10.0f;

    public float hunger;
    public float health;

    public InventorySystem inventorySystem;

    public Vector3 position;

    public bool isFlip = false;

    public uint woodCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        if(ItemOnHand == "Axe"){
            canChop = true;
        }else{
            canChop = false;
        }

        if(inventorySystem.GetItemCount("Wood") >= 3){
            canFire = true;
        }else{
            canFire = false;
        }

        speedCharacter = baseSpeed + (baseSpeed*(hunger/100.0f));


    }
}
