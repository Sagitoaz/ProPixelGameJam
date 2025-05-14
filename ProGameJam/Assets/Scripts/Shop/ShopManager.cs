using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Player player;
    public Item[] shopItems; // Danh sách vật phẩm trong shop (gán trong Inspector)
    public ShopSlot[] shopSlots; // 6 slot trong UI (gán các GameObject Image chứa script ShopSlot)
    public TextMeshProUGUI itemNameText; // Text cho tên vật phẩm
    public TextMeshProUGUI itemDescriptionText; // Text cho mô tả
    public TextMeshProUGUI itemPriceText; // Text cho giá
    public Button buyButton; // Nút mua
    public TextMeshProUGUI coinRemainingText; // Text hiển thị số vàng (CoinRemaining)
    public InventoryManager inventoryManager; // Tham chiếu đến InventoryManager

    private Item selectedItem; // Vật phẩm đang được chọn

    void Start()
    {
        // Kiểm tra các tham chiếu
        if (itemNameText == null) Debug.LogError("itemNameText is not assigned in ShopManager!");
        if (itemDescriptionText == null) Debug.LogError("itemDescriptionText is not assigned in ShopManager!");
        if (itemPriceText == null) Debug.LogError("itemPriceText is not assigned in ShopManager!");
        if (buyButton == null) Debug.LogError("buyButton is not assigned in ShopManager!");
        if (coinRemainingText == null) Debug.LogError("coinRemainingText is not assigned in ShopManager!");
        if (player == null) Debug.LogError("playerGoldManager is not assigned in ShopManager!");
        if (inventoryManager == null) Debug.LogError("inventoryManager is not assigned in ShopManager!");
        if (shopSlots == null || shopSlots.Length == 0) Debug.LogError("shopSlots array is not assigned or empty in ShopManager!");
        PopulateShop();
        if (buyButton != null) buyButton.onClick.AddListener(BuyItem);
        UpdateDescriptionPanel(null);
        UpdateGoldDisplay();
    }

    void PopulateShop()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (i < shopItems.Length && i < shopSlots.Length) // Đảm bảo không vượt quá số slot
            {
                shopSlots[i].Setup(shopItems[i], this);
            }
            // else
            // {
            //     shopSlots[i].gameObject.SetActive(false); // Ẩn slot nếu không có vật phẩm
            // }
        }
    }

    public void DisplayItemInfo(Item item)
    {
        selectedItem = item;
        UpdateDescriptionPanel(item);
    }

    void UpdateDescriptionPanel(Item item)
    {
        if (item == null)
        {
            if (itemNameText != null) itemNameText.text = "";
            if (itemDescriptionText != null) itemDescriptionText.text = "";
            if (itemPriceText != null) itemPriceText.text = "";
            if (buyButton != null) buyButton.gameObject.SetActive(false);
        }
        else
        {
            if (itemNameText != null) itemNameText.text = item.displayName;
            if (itemDescriptionText != null) itemDescriptionText.text = item.description;
            if (itemPriceText != null) itemPriceText.text = $"{item.price}";
            if (buyButton != null) buyButton.gameObject.SetActive(true);
        }
    }

    void UpdateGoldDisplay()
    {
        if (coinRemainingText != null && player != null)
        {
            coinRemainingText.text = player.GetCoin().ToString() + " Gold";
        }
    }

    void BuyItem()
    {
        if (selectedItem != null && inventoryManager != null && player != null)
        {
            // Kiểm tra xem có đủ vàng không (tùy chọn, bạn có thể bỏ nếu đã xử lý trong Player)
            if (player.GetCoin() >= selectedItem.price)
            {
                // Thêm vật phẩm vào inventory
                if (inventoryManager.AddItem(selectedItem))
                {
                    player.DeductCoin(selectedItem.price);
                    UpdateGoldDisplay(); // Cập nhật UI sau khi trừ vàng
                    Debug.Log($"Bought {selectedItem.displayName}!");
                }
                else
                {
                    Debug.Log("Inventory full!");
                    // Có thể thêm thông báo cho người chơi ở đây
                }
            }
            else
            {
                Debug.Log("Not enough gold!");
                // Có thể thêm thông báo cho người chơi ở đây
            }
        }
    }
}