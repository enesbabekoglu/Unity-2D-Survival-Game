using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireControl : MonoBehaviour
{
    public PlayerState pState;

    public TextMeshProUGUI text;
    public float hoverRadius = 1.0f;

    public float proximityRange;

    public bool playerNear = false;

    public Animator animator;

    public Slider healthBar;
    public float maxHealth;

    public float health;

    public bool isBurning = false;

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
        if(isBurning){
            health -= Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth); 
            healthBar.value = health;
            if(health == 0.0f){
                putOutFire();
            }
        }
    }


    public void lightFire(){
        pState.inventorySystem.RemoveItem("Wood", 3);
        health = maxHealth;
        isBurning = true;

        animator.SetBool("burning", true);
    }

    public void putOutFire(){
        isBurning = false;
        animator.SetBool("burning", false);
    }

    public void setUIVisibility(bool flag){
            text.gameObject.SetActive(flag);
            healthBar.gameObject.SetActive(flag);
    }

}
