using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEventBinder : MonoBehaviour
{
    public static ButtonEventBinder Instance;

    private void Awake()
    {
        Debug.Log("ButtonEventBinder Awake çağrıldı.");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("ButtonEventBinder Singleton oluşturuldu ve sahne yüklendiğinde dinleniyor.");
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Zaten bir ButtonEventBinder var. Yeni nesne yok edildi.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Sahne yüklendi: {scene.name}");
        BindAllButtons();
        BindEventTriggers();
    }

    private void BindAllButtons()
    {
        Debug.Log("BindAllButtons çağrıldı. Tüm butonlar aranıyor...");
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>(); // Aktif ve pasif butonları bulur

        foreach (Button button in buttons)
        {
            if (button == null || !button.gameObject.activeInHierarchy) continue;

            Debug.Log($"Buton bulundu: {button.name}");

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() =>
            {
                CentralizedButtonSound.Instance?.PlayClickSound();
                Debug.Log($"Butona tıklama sesi oynatıldı: {button.name}");
            });

            // Belirttiğiniz sahne geçiş eventleri
            switch (button.name)
            {
                case "MenuPlayButton":
                    button.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
                    Debug.Log("MenuPlayButton için sahne geçiş eventi bağlandı.");
                    break;

                case "MenuHowCanPlayButton":
                    button.onClick.AddListener(() => SceneManager.LoadScene("HowCanPlayScene"));
                    Debug.Log("MenuHowCanPlayButton için sahne geçiş eventi bağlandı.");
                    break;

                case "BackToMainFromHCPButton":
                    button.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
                    Debug.Log("BackToMainFromHCPButton için sahne geçiş eventi bağlandı.");
                    break;

                default:
                    Debug.Log($"Buton '{button.name}' için özel bir işlem tanımlanmadı.");
                    break;
            }
        }
    }

    private void BindEventTriggers()
    {
        Debug.Log("BindEventTriggers çağrıldı. Hover sesleri için triggerlar bağlanıyor...");
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            if (button == null) continue;

            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
                Debug.Log($"EventTrigger eklendi: {button.name}");
            }

            // Hover için Event Trigger
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEnter.callback.AddListener((eventData) =>
            {
                CentralizedButtonSound.Instance?.PlayHoverSound();
                Debug.Log($"Hover sesi oynatıldı: {button.name}");
            });

            trigger.triggers.Clear();
            trigger.triggers.Add(pointerEnter);
        }
    }
}
