using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro için eklenen namespace

public class InventoryUI : MonoBehaviour
{
    public InventorySystem inventorySystem; // InventorySystem scriptine referans
    public GameObject inventoryCanvasPrefab; // Envanter öğesinin canvas prefab'i
    public Transform inventoryContent; // Envanter prefab'inin ekleneceği alan

    private Dictionary<string, GameObject> inventoryItems = new Dictionary<string, GameObject>();

    void Start()
    {
        // InventorySystem event'ine abone ol
        inventorySystem.InventoryChanged += UpdateInventoryUI;
    }

    public void InitializeInventoryUI()
    {
        foreach (var item in inventorySystem.initialItems)
        {
            string itemName = item.Key; // Ürün adı
            AddInventoryItemUI(itemName, inventorySystem.GetInventory()[itemName]);
        }
    }

    void AddInventoryItemUI(string itemName, int count)
    {
        // Prefab'i oluştur ve parent'ına ekle
        GameObject newCanvas = Instantiate(inventoryCanvasPrefab, inventoryContent);
        newCanvas.name = $"{itemName}InventoryCanvas";

        // Canvas altındaki InventoryBox'u bul ve adlandır
        Transform inventoryBox = newCanvas.transform.Find("InventoryBox");
        if (inventoryBox != null)
        {
            inventoryBox.name = $"{itemName}InventoryBox";

            // InventoryBox altındaki InventoryImageBox'u bul ve adlandır
            Transform imageBox = inventoryBox.Find("InventoryImageBox");
            if (imageBox != null)
            {
                imageBox.name = $"{itemName}InventoryImageBox";

                // ImageBox altındaki ItemIcon'u bul ve sprite ekle
                Transform itemIcon = imageBox.Find("InventoryItemIcon");
                if (itemIcon != null)
                {
                    itemIcon.name = $"{itemName}InventoryItemIcon";
                    Image itemIconImage = itemIcon.GetComponent<Image>();
                    if (itemIconImage != null)
                    {
                        // InventorySystem'den Sprite bilgisini al
                        string spriteName = inventorySystem.GetItemProperty(itemName, "Sprite");
                        if (!string.IsNullOrEmpty(spriteName))
                        {
                            Sprite loadedSprite = Resources.Load<Sprite>($"Objects/{spriteName}");
                            if (loadedSprite != null)
                            {
                                itemIconImage.sprite = loadedSprite;
                            }
                            else
                            {
                                Debug.LogWarning($"Sprite '{spriteName}' not found in Resources/Objects");
                            }
                        }
                    }
                }
            }

            // InventoryBox altındaki InventoryItemCount'u bul ve adlandır
            Transform itemCount = inventoryBox.Find("InventoryItemCount");
            if (itemCount != null)
            {
                itemCount.name = $"{itemName}InventoryItemCount";
                Text itemCountText = itemCount.GetComponent<Text>();
                if (itemCountText != null)
                {
                    itemCountText.text = count.ToString(); // Ürün miktarını yaz
                }
            }
        }

        // Envanterdeki objeleri takip için dictionary'e ekle
        inventoryItems[itemName] = newCanvas;
    }

public void UpdateItemCountUI(string itemName, int count)
{
    Debug.Log($"Updating UI for {itemName} with count: {count}");

    if (inventoryItems.ContainsKey(itemName))
    {
        GameObject existingCanvas = inventoryItems[itemName];
        Transform inventoryBox = existingCanvas.transform.Find($"{itemName}InventoryBox");
        if (inventoryBox != null)
        {
            Transform itemCount = inventoryBox.Find($"{itemName}InventoryItemCount");
            if (itemCount != null)
            {
                TextMeshProUGUI itemCountText = itemCount.GetComponent<TextMeshProUGUI>(); // Text yerine TextMeshProUGUI kullanıldı
                if (itemCountText != null)
                {
                    itemCountText.text = count.ToString(); // Yeni miktarı güncelle
                    Debug.Log($"UI updated for {itemName}: {count}");
                }
                else
                {
                    Debug.LogWarning($"TextMeshProUGUI component not found in {itemName}InventoryItemCount!");
                }
            }
            else
            {
                Debug.LogWarning($"ItemCount transform not found for {itemName}!");
            }
        }
        else
        {
            Debug.LogWarning($"InventoryBox transform not found for {itemName}!");
        }
    }
    else
    {
        Debug.LogWarning($"Item '{itemName}' UI not found for updating!");
    }
}

    public void UpdateInventoryUI()
    {
        foreach (KeyValuePair<string, int> item in inventorySystem.GetInventory())
        {
            if (inventoryItems.ContainsKey(item.Key))
            {
                // Mevcut öğeyi güncelle
                UpdateItemCountUI(item.Key, item.Value);
            }
        }
    }
}
