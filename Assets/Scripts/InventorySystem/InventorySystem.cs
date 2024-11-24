using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Envanter değişikliklerini bildiren event
    public event System.Action InventoryChanged;

    // Ürünlerin miktarlarını tutan dictionary
    public Dictionary<string, int> itemAmounts = new Dictionary<string, int>();

    // Başlangıç ürünlerini ve özelliklerini tutan dictionary
    public Dictionary<string, Dictionary<string, string>> initialItems = new Dictionary<string, Dictionary<string, string>>();

    public InventoryUI inventoryUI; // InventoryUI referansı

    void Awake()
    {
        // Eğer initialItems boşsa varsayılan değerleri ekle
        if (initialItems.Count == 0)
        {
            initialItems = new Dictionary<string, Dictionary<string, string>>()
            {
                { "Blackberry", new Dictionary<string, string> { { "TextName", "Böğürtlen" } , { "Sprite", "Blackberry" } , { "Edible", "True" } , { "FoodBonus", "5" } } },
                { "Wood", new Dictionary<string, string> { { "TextName", "Odun" } , { "Sprite", "Wood_2" } , { "Edible", "False" } , { "FoodBonus", "0" } } },
                { "Fish", new Dictionary<string, string> { { "TextName", "Balık" } , { "Sprite", "Fish" } , { "Edible", "False" } , { "FoodBonus", "0" } } },
                { "Grilled_Fish", new Dictionary<string, string> { { "TextName", "Pişmiş Balık" } , { "Sprite", "Grilled_Fish" } , { "Edible", "True" } , { "FoodBonus", "10" } } },
            };
            Debug.Log("Initial items set to default values: Fish, Wood");
        }
    }

    public void Start()
    {
        // Başlangıç ürünlerini envantere ekle
        foreach (var item in initialItems)
        {
            string itemName = item.Key;
            if (!itemAmounts.ContainsKey(itemName))
            {
                itemAmounts[itemName] = 0;
                Debug.Log($"Item added to inventory: {itemName}");
            }
        }
    }

    // Ürün ekleme
    public void AddItem(string itemName, int amount)
    {
        if (itemAmounts.ContainsKey(itemName))
        {
            itemAmounts[itemName] += amount;
            Debug.Log($"Added {amount} of {itemName}. New amount: {itemAmounts[itemName]}");

            // UI'yi güncelle
            inventoryUI.UpdateItemCountUI(itemName, itemAmounts[itemName]);
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' does not exist in the inventory!");
            return;
        }

        // Envanter değişimini bildir
        InventoryChanged?.Invoke();
    }

    // Ürün çıkarma
    public void RemoveItem(string itemName, int amount)
    {
        if (itemAmounts.ContainsKey(itemName))
        {
            itemAmounts[itemName] -= amount;
            if (itemAmounts[itemName] < 0)
                itemAmounts[itemName] = 0; // Negatif olmamasını sağla
            Debug.Log($"Removed {amount} of {itemName}. New amount: {itemAmounts[itemName]}");

            // UI'yi güncelle
            inventoryUI.UpdateItemCountUI(itemName, itemAmounts[itemName]);
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' does not exist in the inventory!");
            return;
        }

        // Envanter değişimini bildir
        InventoryChanged?.Invoke();
    }

    // Belirli bir ürünün sprite değerini al
    public string GetItemProperty(string itemName, string propertyKey)
    {
        if (initialItems.ContainsKey(itemName) && initialItems[itemName].ContainsKey(propertyKey))
        {
            return initialItems[itemName][propertyKey];
        }
        Debug.LogWarning($"Item '{itemName}' içinde '{propertyKey}' anahtarı bulunamadı!");
        return null;
    }

    // Belirli bir ürünün miktarını döndür
    public int GetItemCount(string itemName)
    {
        if (itemAmounts.ContainsKey(itemName))
        {
            return itemAmounts[itemName];
        }
        Debug.LogWarning($"Item '{itemName}' does not exist in the inventory!");
        return 0; // Ürün bulunamazsa 0 döner
    }

    // Tüm ürün adları ve miktarlarını döndür
    public Dictionary<string, int> GetInventory()
    {
        return new Dictionary<string, int>(itemAmounts); // Güvenlik için kopya döndür
    }
}
