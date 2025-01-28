using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damageOnCollision : MonoBehaviour
{
    public float DPS;

    private bool isCollidingPlayer = false;

    public HungerHealthSystem Instance;
    public PlayerState pState;
    private float baseSpeed;

    // Ses ve hasar yazısı için değişkenler
    public AudioSource audioSource;
    public AudioClip damageSound;
    public GameObject damageTextPrefab;
    public Transform damageTextParent;

    private int previousHealth; // Önceki tam sayı can değeri
    private static float lastSoundTime = 0f; // En son sesin çalındığı zaman
    private float soundCooldown = 1f; // Ses çalma bekleme süresi (1 saniye)

    void Start()
    {
        Vector3 tempPos = transform.position;
        tempPos.z = tempPos.y;
        transform.position = tempPos;
        baseSpeed = pState.baseSpeed;

        if (Instance == null)
        {
            Instance = FindObjectOfType<HungerHealthSystem>();
            if (Instance == null)
            {
                Debug.LogError("Sahnede HungerHealthSystem bileşeni bulunamadı!");
            }
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Oyunun başında tam sayı can değerini kaydediyoruz
        previousHealth = Mathf.FloorToInt(GameManager.Instance.Health);
        
        GameObject player = GameObject.FindWithTag("player");
        pState = player.GetComponent<PlayerState>();
        damageTextParent = player.transform;
        Instance = GameObject.FindWithTag("hungerHealthMenu").GetComponent<HungerHealthSystem>();

    }

    void Update()
    {
        if (Instance == null)
        {
            return;
        }

        if (isCollidingPlayer)
        {
            float damage = DPS * Time.deltaTime * -1;
            GameManager.Instance.Health += damage;

            int currentHealth = Mathf.FloorToInt(GameManager.Instance.Health); // Tam sayı kısmı

            if (currentHealth < previousHealth) // Eğer can değeri düştüyse
            {
                // Ses sadece her 1 saniyede bir çalınır
                if (Time.time - lastSoundTime >= soundCooldown)
                {
                    PlayDamageSound();
                    lastSoundTime = Time.time; // En son ses çalınma zamanını güncelle
                }

                // Her `Bush` için yazı oluştur
                CreateDamageText("-1");

                previousHealth = currentHealth; // Yeni tam sayı can değerini kaydediyoruz
            }

            Instance.UpdateHealth(0); // UI güncellemesi
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("player"))
        {
            isCollidingPlayer = true;
            pState.baseSpeed = baseSpeed / 2.0f;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("player"))
        {
            isCollidingPlayer = false;
            pState.baseSpeed = baseSpeed;
            audioSource.Stop();
        }
    }

    private void CreateDamageText(string damageAmount)
    {
        if (damageTextPrefab != null && damageTextParent != null)
        {
            GameObject damageTextInstance = Instantiate(damageTextPrefab, damageTextParent);
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
            damageTextInstance.transform.position = transform.position + randomOffset;

            TextMeshPro textMesh = damageTextInstance.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = damageAmount;
            }
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }
}
