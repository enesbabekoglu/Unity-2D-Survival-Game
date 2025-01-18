using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScaleAnimation : MonoBehaviour
{
    public Vector3 startScale = new Vector3(1f, 1f, 1f);  // Başlangıç boyutu (Z ekseni 1 olarak kalır)
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1f);  // Hedef boyut
    public float duration = 1f;  // Büyüme-küçülme süresi (saniye)

    private Transform objectTransform;  // 2D nesne ve UI için ortak transform
    private Coroutine scaleCoroutine;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        objectTransform = GetComponent<RectTransform>() != null ? (Transform)GetComponent<RectTransform>() : transform;

        if (objectTransform != null)
        {
            StartScaleAnimation();  // Objeyi açınca animasyonu başlat
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StopScaleAnimation();  // Objeyi kapatınca coroutine'i durdur
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene" && objectTransform != null)
        {
            Debug.Log("GameScene yüklendi. Scale animasyonu başlatılıyor...");
            StartScaleAnimation();  // Sahne yüklendiğinde animasyonu başlat
        }
    }

    private void StartScaleAnimation()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);  // Varsa eski coroutine'i durdur
        }
        scaleCoroutine = StartCoroutine(ScaleLoop());
    }

    private void StopScaleAnimation()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);  // Animasyonu durdur
            scaleCoroutine = null;
        }
    }

    private IEnumerator ScaleLoop()
    {
        while (true)
        {
            // Büyüme
            yield return StartCoroutine(ScaleTo(targetScale));
            // Küçülme
            yield return StartCoroutine(ScaleTo(startScale));
        }
    }

    private IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 initialScale = objectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            objectTransform.localScale = Vector3.Lerp(initialScale, target, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;  // Time.timeScale = 0 olsa bile çalışır
            yield return null;
        }

        objectTransform.localScale = target;  // Son değer
    }
}
