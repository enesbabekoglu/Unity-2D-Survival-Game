using UnityEngine;

public class FishingTriggerHandler : MonoBehaviour
{
    public FishingSystem fishingSystem; // FishingSystem referansı
    private GameObject currentWaterZone; // Şu anda temas edilen su alanı

    void Update()
    {
        // Eğer su alanında bulunuyorsa ve tıklandıysa balık tutmayı başlat
        if (Input.GetMouseButtonDown(0) && currentWaterZone != null)
        {
            fishingSystem.ShowFishingUI(); // Balık tutma UI'sini göster
        }
    }

    // Su alanına girildiğinde
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water")) // Su alanının Tag'i "Water"
        {
            currentWaterZone = collision.gameObject; // Su alanını kaydet
        }
    }

    // Su alanından çıkıldığında
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentWaterZone)
        {
            currentWaterZone = null; // Su alanını sıfırla
        }
    }
}
