using UnityEngine;
using UnityEngine.Audio;

public class CentralizedButtonSound : MonoBehaviour
{
    public static CentralizedButtonSound Instance; // Singleton referansı
    private AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip buttonHoverSound;      // Hover sesi
    public AudioClip buttonClickSound;      // Click sesi

    [Header("Audio Mixer")]
    public AudioMixerGroup masterMixerGroup; // Master volume kontrolü

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = masterMixerGroup;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayHoverSound()
    {
        if (buttonHoverSound != null)
        {
            audioSource.PlayOneShot(buttonHoverSound);
        }
        else
        {
            Debug.LogWarning("Hover sesi atanmadı!");
        }
    }

    public void PlayClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("Click sesi atanmadı!");
        }
    }
}
