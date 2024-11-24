using UnityEngine;
using System.Collections;

public class FishingTriggerHandler : MonoBehaviour
{

    public Notification notification; // Notification script referansı
    public FishingSystem fishingSystem; // FishingSystem referansı
    private GameObject currentWaterZone; // Şu anda temas edilen su alanı
    public GameObject player; // Player GameObject referansı (PlayerState'e ulaşmak için)
    private bool isFishing; // Balık tutma işleminin aktif olup olmadığını kontrol eder
    public Sprite fishingRodActiveSprite; // Balık tutma işlemi sırasında kullanılacak olta sprite'ı
    private Sprite defaultSprite; // Oyuncunun elindeki default sprite
    private SpriteRenderer playerSpriteRenderer; // Oyuncunun SpriteRenderer referansı

    void Start()
    {
        // Oyuncunun SpriteRenderer bileşenine erişim
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            defaultSprite = playerSpriteRenderer.sprite; // Oyuncunun mevcut sprite'ını kaydet
        }
        else
        {
            Debug.LogError("Player'da SpriteRenderer bileşeni bulunamadı!");
        }
    }

    void Update()
    {
        // Eğer su alanında bulunuyorsa ve bir alana tıklanıyorsa kontrol yap
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse sol tuşuna tıklandı.");

            if (currentWaterZone != null) // Eğer su alanında bulunuyorsak
            {
                Debug.Log("Su alanında bulunuyorsunuz.");

                // Tıklama yapıldığında raycast ile kontrol
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int layerMask = LayerMask.GetMask("WaterFishingArea"); // Sadece WaterFishingArea katmanındaki objeler
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);

                if (hit.collider != null)
                {
                    Debug.Log($"WaterFishingArea alanına tıkladınız. Nesne adı: {hit.collider.gameObject.name}");

                    // PlayerState script'ine erişim
                    PlayerState playerState = player.GetComponent<PlayerState>();

                    if (playerState != null)
                    {
                        Debug.Log($"PlayerState alındı. Elinizdeki eşya: {playerState.ItemOnHand}");
                        if (playerState.ItemOnHand == "FishingRod")
                        {
                            if (!isFishing)
                            {
                                StartCoroutine(StartFishingCoroutine());
                            }
                            else
                            {
                                notification.ShowNotification("Balık tutma işlemi zaten devam ediyor.");
                            }
                        }
                        else
                        {
                            Debug.Log("Elinizde olta bulunmuyor.");
                        }
                    }
                    else
                    {
                        Debug.LogError("PlayerState bileşenine ulaşılamadı.");
                    }
                }
                else
                {
                    Debug.Log("Tıklama herhangi bir WaterFishingArea nesnesine yapılmadı.");
                }
            }
            else
            {
                Debug.Log("Su alanında değilsiniz.");
            }
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

    // Coroutine: Her 5 saniyede bir %50 ihtimalle balık ekranını açar
    private IEnumerator StartFishingCoroutine()
    {
        isFishing = true;
        notification.ShowNotification("Balık tutma işlemi başladı. Bekleniyor...");

        // Olta sprite'ını değiştir
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = fishingRodActiveSprite;
        }

        yield return new WaitForSeconds(5f); // 5 saniye bekle

        if (Random.value < 0.5f) // %50 ihtimalle balık ekranı açılır
        {
            fishingSystem.ShowFishingUI(); // Balık tutma UI'sini göster
        }
        else
        {
            notification.ShowNotification("Bu sefer balık yok!");
        }

        // Sprite'ı eski haline döndür
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = defaultSprite;
        }

        isFishing = false; // Balık tutma işlemi tamamlandı
    }
}
