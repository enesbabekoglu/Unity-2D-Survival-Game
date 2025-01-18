using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Pause menü paneli (Canvas içindeki panel)
    public Button continueButton; // Devam et butonu
    public Button backButton; // Menüye dön butonu
    public Button closeButton; // Çıkış çarpı butonu

    private bool isGamePaused = false;
    private float lastEscapePressTime = 0f; // Son Escape basış zamanı
    private float debounceTime = 0.3f; // Tuş basımı arasındaki minimum süre

    void Start()
    {
        // Başlangıçta paneli kapalı yap
        pauseMenuUI.SetActive(false);

        // Butonlara event ekleme
        continueButton.onClick.AddListener(ResumeGame);
        backButton.onClick.AddListener(GoToMainMenu);
        closeButton.onClick.AddListener(ResumeGame); // Çarpı da devam işlevi görsün
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.time - lastEscapePressTime > debounceTime)
        {
            lastEscapePressTime = Time.time; // Son basış zamanını güncelle

            if (isGamePaused)
            {
                ResumeGame(); // Oyun durdurulmuşsa devam ettir
            }
            else
            {
                lastEscapePressTime = 0f;
                PauseGame(); // Oyun devam ediyorsa durdur
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Menü aç
        Time.timeScale = 0f; // Oyunu durdur
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Menü kapat
        Time.timeScale = 1f; // Oyunu devam ettir
        isGamePaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu normale döndür
        SceneManager.LoadScene("MainScene"); // Ana menü sahnesini yükle
    }
}