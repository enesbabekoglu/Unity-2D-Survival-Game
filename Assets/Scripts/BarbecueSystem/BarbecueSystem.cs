using System.Collections;
using TMPro;
using UnityEngine;

public class BarbecueSystem : MonoBehaviour
{
    public PlayerState pState;  // Oyuncunun durumlarını kontrol eden sistem
    public GameObject FireBarbecueTimeBox;  // UI kutusu (ana obje)
    public TextMeshProUGUI FireBarbecueText;  // Kalan süreyi gösteren yazı
    public TextMeshProUGUI FireBarbecueText2;  // Pişirme durumu mesajı
    public float barbecueTime = 5f;  // Pişirme süresi
    public bool isCooking = false;  // Balık pişiriliyor mu kontrolü

    private FireControl fireControl;  // Ateş kontrolüne referans

    void Start()
    {
        setBarbecueUIVisibility(false);  // Başlangıçta UI gizli
        fireControl = FindObjectOfType<FireControl>();  // Ateş kontrolüne referans bulma
        if (fireControl == null)
        {
            Debug.LogError("FireControl bulunamadı. Ateş kontrolüne erişim başarısız!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryCookFishOrLightFire();
        }
    }

    public void TryCookFishOrLightFire()
    {
        if (fireControl != null)
        {
            if (fireControl.isBurning && !isCooking)
            {
                if (pState.inventorySystem.GetItemCount("Fish") >= 1)
                {
                    pState.inventorySystem.RemoveItem("Fish", 1);
                    isCooking = true;
                    setBarbecueUIVisibility(true);
                    FireBarbecueText2.text = "Pişiyor...";
                    fireControl.StartCookingSound();  // Pişirme sesi başlat
                    StartCoroutine(CookFishCoroutine());
                }
                else
                {
                    Debug.Log("Envanterde yeterli miktarda balık yok.");
                }
            }
        }
        else
        {
            Debug.LogError("FireControl nesnesine erişim başarısız!");
        }
    }

    private IEnumerator CookFishCoroutine()
    {
        float timeLeft = barbecueTime;
        Debug.Log("Balık pişirme işlemi başladı.");

        while (timeLeft > 0)
        {
            FireBarbecueText.text = $"{Mathf.CeilToInt(timeLeft)}s";  // Kalan süreyi tam sayı olarak göster
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        fireControl.StopCookingSound();  // Pişirme sesi durdur
        FireBarbecueText.text = "";  // Süreyi temizle
        FireBarbecueText2.text = "Pişti!";  // Pişirme tamamlandığında mesaj
        Debug.Log("Balık pişti!");
        yield return new WaitForSeconds(1);

        pState.inventorySystem.AddItem("Grilled_Fish", 1);  // Pişmiş balığı envantere ekle
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
            Debug.LogWarning("FireBarbecueTimeBox referansı atanmamış.");
        }
    }
}
