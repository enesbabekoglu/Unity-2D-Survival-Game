using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class followHolder : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float hoverRadius = 1.0f;
    
    public GameObject player;
    public PlayerState pState;

    public String ItemType;

    public Color unavailableColor;
    public Color availableColor;

    public float Xoffset;
    public float Yoffset;

    public bool isFlip = false;
    private bool playerNear = false;
    private bool followPlayer = false;

    private bool mouseNear = false;

    public SpriteRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("player");
        pState = player.GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    

        if(Math.Abs(mousePos.x - transform.position.x) < hoverRadius && Math.Abs(mousePos.y - transform.position.y) < hoverRadius){
            if(!followPlayer){
                text.gameObject.SetActive(true);
            }
            mouseNear = true;
        }else if(!playerNear){
            text.gameObject.SetActive(false);
            mouseNear = false;
        }
    
        

        if(playerNear && mouseNear && Input.GetMouseButtonDown(0) && !followPlayer){
            followPlayer = true;
            pState.ItemOnHand = ItemType;
            pState.canChop = true;
            text.gameObject.SetActive(false);

        }else if(playerNear && mouseNear && Input.GetMouseButtonDown(1) && followPlayer){
            followPlayer = false;
            pState.ItemOnHand = null;
            pState.canChop = false;
            text.gameObject.SetActive(true);

        }

        if(playerNear && Input.GetKeyDown(KeyCode.P) && !followPlayer){
            followPlayer = true;
            pState.canChop = true;
            text.gameObject.SetActive(false);

        }else if(playerNear && Input.GetKeyDown(KeyCode.P) && followPlayer){
            followPlayer = false;
            pState.canChop = false;
            text.gameObject.SetActive(true);

        }

        if(followPlayer){
            if(pState.isFlip){
                transform.transform.position = new Vector3(player.transform.position.x-Xoffset, player.transform.position.y+Yoffset, player.transform.position.z);
                if(!isFlip){
                    renderer.flipX = true;
                    isFlip = true;
                }

            }else{
                transform.transform.position = new Vector3(player.transform.position.x+Xoffset, player.transform.position.y+Yoffset, player.transform.position.z);
                if(isFlip){
                    renderer.flipX = false;
                    isFlip = false;
                }
            }
        }


    }




    void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.CompareTag("player")){
            playerNear = true;
            text.color = availableColor;
            text.gameObject.SetActive(true);
        }
        
    }

    void OnTriggerExit2D(Collider2D collider){

        if(collider.gameObject.CompareTag("player")){
            playerNear = false;
            text.color = unavailableColor;
            text.gameObject.SetActive(false);
        }
        
    }
}
