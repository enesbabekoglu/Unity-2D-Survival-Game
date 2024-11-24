using UnityEngine;
using TMPro;

public class FishingSystem : MonoBehaviour
{

    public Notification notification; // Notification script referansı
    public GameObject fishingUI; // FishingUI paneli.
    public RectTransform fishingBar; // FishingBar elementi.
    public RectTransform fishingBarFish; // FishingBarFish nesnesi.
    public TextMeshProUGUI fishingPanelTimeText; // Kalan süreyi gösterecek TextMeshPro objesi.
    public float moveAmount = 50f; // Her tuşa basıldığında balığın hareket edeceği piksel miktarı.
    public float speedPerSecond = 350f; // Balığın her saniyede gideceği piksel miktarı.
    public float fishingDuration = 5f; // Balık tutma süresi (saniye).

    private float fishPosition = 0f; // Balığın pozisyonu (piksel olarak).
    private bool movingRight = true; // Otomatik hareket yönü.
    private float remainingTime; // Kalan süre.
    private bool isFishingActive = false; // Balık tutma işleminin aktif olup olmadığını kontrol eder.
    private bool inputEnabled = true; // Oyuncu girişinin aktif olup olmadığını kontrol eder.
    public InventorySystem inventorySystem; // Envanter sistemi referansı

    void Update()
    {
        if (fishingUI != null && fishingUI.activeSelf && isFishingActive)
        {
            HandleAutoMovement(); // Balığın otomatik hareketi.
            if (inputEnabled) HandlePlayerInput(); // Oyuncu müdahalesi.
            UpdateFishingBarFishPosition(); // Balığın pozisyonunu güncelle.
            UpdateFishingTimer(); // Zamanlayıcıyı güncelle.
        }
    }

    public void ShowFishingUI()
    {
        if (fishingUI != null)
        {
            fishingUI.SetActive(true); // FishingUI nesnesini aktif hale getirir.
            ResetFishingBarFishPosition(); // FishingBarFish pozisyonunu sıfırla.
            remainingTime = fishingDuration; // Süreyi başlat.
            isFishingActive = true; // Balık tutma işlemini aktif yap.
            inputEnabled = true; // Oyuncu girişini aktif yap.
        }
    }

    public void HideFishingUI()
    {
        if (fishingUI != null)
        {
            fishingUI.SetActive(false); // FishingUI nesnesini devre dışı bırakır.
            isFishingActive = false; // Balık tutma işlemini durdur.
        }
    }

    private void HandleAutoMovement()
    {
        if (!isFishingActive) return;

        // FishingBar'ın genişliğini al.
        float barWidth = fishingBar.rect.width;
        float fishWidth = fishingBarFish.rect.width;

        // Balığın hızı: Her frame'de hız × geçen süre kadar hareket.
        float movement = speedPerSecond * Time.deltaTime;

        if (movingRight)
        {
            fishPosition += movement;
            if (fishPosition >= barWidth - fishWidth) // Sağ sınır.
            {
                fishPosition = barWidth - fishWidth; // Sağ sınırda dur.
                movingRight = false; // Yönü değiştir.
            }
        }
        else
        {
            fishPosition -= movement;
            if (fishPosition <= 0) // Sol sınır.
            {
                fishPosition = 0; // Sol sınırda dur.
                movingRight = true; // Yönü değiştir.
            }
        }
    }

    private void HandlePlayerInput()
    {
        // FishingBar'ın genişliğini al.
        float barWidth = fishingBar.rect.width;
        float fishWidth = fishingBarFish.rect.width;

        // Sağ tuşa basıldığında balığı sağa çek.
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            fishPosition = Mathf.Clamp(fishPosition + moveAmount, 0, barWidth - fishWidth);
        }

        // Sol tuşa basıldığında balığı sola çek.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            fishPosition = Mathf.Clamp(fishPosition - moveAmount, 0, barWidth - fishWidth);
        }
    }

    private void UpdateFishingBarFishPosition()
    {
        if (fishingBar == null || fishingBarFish == null)
            return;

        // Balığın pozisyonunu güncelle.
        fishingBarFish.localPosition = new Vector3(
            -fishingBar.rect.width / 2 + fishPosition, // Sol kenardan itibaren pozisyon.
            fishingBarFish.localPosition.y,
            fishingBarFish.localPosition.z
        );
    }

    private void ResetFishingBarFishPosition()
    {
        // Balığı ortada başlat.
        fishPosition = fishingBar.rect.width / 2; // Ortadan başlat.
        movingRight = true; // Sağa doğru başlat.
        UpdateFishingBarFishPosition(); // Pozisyonu güncelle.
    }

    private void UpdateFishingTimer()
    {
        remainingTime -= Time.deltaTime;

        if (fishingPanelTimeText != null)
        {
            fishingPanelTimeText.text = Mathf.Ceil(remainingTime).ToString();
        }

        if (remainingTime <= 0)
        {
            EndFishing();
        }
    }

    private void EndFishing()
    {
        isFishingActive = false; // Balık tutma işlemini durdur.
        inputEnabled = false; // Oyuncu girişini devre dışı bırak.

        if (IsFishInGreenZone())
        {
            if (inventorySystem != null)
            {
                notification.ShowNotification("Balığı tuttunuz!");
                inventorySystem.AddItem("Fish", 1); // Balığı envantere ekle.
            }
            else
            {
                Debug.Log("Balık envantere eklenemedi");
            }
        }
        else
        {
            notification.ShowNotification("Balık kaçtı!");
        }

        HideFishingUI(); // Balık tutma UI'sini gizle.
    }

    private bool IsFishInGreenZone()
    {
        // FishingBar'ın genişliğini al.
        float barWidth = fishingBar.rect.width;
        float greenZoneStart = barWidth * 0.25f; // Yeşil alan başlangıcı.
        float greenZoneEnd = barWidth * 0.75f; // Yeşil alan bitişi.

        // Balığın merkezi.
        float fishCenter = fishPosition + fishingBarFish.rect.width / 2;

        return fishCenter >= greenZoneStart && fishCenter <= greenZoneEnd;
    }

    void OnApplicationQuit()
    {
        if (isFishingActive)
        {
            notification.ShowNotification("Balık tutma iptal edildi.");
        }
    }
}
