using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryCanvas; // Prefab'ınızı Inspector'da atayın
    public Transform TopMenu;
    public InventorySystem inventorySystem;

    private Dictionary<string, GameObject> uiItems = new Dictionary<string, GameObject>();

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log("UpdateUI çağrıldı.");
        foreach (var item in inventorySystem.GetInventory())
        {
            string itemName = item.Key;
            int itemAmount = item.Value;

            Debug.Log($"UI'de güncellenecek öğe: {itemName}, Miktar: {itemAmount}");

            if (uiItems.ContainsKey(itemName))
            {
                var itemUI = uiItems[itemName];
                var countText = itemUI.transform.Find("InventoryItemCount")?.GetComponent<TextMeshProUGUI>();
                if (countText != null)
                {
                    countText.text = itemAmount.ToString();
                    Debug.Log($"UI güncellendi: {itemName}");
                }
                else
                {
                    Debug.LogError($"InventoryItemCount bulunamadı: {itemName}");
                }
            }
            else
            {
                GameObject newItemUI = Instantiate(InventoryCanvas, TopMenu);
                newItemUI.name = $"{itemName}Box";
                Debug.Log($"Yeni UI oluşturuldu: {itemName}");

                var icon = newItemUI.transform.Find("InventoryImageBox/InventoryItemIcon")?.GetComponent<Image>();
                var count = newItemUI.transform.Find("InventoryItemCount")?.GetComponent<TextMeshProUGUI>();

                if (icon == null || count == null)
                {
                    Debug.LogError($"Prefab içinde gerekli elemanlar bulunamadı: {itemName}");
                    Destroy(newItemUI); // Yanlış oluşturulan nesneyi temizle
                    continue;
                }

                icon.sprite = GetItemSprite(itemName);
                count.text = itemAmount.ToString();

                uiItems[itemName] = newItemUI;
            }
        }
    }

    private Sprite GetItemSprite(string itemName)
    {
        var sprite = Resources.Load<Sprite>($"Objects/{itemName}");
        if (sprite == null)
        {
            Debug.LogWarning($"Sprite bulunamadı: {itemName}. Varsayılan sprite kullanılıyor.");
            sprite = Resources.Load<Sprite>("Objects/DefaultItem"); // Varsayılan bir sprite olduğundan emin olun
        }
        return sprite;
    }
}