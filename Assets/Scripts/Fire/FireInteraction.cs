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

    private Collider2D fireCollider; // Ateşin Collider referansı

    // Start is called before the first frame update
    void Start()
    {
        fireCollider = GetComponent<Collider2D>(); // Ateş objesindeki Collider2D bileşeni
        if (fireCollider == null)
        {
            Debug.LogError("Collider2D bulunamadı! FireInteraction doğru bir objeye eklenmemiş.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Oyuncu yakınlık kontrolü
        if (Vector3.Distance(transform.position, fireControl.pState.position) < fireControl.proximityRange)
        {
            fireControl.playerNear = true;
            fireControl.text.color = availableColor;
            if (fireControl.health > 0.0f)
            {
                fireControl.setUIVisibility(true);
            }
            else
            {
                fireControl.setUIVisibility(false);
            }
        }
        else
        {
            fireControl.playerNear = false;
            fireControl.text.color = unavailableColor;
            fireControl.setUIVisibility(false);
        }

        // Fare konumunun Fire Collider içinde olup olmadığını kontrol et
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (fireCollider != null && fireCollider.OverlapPoint(mousePos))
        {
            if (fireControl.health > 0.0f)
            {
                fireControl.setUIVisibility(true);
            }
            else
            {
                fireControl.setUIVisibility(false);
            }
            mouseNear = true;
        }
        else if (!fireControl.playerNear)
        {
            fireControl.setUIVisibility(false);
            mouseNear = false;
        }

        // Ateşi yakma işlemi (sadece fare collider içine tıklarsa)
        if (fireControl.playerNear && mouseNear && Input.GetMouseButtonDown(0) && fireControl.pState.canFire && !fireControl.isBurning)
        {
            fireControl.lightFire();
        }
    }

    // İsteğe bağlı olarak diğer tetikleyiciler
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Oyuncu Fire Collider içine girdiğinde başka bir tetikleme gerekiyorsa buraya ekleyebilirsiniz
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // Oyuncu Fire Collider'dan çıktığında işlem gerekiyorsa buraya ekleyebilirsiniz
    }
}
