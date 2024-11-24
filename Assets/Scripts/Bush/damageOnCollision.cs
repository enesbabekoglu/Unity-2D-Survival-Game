using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class damageOnCollision : MonoBehaviour
{
    public float DPS;

    private bool isCollidingPlayer = false;

    public static HungerHealthSystem Instance;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 tempPos = transform.position;
        tempPos.z = tempPos.y;
        transform.position = tempPos;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isCollidingPlayer){
            
            Debug.Log($"Damage given: {DPS * Time.deltaTime}");
            //HungerHealthSystem.Instance.UpdateHealth(DPS * Time.deltaTime);

        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("player")){
            Debug.Log($"Collision with bush: {collider.gameObject.name}");
            isCollidingPlayer = true;
        }
    }


    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.CompareTag("player")){
            Debug.Log($"No more collision with bush: {collider.gameObject.name}");
            isCollidingPlayer = false;

        }
        
        
    }
}
