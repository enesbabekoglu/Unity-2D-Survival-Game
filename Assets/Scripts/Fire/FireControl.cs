using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    public AudioClip fireSizzleSound;
    public AudioClip cookingSizzleSound;
    private AudioSource fireAudioSource;
    private AudioSource cookingAudioSource;
    public AudioMixerGroup sfxMixerGroup;

    public GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        setUIVisibility(false);

        fireAudioSource = gameObject.AddComponent<AudioSource>();
        fireAudioSource.clip = fireSizzleSound;
        fireAudioSource.loop = true;
        fireAudioSource.spatialBlend = 1.0f;
        fireAudioSource.minDistance = 2f;
        fireAudioSource.maxDistance = 30f;
        fireAudioSource.rolloffMode = AudioRolloffMode.Linear;
        fireAudioSource.outputAudioMixerGroup = sfxMixerGroup;

        cookingAudioSource = gameObject.AddComponent<AudioSource>();
        cookingAudioSource.clip = cookingSizzleSound;
        cookingAudioSource.loop = true;
        cookingAudioSource.spatialBlend = 1.0f;
        cookingAudioSource.minDistance = 2f;
        cookingAudioSource.maxDistance = 20f;
        cookingAudioSource.rolloffMode = AudioRolloffMode.Linear;
        cookingAudioSource.outputAudioMixerGroup = sfxMixerGroup;

        gameManager = GameObject.FindWithTag("gameManager").GetComponent<GameManager>();
        pState = GameObject.FindWithTag("player").GetComponent<PlayerState>();
    }

    void Update()
    {
        if (gameManager.isBurning)
        {
            if (!animator.GetBool("burning"))
            {
                animator.SetBool("burning", true);
            }

            gameManager.fireHealth -= Time.deltaTime;
            gameManager.fireHealth = Mathf.Clamp(gameManager.fireHealth, 0, maxHealth);
            healthBar.value = gameManager.fireHealth;

            if (gameManager.fireHealth == 0.0f)
            {
                putOutFire();
            }
        }
    }

    public void lightFire()
    {
        // **Eğer ateş zaten yanıyorsa tekrar odun atılmasını engelle**
        if (gameManager.isBurning)
        {
            Debug.Log("Ateş zaten yanıyor, tekrar odun eklenemez!");
            return;
        }

        if (pState.inventorySystem.GetItemCount("Wood") >= 3)
        {
            pState.inventorySystem.RemoveItem("Wood", 3);
            gameManager.fireHealth = maxHealth;
            gameManager.isBurning = true;
            animator.SetBool("burning", true);
            fireAudioSource.Play();
        }
        else
        {
            Debug.Log("Yeterli odununuz yok!");
        }
    }

    public void putOutFire()
    {
        gameManager.isBurning = false;
        animator.SetBool("burning", false);
        fireAudioSource.Stop();
        cookingAudioSource.Stop();
    }

    public void StartCookingSound()
    {
        if (!cookingAudioSource.isPlaying)
        {
            cookingAudioSource.Play();
        }
    }

    public void StopCookingSound()
    {
        if (cookingAudioSource.isPlaying)
        {
            cookingAudioSource.Stop();
        }
    }

    public void setUIVisibility(bool flag)
    {
        text.gameObject.SetActive(flag);
        healthBar.gameObject.SetActive(flag);
    }
}
