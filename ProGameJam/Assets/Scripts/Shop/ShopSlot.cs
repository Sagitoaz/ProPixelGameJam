using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    private Image background; // Image nền (ô màu trắng, Image của chính GameObject)
    private Image itemIcon;   // Image biểu tượng vật phẩm (Image từ GameObject con ItemIcon)
    private TextMeshProUGUI stockText; // Số lượng tồn kho (TextMeshProUGUI từ Button Stock)
    private Item item; // Vật phẩm trong slot
    private ShopManager shopManager;
    private int itemIndex; // Chỉ số của vật phẩm trong danh sách shopItems

    void Awake()
    {
        // Lấy Image nền từ chính GameObject
        background = GetComponent<Image>();
        // Lấy Image biểu tượng từ GameObject con ItemIcon
        itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        // Lấy TextMeshProUGUI số lượng tồn kho từ Button Stock
        Button stockButton = transform.Find("Stock").GetComponent<Button>();
        if (stockButton != null)
        {
            stockText = stockButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        if (stockText == null)
        {
            Debug.LogError("Stock TextMeshProUGUI not found in ShopSlot!");
        }
        // Đảm bảo có Button, nếu không thì thêm
        if (!GetComponent<Button>())
        {
            gameObject.AddComponent<Button>();
        }

        // Ban đầu ẩn ItemIcon và Stock
        if (itemIcon != null) itemIcon.gameObject.SetActive(false);
        if (stockText != null) stockText.transform.parent.gameObject.SetActive(false);
    }

    public void Setup(Item newItem, ShopManager manager, int index)
    {
        item = newItem;
        shopManager = manager;
        itemIndex = index;
        // Đảm bảo nền luôn hiển thị
        if (background != null)
        {
            background.gameObject.SetActive(true);
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (shopManager == null || itemIndex >= shopManager.shopItems.Length) return;

        int currentStock = shopManager.shopItems[itemIndex].stock;
        if (item == null || currentStock <= 0)
        {
            // Khi không có vật phẩm hoặc hết hàng, ẩn ItemIcon và Stock
            if (itemIcon != null) itemIcon.gameObject.SetActive(false);
            if (stockText != null) stockText.transform.parent.gameObject.SetActive(false);
            // Xóa sự kiện onClick và đặt item thành null
            GetComponent<Button>().onClick.RemoveAllListeners();
            item = null;
        }
        else
        {
            // Hiển thị ItemIcon và Stock nếu có vật phẩm và còn hàng
            if (itemIcon != null)
            {
                itemIcon.sprite = item.image != null ? item.image : null;
                itemIcon.gameObject.SetActive(true);
            }
            if (stockText != null)
            {
                stockText.text = currentStock.ToString();
                stockText.transform.parent.gameObject.SetActive(true); // Hiển thị Button Stock
            }
            // Gán sự kiện onClick
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(() => shopManager.DisplayItemInfo(item, itemIndex));
        }
    }
}