using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 0.95f; // Küçültme oranı
    public float transitionDuration = 0.1f; // Geçiş süresi (saniye cinsinden)

    private bool isPointerOver = false;
    private float transitionProgress = 0f;

    private Transform objectTransform; // UI veya 2D nesneler için transform

    void Start()
    {
        // UI için RectTransform veya normal Transform seçimi
        objectTransform = GetComponent<RectTransform>() != null ? (Transform)GetComponent<RectTransform>() : transform;

        originalScale = objectTransform.localScale; // Başlangıç boyutunu kaydet
    }

    void Update()
    {
        // Gerçek zamana göre animasyon
        if (isPointerOver)
        {
            transitionProgress += Time.unscaledDeltaTime / transitionDuration;  // `unscaledDeltaTime` kullanılıyor
        }
        else
        {
            transitionProgress -= Time.unscaledDeltaTime / transitionDuration;
        }

        // 0 ile 1 arasında değer tutma ve boyutu yumuşak geçişle ayarlama
        transitionProgress = Mathf.Clamp01(transitionProgress);
        objectTransform.localScale = Vector3.Lerp(originalScale, new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.z), transitionProgress);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true; // Fare üstüne geldiğinde küçültme başlar
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false; // Fare çıktığında boyut geri döner
    }
}
