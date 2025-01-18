using System.Collections;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float lifetime = 3f;  // Yazının ekranda kalma süresi
    public float fadeDuration = 3f;  // Şeffaflaşma süresi
    public Vector3 moveOffset = new Vector3(0, 1, 0);  // Yazının hareket yönü
    public Vector3 initialScale = new Vector3(1.5f, 1.5f, 1.5f);  // İlk zoom ölçeği
    public Vector3 finalScale = new Vector3(1f, 1f, 1f);  // Nihai ölçek

    private TextMeshPro textMesh;  // Text bileşeni
    private Color textColor;

    void Start()
    {
        // Alt objelerdeki `TextMeshPro` bileşenini al
        textMesh = GetComponentInChildren<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro bileşeni bulunamadı. Lütfen prefab'e `TextMeshPro - Text` bileşenini ekleyin.");
            return;
        }

        textColor = textMesh.color;

        // Başlangıçta büyütme efekti
        transform.localScale = initialScale;
        StartCoroutine(ScaleDown());

        // Zamanla kaybolma işlemini başlat
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator ScaleDown()
    {
        float elapsed = 0f;
        while (elapsed < 0.3f)  // Zoom süresi
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;  // Nihai ölçek
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            // Yavaşça yukarı doğru hareket
            transform.position += moveOffset * Time.deltaTime;

            // Şeffaflık azaltma
            textColor.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            textMesh.color = textColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);  // Yazıyı yok et
    }
}
