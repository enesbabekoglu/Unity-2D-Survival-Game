using UnityEngine;
using TMPro;

public class HungerHealthSystem : MonoBehaviour
{
    public TextMeshProUGUI HungerCount; // Açlık UI
    public TextMeshProUGUI HealthCount; // Sağlık UI

    private float hungerDecreaseRate; // Saniyede azalan açlık
    private float hungerTimer;

    private void Start()
    {
        // GameManager'ın varlığını kontrol et
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager bulunamadı! HungerHealthSystem çalışamayacak.");
            return;
        }

        // UI bileşenlerini otomatik bul
        if (HungerCount == null)
        {
            HungerCount = GameObject.Find("HungerCount")?.GetComponent<TextMeshProUGUI>();
        }
        if (HealthCount == null)
        {
            HealthCount = GameObject.Find("HealthCount")?.GetComponent<TextMeshProUGUI>();
        }

        // Referansları kontrol et
        if (HungerCount == null || HealthCount == null)
        {
            Debug.LogError("HungerCount veya HealthCount atanmadı! Lütfen UI öğelerini kontrol edin.");
            return;
        }

        // Açlık azaltma oranını hesapla
        if (GameManager.Instance.MaxHungerMinutes <= 0)
        {
            Debug.LogError("MaxHungerMinutes sıfır ya da negatif! Doğru bir değer girin.");
            return;
        }

        hungerDecreaseRate = GameManager.Instance.MaxHunger / (GameManager.Instance.MaxHungerMinutes * 60f);
        hungerTimer = 0f;

        // UI'yı başlangıçta güncelle
        UpdateUI();
    }

    private void Update()
    {
        UpdateHunger();
    }

    private void UpdateHunger()
    {
        hungerTimer += Time.deltaTime;

        if (hungerTimer >= 1f) // Her saniye
        {
            GameManager.Instance.Hunger = Mathf.Clamp(GameManager.Instance.Hunger - hungerDecreaseRate, 0, GameManager.Instance.MaxHunger);
            hungerTimer = 0f; // Zamanlayıcı sıfırla
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance bulunamadı! UI güncellenemiyor.");
            return;
        }

        // Açlık değerini güncelle
        if (HungerCount != null)
        {
            float hunger = Mathf.Round(GameManager.Instance.Hunger * 100f) / 100f; // İki ondalık basamağa yuvarlama
            HungerCount.text = $"%{hunger:0.00}";
        }

        // Sağlık değerini güncelle
        if (HealthCount != null)
        {
            int health = GameManager.Instance.Health;
            int maxHealth = GameManager.Instance.MaxHealth;
            HealthCount.text = $"{health}/{maxHealth}";
        }
    }
}
