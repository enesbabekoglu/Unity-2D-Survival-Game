using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Inventory yönetimi için mevcut bileşenler
    public InventorySystem inventorySystem;
    public InventoryUI inventoryUI;

    // PlayerData değişkenleri
    public float Hunger = 30f; // Açlık başlangıç değeri
    public float Health = 100f; // Sağlık başlangıç değeri
    public float MaxHunger = 100f;
    public float MaxHealth = 100f;
    public float MaxHungerMinutes = 180f; // Açlık süresi (dakika)

    public float fireHealth = 0.0f;
    public bool isBurning = false;

    // Singleton
    public static GameManager Instance;

    void Awake()
    {
        // Singleton yapılandırması
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Inventory bileşenlerini bağla
        if (inventorySystem == null)
        {
            Debug.LogError("InventorySystem is not assigned in the GameManager!");
        }

        if (inventoryUI == null)
        {
            Debug.LogError("InventoryUI is not assigned in the GameManager!");
        }

        if (inventorySystem != null && inventoryUI != null)
        {
            inventorySystem.inventoryUI = inventoryUI;
        }
    }

    void Start()
    {
        if (inventorySystem != null && inventoryUI != null)
        {
            inventorySystem.Start();
            inventoryUI.InitializeInventoryUI();
            Debug.Log("GameManager initialized!");

            // Test için başlangıçta bazı ürünler ekleyelim
            inventorySystem.AddItem("Blackberry", 3);
            inventorySystem.AddItem("Wood", 5);

        }
    }
}
