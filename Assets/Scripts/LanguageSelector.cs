using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour
{
    public GameObject checkEnglish;  // İngilizce check simgesi
    public GameObject checkTurkish;  // Türkçe check simgesi
    public GameObject checkRussian;  // Rusça check simgesi

    private void Start()
    {
        // En son seçilen dili yükle, yoksa varsayılan dil olarak Türkçe kullan
        string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", "en");
        int localeIndex = GetLocaleIndexByCode(savedLanguage);

        if (localeIndex >= 0)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
        }

        // Başlangıçta doğru "Check" simgesini güncelle
        UpdateCheckmarks();
    }

    public void SetLanguage(int localeIndex)
    {
        // Dil seçimini değiştir
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeIndex];
        
        // Seçilen dili kaydet
        PlayerPrefs.SetString("SelectedLanguage", LocalizationSettings.SelectedLocale.Identifier.Code);
        PlayerPrefs.Save();

        UpdateCheckmarks();
    }

    private void UpdateCheckmarks()
    {
        // Aktif dil kodunu al
        var currentLocale = LocalizationSettings.SelectedLocale.Identifier.Code;

        checkEnglish.SetActive(currentLocale == "en");
        checkTurkish.SetActive(currentLocale == "tr-TR");  // Türkçe dil kodu "tr-TR" olarak kontrol ediliyor
        checkRussian.SetActive(currentLocale == "ru");
    }

    private int GetLocaleIndexByCode(string code)
    {
        // Belirtilen dil koduna göre uygun locale index'ini döndür
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == code)
            {
                return i;
            }
        }

        return -1;  // Eğer dil bulunamazsa -1 döndür
    }
}
