using UnityEngine;

public class EggPickup : MonoBehaviour
{
    public string eggName = "Egg"; // Envanterde görünecek isim

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player")) // Oyuncu ile temas kontrolü
        {
            PlayerState playerState = collision.GetComponent<PlayerState>();
            if (playerState != null && playerState.inventorySystem != null)
            {
                playerState.inventorySystem.AddItem(eggName, 1); // Yumurtayı envantere ekle
                Debug.Log($"{eggName} envantere eklendi.");

                Destroy(gameObject); // Yumurtayı sahneden kaldır
            }
            else
            {
                Debug.LogWarning("PlayerState ya da InventorySystem bulunamadı!");
            }
        }
    }
}
