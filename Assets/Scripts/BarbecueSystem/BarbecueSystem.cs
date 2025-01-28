using System.Collections;
using TMPro;
using UnityEngine;

public class BarbecueSystem : MonoBehaviour
{
    public PlayerState pState; // Oyuncunun durumlarını kontrol eden sistem
    public GameObject FireBarbecueTimeBox; // UI kutusu (ana obje)
    public TextMeshProUGUI FireBarbecueText; // Kalan süreyi gösteren yazı
    public TextMeshProUGUI FireBarbecueText2; // Pişirme durumu mesajı
    public float barbecueTime = 5f; // Pişirme süresi
    public bool isCooking = false; // Balık pişiriliyor mu kontrolü

    public FireControl fireControl; // Ateş kontrolüne referans
    public GameManager gameManager; // GameManager referansı

    public GameObject barbecueObject; // Tıklanması gereken nesne (Inspector'dan atanmalı)

    void Start()
    {
        setBarbecueUIVisibility(false); // Başlangıçta UI gizli

        fireControl = FindObjectOfType<FireControl>(); // Ateş kontrolünü bul
        if (fireControl == null)
        {
            Debug.LogError("FireControl bulunamadı! Ateş kontrolü eksik.");
        }

        pState = GameObject.FindWithTag("player")?.GetComponent<PlayerState>();
        if (pState == null)
        {
            Debug.LogError("PlayerState bulunamadı! Oyuncu bileşeni eksik.");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager bulunamadı! Sahneye eklendiğinden emin olun.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Fare sol tıklaması
        {
            DetectClickOnBarbecue();
        }
    }

    private void DetectClickOnBarbecue()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (barbecueObject == null)
        {
            Debug.LogError("BarbecueObject atanmadı! Inspector'dan doğru nesneyi seçin.");
            return;
        }

        // **2D fizik kullanılıyorsa**
        Collider2D hitCollider2D = Physics2D.OverlapPoint(mousePosition);

        if (hitCollider2D != null && hitCollider2D.gameObject == barbecueObject)
        {
            TryCookFishOrLightFire();
        }

    }

    public void TryCookFishOrLightFire()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager eksik! İşlem iptal edildi.");
            return;
        }

        if (!gameManager.isBurning)
        {
            Debug.Log("Ateş yanmıyor! Önce ateşi yakın.");
        }
        else if (!isCooking && pState.inventorySystem.GetItemCount("Fish") >= 1)
        {
            pState.inventorySystem.RemoveItem("Fish", 1);
            isCooking = true;
            setBarbecueUIVisibility(true);
            FireBarbecueText2.text = "Pişiyor...";
            fireControl.StartCookingSound();
            StartCoroutine(CookFishCoroutine());
        }
        else if (pState.inventorySystem.GetItemCount("Fish") < 1)
        {
            Debug.Log("Envanterde yeterli miktarda balık yok.");
        }
    }

    private IEnumerator CookFishCoroutine()
    {
        float timeLeft = barbecueTime;
        Debug.Log("Balık pişirme başladı.");

        while (timeLeft > 0)
        {
            FireBarbecueText.text = $"{Mathf.CeilToInt(timeLeft)}s"; // Kalan süreyi güncelle
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        fireControl.StopCookingSound();
        FireBarbecueText.text = "";
        FireBarbecueText2.text = "Pişti!";
        Debug.Log("Balık pişti!");

        yield return new WaitForSeconds(1);

        pState.inventorySystem.AddItem("Grilled_Fish", 1);
        isCooking = false;
        setBarbecueUIVisibility(false);
    }

    public void setBarbecueUIVisibility(bool flag)
    {
        if (FireBarbecueTimeBox != null)
        {
            FireBarbecueTimeBox.SetActive(flag);
        }
        else
        {
            Debug.LogWarning("FireBarbecueTimeBox referansı eksik!");
        }
    }
}
