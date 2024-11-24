using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void lightFire(){
        animator.SetBool("burning", true);
    }

    public void putOutFire(){
        animator.SetBool("burning", false);
    }
}
