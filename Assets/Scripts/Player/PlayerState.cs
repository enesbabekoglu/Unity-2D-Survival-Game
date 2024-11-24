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
    }
}
