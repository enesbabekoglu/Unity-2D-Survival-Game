using System.Collections;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    public GameObject notificationCanvas; // NotificationCanvas referansı
    public TextMeshProUGUI notificationText; // NotificationText referansı (TextMeshPro)
    public float displayDuration = 2f; // Bildirimin ekranda kalma süresi

    private Coroutine currentNotificationCoroutine;

    void Start()
    {
        // Başlangıçta canvas'ı gizle
        notificationCanvas.SetActive(false);
    }

    // Bildirimi gösterme fonksiyonu
    public void ShowNotification(string message)
    {
        // Önceki bildirim aktifse durdur
        if (currentNotificationCoroutine != null)
        {
            StopCoroutine(currentNotificationCoroutine);
        }

        // Yeni bildirimi başlat
        currentNotificationCoroutine = StartCoroutine(DisplayNotification(message));
    }

    // Bildirimi ekranda göster ve süre sonunda gizle
    private IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message; // Gelen mesajı NotificationText'e yaz
        notificationCanvas.SetActive(true); // Canvas'ı görünür yap

        yield return new WaitForSeconds(displayDuration); // Belirtilen süre kadar bekle

        notificationCanvas.SetActive(false); // Canvas'ı tekrar gizle
        currentNotificationCoroutine = null;
    }
}
