using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;  // Audio Mixer için gerekli

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

    // Ses kaynakları ve mixer
    public AudioClip fireSizzleSound;  // Ateş sesi
    public AudioClip cookingSizzleSound;  // Pişirme sesi
    private AudioSource fireAudioSource;
    private AudioSource cookingAudioSource;

    public AudioMixerGroup sfxMixerGroup;  // Merkezi ses kontrol için mixer group

    void Start()
    {
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        setUIVisibility(false);

        // Ateş ses kaynağı
        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.clip = fireSizzleSound;
        fireAudioSource.loop = true;  // Ateş sesi döngüde çalsın
        fireAudioSource.spatialBlend = 1.0f;  // 3D ses efekti
        fireAudioSource.minDistance = 2f;  // Tam ses duyulmaya başlanacak mesafe
        fireAudioSource.maxDistance = 30f;  // Bu mesafeden sonra ses tamamen kaybolur
        fireAudioSource.rolloffMode = AudioRolloffMode.Linear;  // Mesafeye göre doğrusal ses düşüşü
        fireAudioSource.outputAudioMixerGroup = sfxMixerGroup;  // Mixer group bağlantısı

        // Pişirme ses kaynağı
        cookingAudioSource = gameObject.AddComponent<AudioSource>();
        cookingAudioSource.clip = cookingSizzleSound;
        cookingAudioSource.loop = true;  // Pişirme sesi döngüde çalsın
        cookingAudioSource.spatialBlend = 1.0f;  // 3D ses efekti
        cookingAudioSource.minDistance = 2f;  // Yakın mesafede tam ses
        cookingAudioSource.maxDistance = 20f;  // Bu mesafeden sonra ses tamamen kaybolur
        cookingAudioSource.rolloffMode = AudioRolloffMode.Linear;  // Doğrusal azalma
        cookingAudioSource.outputAudioMixerGroup = sfxMixerGroup;  // Mixer group bağlantısı
    }

    void Update()
    {
        if (isBurning)
        {
            health -= Time.deltaTime;
            health = Mathf.Clamp(health, 0, maxHealth);
            healthBar.value = health;
            if (health == 0.0f)
            {
                putOutFire();
            }
        }
    }

    public void lightFire()
    {
        pState.inventorySystem.RemoveItem("Wood", 3);
        health = maxHealth;
        isBurning = true;
        animator.SetBool("burning", true);
        fireAudioSource.Play();  // Ateş sesini başlat
    }

    public void putOutFire()
    {
        isBurning = false;
        animator.SetBool("burning", false);
        fireAudioSource.Stop();  // Ateş sesi durdur
        cookingAudioSource.Stop();  // Pişirme sesi durdur
    }

    public void StartCookingSound()
    {
        if (!cookingAudioSource.isPlaying)
        {
            cookingAudioSource.Play();  // Pişirme sesini başlat
        }
    }

    public void StopCookingSound()
    {
        if (cookingAudioSource.isPlaying)
        {
            cookingAudioSource.Stop();  // Pişirme sesini durdur
        }
    }

    public void setUIVisibility(bool flag)
    {
        text.gameObject.SetActive(flag);
        healthBar.gameObject.SetActive(flag);
    }
}
