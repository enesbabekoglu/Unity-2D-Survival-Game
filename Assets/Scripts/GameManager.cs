using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI.UpdateUI();
    }

}
