using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSoundManager : MonoBehaviour
{
    public Transform player;  // Oyuncunun Transform bileşeni
    public AudioSource waterAudioSource;  // Su sesi kaynağı

    public float minDistance = 5f;  // Maksimum sesin duyulacağı mesafe
    public float maxDistance = 30f; // Sesin tamamen kesileceği mesafe

    void Start()
    {
        if (waterAudioSource == null)
        {
            waterAudioSource = GetComponent<AudioSource>();  // AudioSource'u al
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("player").transform;  // Oyuncuyu bul
        }
    }

    void Update()
    {
        if (player != null && waterAudioSource != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            float volume = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));  
            waterAudioSource.volume = volume;
        }
    }
}
