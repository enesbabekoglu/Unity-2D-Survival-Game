using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageOnCollision : MonoBehaviour
{
    public float DPS;

    private bool isCollidingPlayer = false;


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
        if (HungerHealthSystem.Instance == null)
        {
            Debug.LogError("HungerHealthSystem.Instance bulunamadı. Sağlık güncellemesi yapılamıyor.");
            return;
        }

        if (isCollidingPlayer)
        {
            float damage = DPS * Time.deltaTime * -1; // Negatif değer sağlığı azaltır
            Debug.Log($"Damage given: {damage}");
            HungerHealthSystem.Instance.UpdateHealth(damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("player"))
        {
            Debug.Log($"Collision with bush: {collider.gameObject.name}");
            isCollidingPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("player"))
        {
            Debug.Log($"No more collision with bush: {collider.gameObject.name}");
            isCollidingPlayer = false;
        }
    }
}
