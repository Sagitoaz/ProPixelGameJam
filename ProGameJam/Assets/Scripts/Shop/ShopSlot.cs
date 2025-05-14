using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    private Image background; // Image nền (ô màu trắng, Image của chính GameObject)
    private Image itemIcon;   // Image biểu tượng vật phẩm (Image từ GameObject con ItemIcon)
    private TextMeshProUGUI priceText; // Giá (TextMeshProUGUI từ GameObject con)
    private Item item; // Vật phẩm trong slot
    private ShopManager shopManager;

    void Awake()
    {
        // Lấy Image nền từ chính GameObject
        background = GetComponent<Image>();
        // Lấy Image biểu tượng từ GameObject con ItemIcon
        itemIcon = transform.Find("ItemIcon").GetComponent<Image>();
        // Đảm bảo có Button, nếu không thì thêm
        if (!GetComponent<Button>())
        {
            gameObject.AddComponent<Button>();
        }
    }

    public void Setup(Item newItem, ShopManager manager)
    {
        item = newItem;
        shopManager = manager;
        // Gán sprite cho biểu tượng vật phẩm
        if (itemIcon != null)
        {
            itemIcon.sprite = item.image != null ? item.image : null;
        }
        // Gán giá
        if (priceText != null)
        {
            priceText.text = $"{item.price} Gold"; // Giả sử ID là giá (có thể thay đổi)
        }
        // Đảm bảo nền hiển thị
        if (background != null)
        {
            background.gameObject.SetActive(true);
        }
        GetComponent<Button>().onClick.AddListener(() => shopManager.DisplayItemInfo(item));
    }
}