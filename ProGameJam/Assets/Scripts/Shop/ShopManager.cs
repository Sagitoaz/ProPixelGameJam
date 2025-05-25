using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Player player;
    public ShopItem[] shopItems; // Danh sách vật phẩm trong shop (gán trong Inspector)
    public ShopSlot[] shopSlots; // 6 slot trong UI (gán các GameObject Image chứa script ShopSlot)
    public TextMeshProUGUI itemNameText; // Text cho tên vật phẩm
    public TextMeshProUGUI itemDescriptionText; // Text cho mô tả
    public TextMeshProUGUI itemPriceText; // Text cho giá
    public Button buyButton; // Nút mua
    public TextMeshProUGUI coinRemainingText; // Text hiển thị số coin (CoinRemaining)
    public InventoryManager inventoryManager; // Tham chiếu đến InventoryManager

    private Item selectedItem; // Vật phẩm đang được chọn
    private int selectedItemIndex; // Chỉ số của vật phẩm được chọn trong shopItems

    void Start()
    {
        // Kiểm tra các tham chiếu
        if (itemNameText == null) Debug.LogError("itemNameText is not assigned in ShopManager!");
        if (itemDescriptionText == null) Debug.LogError("itemDescriptionText is not assigned in ShopManager!");
        if (itemPriceText == null) Debug.LogError("itemPriceText is not assigned in ShopManager!");
        if (buyButton == null) Debug.LogError("buyButton is not assigned in ShopManager!");
        if (coinRemainingText == null) Debug.LogError("coinRemainingText is not assigned in ShopManager!");
        if (player == null) Debug.LogError("player is not assigned in ShopManager!");
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
            if (i < shopItems.Length && shopItems[i].item != null && shopItems[i].stock > 0) // Chỉ gán nếu có vật phẩm và còn hàng
            {
                shopSlots[i].Setup(shopItems[i].item, this, i);
            }
            else
            {
                shopSlots[i].Setup(null, this, i); // Gán null để đảm bảo slot trống
            }
            shopSlots[i].gameObject.SetActive(true); // Luôn hiển thị ô slot
        }
    }

    // Hàm làm mới giao diện cửa hàng khi mở ShopCanvas
    public void RefreshShop()
    {
        PopulateShop();
        UpdateGoldDisplay();
        UpdateDescriptionPanel(null); // Xóa bảng mô tả khi mở lại
        selectedItem = null;
        selectedItemIndex = -1;
    }

    public void DisplayItemInfo(Item item, int itemIndex)
    {
        // Kiểm tra nếu stock của vật phẩm là 0 thì không hiển thị bảng mô tả
        if (item == null || itemIndex >= shopItems.Length || shopItems[itemIndex].stock <= 0)
        {
            UpdateDescriptionPanel(null);
            selectedItem = null;
            selectedItemIndex = -1;
            return;
        }

        selectedItem = item;
        selectedItemIndex = itemIndex;
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
            if (itemPriceText != null) itemPriceText.text = $"{item.price} Coin";
            if (buyButton != null) buyButton.gameObject.SetActive(true);
        }
    }

    void UpdateGoldDisplay()
    {
        if (coinRemainingText != null && player != null)
        {
            coinRemainingText.text = player.GetCoin().ToString() + " Coin";
        }
    }

    void BuyItem()
    {
        if (selectedItem != null && inventoryManager != null && player.GetCoin() >= selectedItem.price)
        {
            // Thêm vật phẩm vào inventory
            if (inventoryManager.AddItem(selectedItem))
            {
                // Giảm số lượng tồn kho, nhưng không cho phép giảm xuống âm
                if (shopItems[selectedItemIndex].stock > 0)
                {
                    shopItems[selectedItemIndex].stock--;
                    player.DeductCoin(selectedItem.price);
                    UpdateGoldDisplay();
                    string boughtItem = selectedItem.displayName;
                    // Cập nhật giao diện cửa hàng
                    if (shopItems[selectedItemIndex].stock <= 0)
                    {
                        // Nếu hết hàng, ẩn biểu tượng và số lượng, xóa bảng mô tả
                        shopSlots[selectedItemIndex].UpdateDisplay();
                        UpdateDescriptionPanel(null);
                        selectedItem = null;
                        selectedItemIndex = -1;
                    }
                    else
                    {
                        // Cập nhật số lượng tồn kho trên ShopSlot
                        shopSlots[selectedItemIndex].UpdateDisplay();
                    }
                    Debug.Log($"Bought {boughtItem}!");
                }
            }
            else
            {
                Debug.Log("Inventory full!");
            }
        }
    }
}