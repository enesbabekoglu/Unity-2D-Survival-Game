using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer;
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private AudioSource backgroundMusicSource;  // Müzik kaynağı

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Sahne yüklendiğinde çalışacak
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Oyunu açarken kaydedilmiş değerleri uygula
        backgroundMusicSource = GameObject.FindWithTag("BackgroundMusic")?.GetComponent<AudioSource>();
        if (backgroundMusicSource == null)
        {
            Debug.LogError("Background music AudioSource bulunamadı!");
        }

        LoadAudioSettings();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Yeni sahnede slider'ları bul ve bağla
        FindAndBindSliders();
    }

    void Start()
    {
        FindAndBindSliders();
    }

    private void FindAndBindSliders()
    {
        // Hierarchy'deki slider'ları bulmaya çalış
        masterVolumeSlider = GameObject.FindWithTag("MasterVolumeSlider")?.GetComponent<Slider>();
        musicVolumeSlider = GameObject.FindWithTag("MusicVolumeSlider")?.GetComponent<Slider>();

        // Slider'ları UI üzerinde güncelle
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);  // Kaydet
        PlayerPrefs.Save();  // PlayerPrefs'i kaydet
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", LinearToDecibel(volume));  // dB değerine çevirerek uygula
        PlayerPrefs.SetFloat("MusicVolume", volume);  // Kaydet
        PlayerPrefs.Save();  // PlayerPrefs'i kaydet

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = (volume <= 0.01f) ? 0f : 1f;  // Tamamen sessiz yapmak için doğrudan sıfırla
            Debug.Log($"BackgroundMusicSource volume updated: {backgroundMusicSource.volume}");
        }
    }

    private void LoadAudioSettings()
    {
        // Varsayılan değerleri 0.75 olarak ayarla (yarı ses seviyesi)
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);  // Linear (0.0 - 1.0)
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);    // Linear (0.0 - 1.0)

        // AudioMixer için dB'ye çevirerek uygula
        audioMixer.SetFloat("MasterVolume", LinearToDecibel(masterVolume));
        audioMixer.SetFloat("MusicVolume", LinearToDecibel(musicVolume));

        // AudioSource için doğrudan linear değeri uygula
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = musicVolume;
        }

        Debug.Log($"Ses ayarları yüklendi: MasterVolume: {masterVolume}, MusicVolume: {musicVolume}");
    }

    private float LinearToDecibel(float linear)
    {
        // Linear (0.0 - 1.0) aralığını dB (-80 dB ile 0 dB arası) cinsine çevirir
        return (linear > 0.0001f) ? 20f * Mathf.Log10(linear) : -80f;  // Minimum değer -80 dB olarak ayarlanır
    }

}
