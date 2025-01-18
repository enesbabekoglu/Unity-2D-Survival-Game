using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro için namespace

public class InventoryUI : MonoBehaviour
{
    public InventorySystem inventorySystem;         // InventorySystem scriptine referans
    public GameObject inventoryItemPrefab;          // Tek bir envanter öğesi prefab'i
    public Transform inventoryLayout;               // InventoryLayout referansı

    private Dictionary<string, GameObject> inventoryItems = new Dictionary<string, GameObject>();

    void Start()
    {
        // Envanteri başlat ve event'e abone ol
        inventorySystem.InventoryChanged += UpdateInventoryUI;
        InitializeInventoryUI();
    }

    public void InitializeInventoryUI()
    {
        foreach (var item in inventorySystem.GetInventory())
        {
            AddInventoryItemUI(item.Key, item.Value);
        }
    }

    void AddInventoryItemUI(string itemName, int count)
    {
        // Prefab'i oluştur ve parent'ına ekle (InventoryLayout)
        GameObject newItem = Instantiate(inventoryItemPrefab, inventoryLayout);
        newItem.name = $"{itemName}_Item";

        // InventoryItemIcon'u bul ve sprite ekle
        Transform itemIcon = newItem.transform.Find("InventoryItemIcon");
        if (itemIcon != null)
        {
            Image iconImage = itemIcon.GetComponent<Image>();
            if (iconImage != null)
            {
                string spriteName = inventorySystem.GetItemProperty(itemName, "Sprite");
                if (!string.IsNullOrEmpty(spriteName))
                {
                    Sprite loadedSprite = Resources.Load<Sprite>($"Objects/{spriteName}");
                    if (loadedSprite != null)
                    {
                        iconImage.sprite = loadedSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"Sprite '{spriteName}' not found in Resources/Objects");
                    }
                }
            }
        }

        // InventoryItemCount'u bul ve değerini güncelle
        Transform itemCount = newItem.transform.Find("InventoryItemCount");
        if (itemCount != null)
        {
            TextMeshProUGUI itemCountText = itemCount.GetComponent<TextMeshProUGUI>();
            if (itemCountText != null)
            {
                itemCountText.text = count.ToString();
            }
        }

        // Dictionary'e ekle
        inventoryItems[itemName] = newItem;
    }

    public void UpdateItemCountUI(string itemName, int count)
    {
        if (inventoryItems.ContainsKey(itemName))
        {
            GameObject existingItem = inventoryItems[itemName];

            // InventoryItemCount'u bul ve değerini güncelle
            Transform itemCount = existingItem.transform.Find("InventoryItemCount");
            if (itemCount != null)
            {
                TextMeshProUGUI itemCountText = itemCount.GetComponent<TextMeshProUGUI>();
                if (itemCountText != null)
                {
                    itemCountText.text = count.ToString();
                }
            }
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' UI not found for updating!");
        }
    }

    public void UpdateInventoryUI()
    {
        // Envanteri güncelle
        foreach (var item in inventorySystem.GetInventory())
        {
            if (inventoryItems.ContainsKey(item.Key))
            {
                UpdateItemCountUI(item.Key, item.Value);
            }
            else
            {
                AddInventoryItemUI(item.Key, item.Value);
            }
        }
    }
}
