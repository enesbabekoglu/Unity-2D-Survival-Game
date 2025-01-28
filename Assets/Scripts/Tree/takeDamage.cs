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

    // Ses kaynağı ve efektler
    public AudioSource audioSource;
    public AudioClip damageSound;  // Hasar sesi

    // Start is called before the first frame update
    void Start()
    {
        setUIVisibility(false);
        GameObject player = GameObject.FindWithTag("player");
        pState = player.GetComponent<PlayerState>();

        // Eğer AudioSource bileşeni eklenmemişse, ekleyelim
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, pState.position) < proximityRange)
        {
            playerNear = true;
            text.color = availableColor;
            if (treeState.health > 0.0f)
            {
                setUIVisibility(true);
            }
            else
            {
                setUIVisibility(false);
            }
        }
        else
        {
            playerNear = false;
            text.color = unavailableColor;
            setUIVisibility(false);
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Math.Abs(mousePos.x - transform.position.x) < hoverRadius && Math.Abs(mousePos.y - transform.position.y) < hoverRadius)
        {
            if (treeState.health > 0.0f)
            {
                setUIVisibility(true);
            }
            else
            {
                setUIVisibility(false);
            }
            mouseNear = true;
        }
        else if (!playerNear)
        {
            setUIVisibility(false);
            mouseNear = false;
        }
        else
        {
            mouseNear = false;
        }

        if (playerNear && mouseNear && Input.GetMouseButtonDown(0) && pState.canChop && treeState.isAlive)
        {
            takeDmg();
        }
    }

    void takeDmg()
    {
        treeState.health -= damagePerHit;
        treeState.health = Mathf.Clamp(treeState.health, 0, treeState.maxHealth); 

        if (treeState.health == 0.0f)
        {
            treeState.isAlive = false;
            treeState.inventorySystem.AddItem("Wood", woodYield);
        }
        else
        {
            treeState.animator.SetTrigger("damaged");
            StartCoroutine(PlayDamageSound());  // Ses efekti çalıştır
        }
    }

    private IEnumerator PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);  // İlk vuruş sesi
            yield return new WaitForSeconds(0.2f); // Kısa bir gecikme
            audioSource.PlayOneShot(damageSound);  // İkinci vuruş sesi
        }
    }

    void setUIVisibility(bool flag)
    {
        text.gameObject.SetActive(flag);
        treeState.healthBar.gameObject.SetActive(flag);
    }
}
