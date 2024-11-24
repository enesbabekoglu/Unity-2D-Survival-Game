using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public float speedCharacter = 10.0f;
    public float touchDistance = 1.0f;              // Ray length
    public LayerMask collisionLayer;
    public GameObject camera;

    public PlayerState pState;
    public float radius = 5f;  // Detection radius
    public SpriteRenderer spriteRenderer;

    public Animator animator; // Reference to the Animator component



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("hi");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posTemp = transform.position;
        posTemp.y -= 0.5f;

        RaycastHit2D hitUp = Physics2D.Raycast(posTemp, Vector2.up, touchDistance, collisionLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(posTemp, Vector2.down, touchDistance, collisionLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(posTemp, Vector2.right, touchDistance, collisionLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(posTemp, Vector2.left, touchDistance, collisionLayer);



        float horizontalMove = 0.0f;
        float verticalMove = 0.0f;  

        if(Input.GetKey(KeyCode.A) && !hitRight){
            horizontalMove -= 1.0f;
        }

        if(Input.GetKey(KeyCode.D) && !hitLeft){
            horizontalMove += 1.0f;
        }

        if(Input.GetKey(KeyCode.W)&& !hitUp){
            verticalMove += 1.0f;
        }

        if(Input.GetKey(KeyCode.S)&& !hitDown){
            verticalMove -= 1.0f;
        }

        if(horizontalMove != 0.0f || verticalMove != 0.0f){
            animator.SetBool("running", true);
        }else{
            animator.SetBool("running", false);
        
        }

        if(horizontalMove < 0){
            spriteRenderer.flipX = true;
            pState.isFlip = true;
        }else if(horizontalMove > 0){
            spriteRenderer.flipX = false;
            pState.isFlip = false;
        }
        

        transform.Translate(new Vector3(horizontalMove * speedCharacter * Time.deltaTime, verticalMove * speedCharacter * Time.deltaTime, 0.0f));
        camera.transform.Translate(new Vector3(horizontalMove * speedCharacter * Time.deltaTime, verticalMove * speedCharacter * Time.deltaTime, 0.0f));

     
    }
}
