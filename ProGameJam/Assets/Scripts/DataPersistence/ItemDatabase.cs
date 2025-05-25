using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> allItems; // Danh sách tất cả các Item (gán trong Inspector)
    private Dictionary<int, Item> itemDictionary; // Ánh xạ từ ID sang Item

    public static ItemDatabase Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Tìm thấy nhiều hơn một ItemDatabase trong Scene. Hủy bản sao.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Đảm bảo ItemDatabase không bị hủy khi chuyển Scene

        // Khởi tạo dictionary
        itemDictionary = new Dictionary<int, Item>();
        if (allItems == null || allItems.Count == 0)
        {
            Debug.Log("Danh sách allItems trống trong ItemDatabase! Vui lòng gán các item trong Inspector.");
            return;
        }

        foreach (Item item in allItems)
        {
            if (item != null && !itemDictionary.ContainsKey(item.ID))
            {
                itemDictionary.Add(item.ID, item);
            }
            else
            {
                Debug.LogWarning($"ID Item trùng lặp hoặc không hợp lệ: {item?.ID} cho item: {item?.displayName}");
            }
        }
        Debug.Log("ItemDatabase đã được khởi tạo với " + itemDictionary.Count + " item.");
    }

    void OnEnable()
    {
        Debug.Log("ItemDatabase được bật.");
    }

    void OnDisable()
    {
        Debug.LogWarning("ItemDatabase bị tắt. Điều này có thể gây lỗi nếu inventory được tải!");
    }

    // Lấy Item theo ID
    public Item GetItemByID(int id)
    {
        if (itemDictionary.TryGetValue(id, out Item item))
        {
            return item;
        }
        Debug.LogWarning($"Item với ID {id} không tìm thấy trong ItemDatabase.");
        return null;
    }
}