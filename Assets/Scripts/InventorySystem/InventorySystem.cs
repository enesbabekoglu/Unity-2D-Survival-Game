using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Ürünlerin miktarlarını tutan dictionary
    public Dictionary<string, int> itemAmounts = new Dictionary<string, int>();

    // Başlangıç ürünlerini tanımlayan dizi
    public string[] initialItems;

    void Start()
    {
        foreach (string itemName in initialItems)
        {
            if (!itemAmounts.ContainsKey(itemName))
            {
                itemAmounts[itemName] = 0;
                Debug.Log($"Item added to inventory: {itemName}");
            }
        }

        Debug.Log("Initial items created!");
    }

    // Ürün ekleme
    public void AddItem(string itemName, int amount)
    {
        if (itemAmounts.ContainsKey(itemName))
        {
            itemAmounts[itemName] += amount;
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' does not exist in the inventory!");
        }
    }

    // Ürün çıkarma
    public void RemoveItem(string itemName, int amount)
    {
        if (itemAmounts.ContainsKey(itemName))
        {
            itemAmounts[itemName] -= amount;
            if (itemAmounts[itemName] < 0)
                itemAmounts[itemName] = 0; // Negatif olmamasını sağla
        }
        else
        {
            Debug.LogWarning($"Item '{itemName}' does not exist in the inventory!");
        }
    }

    // Belirli bir ürünün miktarını al
    public int GetItemAmount(string itemName)
    {
        if (itemAmounts.ContainsKey(itemName))
        {
            return itemAmounts[itemName];
        }
        return 0;
    }

    // Tüm ürün adları ve miktarlarını döndür
    public Dictionary<string, int> GetInventory()
    {
        return itemAmounts; // Kopya yerine referans döndür
    }

    void Awake()
    {
        // Eğer Inspector'da değerler boşsa, varsayılan değerleri ekle
        if (initialItems == null || initialItems.Length == 0)
        {
            initialItems = new string[] { "Fish", "Wood" };
            Debug.Log("Initial items set to default values: Fish, Wood");
        }
    }

}
