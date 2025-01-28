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
    private Collider2D fireCollider;

    void Start()
    {
        fireCollider = GetComponent<Collider2D>();
        if (fireCollider == null)
        {
            Debug.LogError("Collider2D bulunamadı! FireInteraction doğru bir objeye eklenmemiş.");
        }
    }

    void Update()
    {
        // Oyuncu yakın mı?
        if (Vector3.Distance(transform.position, fireControl.pState.position) < fireControl.proximityRange)
        {
            fireControl.playerNear = true;
            fireControl.text.color = availableColor;
            fireControl.setUIVisibility(fireControl.health > 0.0f);
        }
        else
        {
            fireControl.playerNear = false;
            fireControl.text.color = unavailableColor;
            fireControl.setUIVisibility(false);
        }

        // Fare ateşin üstünde mi?
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (fireCollider != null && fireCollider.OverlapPoint(mousePos))
        {
            fireControl.setUIVisibility(fireControl.health > 0.0f);
            mouseNear = true;
        }
        else if (!fireControl.playerNear)
        {
            fireControl.setUIVisibility(false);
            mouseNear = false;
        }

        // **Ateşi yakma işlemi (sadece eğer yanmıyorsa çalışır)**
        if (fireControl.playerNear && mouseNear && Input.GetMouseButtonDown(0) && fireControl.pState.canFire)
        {
            if (!fireControl.isBurning) 
            {
                fireControl.lightFire();
            }
            else
            {
                Debug.Log("Ateş zaten yanıyor, tekrar odun eklenemez!");
            }
        }
    }
}
