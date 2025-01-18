using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private static BackgroundMusicController instance;

    void Awake()
    {
        // Eğer aynı türden başka bir nesne varsa, bu nesneyi yok et
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Bu nesneyi yok edilmekten koru ve referansı sakla
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
