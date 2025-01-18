using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    // Ses değişkenleri
    public AudioClip hoverSound;           // Butona gelindiğinde çalacak ses
    public AudioClip clickSound;           // Butona tıklandığında çalacak ses

    private AudioSource audioSource;       // Ses çalacak AudioSource

    void Start()
    {
        // AudioSource bileşeni ekle
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Oyunun başında ses çalmasın
    }

    // Mouse butonun üzerine geldiğinde çalışır
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && !audioSource.isPlaying) // Hover sesi
        {
            audioSource.clip = hoverSound;
            audioSource.Play();
        }
    }

    // Mouse butona tıkladığında çalışır
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null) // Tıklama sesi
        {
            audioSource.clip = clickSound;
            audioSource.Play();
        }
    }
}
