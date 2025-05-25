using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private ShopManager shopManager; // Tham chiếu đến ShopManager
    private bool playerInRange = false; // Theo dõi xem Player có ở trong vùng va chạm không
    private bool isShopOpen = false; // Theo dõi trạng thái của ShopCanvas

    void Update()
    {
        // Kiểm tra phím "E" khi Player ở trong vùng va chạm
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShopCanvas();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Khi Player vào vùng va chạm
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered shop range. Press E to open/close shop.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Đảm bảo Player vẫn ở trong vùng va chạm
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Khi Player rời vùng va chạm
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // Tắt ShopCanvas khi rời vùng va chạm
            if (isShopOpen)
            {
                CloseShopCanvas();
            }
            Debug.Log("Player left shop range.");
        }
    }

    private void ToggleShopCanvas()
    {
        if (shopCanvas == null)
        {
            Debug.LogError("Shop Canvas not assigned in Inspector!");
            return;
        }

        if (shopManager == null)
        {
            Debug.LogError("Shop Manager not assigned in Inspector!");
            return;
        }

        // Đảo ngược trạng thái của ShopCanvas
        isShopOpen = !isShopOpen;
        shopCanvas.SetActive(isShopOpen);

        if (isShopOpen)
        {
            // Khi mở ShopCanvas, làm mới giao diện cửa hàng
            shopManager.RefreshShop();
            Debug.Log("Opened Shop Canvas");
        }
        else
        {
            Debug.Log("Closed Shop Canvas");
        }
    }

    private void CloseShopCanvas()
    {
        if (shopCanvas != null)
        {
            shopCanvas.SetActive(false);
            isShopOpen = false;
        }
    }
}