using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EatingSystem : MonoBehaviour
{
    public GameObject EatingUI; // Yemek panelinin ana UI'si
    public GameObject EatingPanelFoodListCanvas; // Yemek paneli
    public GameObject EatingPanelFoodBoxCanvas; // Yemek kutusu prefabı
    public InventorySystem InventorySystem; // Envanter sistemi

    private void Start()
    {
        // Başlangıçta yemek panelini gizle
        if (EatingUI != null)
        {
            EatingUI.SetActive(false);
        }
        else
        {
            Debug.LogError("EatingUI atanmamış!");
        }
    }

    public void ActivateEatingUI()
    {
        Debug.Log("EatingUI aktif ediliyor.");
        if (EatingUI != null)
        {
            EatingUI.SetActive(true); // Yemek panelini aktif et
            UpdateEatingUI(); // UI güncelle
        }
        else
        {
            Debug.LogError("EatingUI aktif edilemiyor çünkü atanmamış!");
        }
    }

    public void DeactivateEatingUI()
    {
        Debug.Log("EatingUI pasif ediliyor.");
        if (EatingUI != null)
        {
            EatingUI.SetActive(false); // Yemek panelini pasif et
        }
    }

    public void UpdateEatingUI()
    {
        Debug.Log("UpdateEatingUI çalıştırılıyor.");
        if (EatingPanelFoodListCanvas == null)
        {
            Debug.LogError("EatingPanelFoodListCanvas atanmamış!");
            return;
        }

        if (EatingPanelFoodBoxCanvas == null)
        {
            Debug.LogError("EatingPanelFoodBoxCanvas atanmamış!");
            return;
        }

        // Paneldeki mevcut öğeleri temizle
        foreach (Transform child in EatingPanelFoodListCanvas.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Önceki UI öğeleri temizlendi.");

        // Envanterdeki ürünleri al
        if (InventorySystem == null)
        {
            Debug.LogError("InventorySystem atanmamış!");
            return;
        }

        Dictionary<string, int> inventory = InventorySystem.GetInventory();
        Debug.Log($"Envanterdeki ürün sayısı: {inventory.Count}");

        foreach (var item in inventory)
        {
            string itemName = item.Key;
            int itemCount = item.Value;
            Debug.Log($"İşlenen ürün: {itemName}, Miktar: {itemCount}");

            if (itemCount > 0 &&
                InventorySystem.initialItems.ContainsKey(itemName) &&
                InventorySystem.initialItems[itemName].ContainsKey("Edible") &&
                InventorySystem.initialItems[itemName]["Edible"] == "True")
            {
                Debug.Log($"Edible ürün bulundu: {itemName}");

                // Prefabı oluştur
                GameObject foodBox = Instantiate(EatingPanelFoodBoxCanvas, EatingPanelFoodListCanvas.transform);
                Debug.Log($"Prefab oluşturuldu: {foodBox.name}");

                Transform foodBoxTransform = foodBox.transform.Find("EatingPanelFoodBox");
                if (foodBoxTransform == null)
                {
                    Debug.LogError($"EatingPanelFoodBox alt nesnesi prefab içinde bulunamadı: {foodBox.name}");
                    continue;
                }

                // FoodIcon
                Transform foodIconTransform = foodBoxTransform.Find("FoodIcon");
                if (foodIconTransform != null)
                {
                    Image foodIcon = foodIconTransform.GetComponent<Image>();
                    if (foodIcon != null)
                    {
                        string spriteName = InventorySystem.GetItemProperty(itemName, "Sprite");
                        Sprite itemSprite = Resources.Load<Sprite>($"Objects/{spriteName}");
                        if (itemSprite != null)
                        {
                            foodIcon.sprite = itemSprite;
                            Debug.Log($"Sprite başarıyla yüklendi: {spriteName}");
                        }
                        else
                        {
                            Debug.LogError($"Sprite yüklenemedi: {spriteName}");
                        }
                    }
                }

                // FoodCount
                Transform foodCountTransform = foodBoxTransform.Find("FoodCount");
                if (foodCountTransform != null)
                {
                    TMP_Text foodCount = foodCountTransform.GetComponent<TMP_Text>();
                    if (foodCount != null)
                    {
                        foodCount.text = itemCount.ToString();
                        Debug.Log($"FoodCount güncellendi: {itemCount}");
                    }
                }

                // FoodName
                Transform foodNameTransform = foodBoxTransform.Find("FoodName");
                if (foodNameTransform != null)
                {
                    TMP_Text foodName = foodNameTransform.GetComponent<TMP_Text>();
                    if (foodName != null)
                    {
                        foodName.text = InventorySystem.GetItemProperty(itemName, "TextName");
                        Debug.Log($"FoodName güncellendi: {InventorySystem.GetItemProperty(itemName, "TextName")}");
                    }
                }

                // Tıklama davranışı ekle
                Button foodButton = foodBox.GetComponent<Button>();
                if (foodButton != null)
                {
                    string currentItem = itemName; // Closure sorunlarını önlemek için
                    foodButton.onClick.RemoveAllListeners();
                    foodButton.onClick.AddListener(() => OnFoodSelected(currentItem));
                    Debug.Log($"Tıklama davranışı eklendi: {currentItem}");
                }
                else
                {
                    Debug.LogError("Button bileşeni prefab içinde bulunamadı!");
                }
            }
        }
    }

    public void OnFoodSelected(string itemName)
    {
        Debug.Log($"OnFoodSelected çağrıldı. Seçilen ürün: {itemName}");

        string foodBonusStr = InventorySystem.GetItemProperty(itemName, "FoodBonus");
        if (string.IsNullOrEmpty(foodBonusStr))
        {
            Debug.LogError($"FoodBonus değeri bulunamadı veya geçersiz! Item: {itemName}");
            return;
        }

        if (!int.TryParse(foodBonusStr, out int foodBonus))
        {
            Debug.LogError($"FoodBonus değeri bir sayı değil! Item: {itemName}, Value: {foodBonusStr}");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance bulunamadı!");
            return;
        }

        GameManager.Instance.Hunger += foodBonus;
        if (GameManager.Instance.Hunger > GameManager.Instance.MaxHunger)
        {
            GameManager.Instance.Hunger = GameManager.Instance.MaxHunger;
        }
        Debug.Log($"Hunger güncellendi: {GameManager.Instance.Hunger}");

        InventorySystem.RemoveItem(itemName, 1);
        Debug.Log($"{itemName} envanterden 1 adet çıkarıldı.");

        UpdateEatingUI();
    }

}
