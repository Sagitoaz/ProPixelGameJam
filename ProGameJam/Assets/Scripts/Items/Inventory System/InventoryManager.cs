using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Player player;
    public int maxStackedItems = 9;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefabs;
    int selectedSlot = -1;

    void Start()
    {
        // Kiểm tra inventorySlots
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            Debug.LogError("inventorySlots chưa được gán hoặc trống trong InventoryManager!");
            inventorySlots = new InventorySlot[5]; // Đảm bảo có 5 slot nếu không được gán
        }

        ChangeSelectedSlot(-1);
    }

    private void Update()
    {
        // Chọn slot bằng phím số (1-5)
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= inventorySlots.Length)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        // Sử dụng vật phẩm trong slot đang chọn bằng phím F
        if (Input.GetKeyDown(KeyCode.F) && selectedSlot >= 0)
        {
            UseSelectedItem();
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        if (newValue >= 0 && newValue < inventorySlots.Length)
        {
            InventorySlot slot = inventorySlots[newValue];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null) // Chỉ chọn slot nếu có vật phẩm
            {
                inventorySlots[newValue].Select();
                selectedSlot = newValue;
                Debug.Log($"Đã chọn slot {newValue + 1} với vật phẩm: {itemInSlot.item.displayName}");
            }
            else
            {
                Debug.Log($"Slot {newValue + 1} trống, không thể chọn.");
                selectedSlot = -1;
            }
        }
        else
        {
            selectedSlot = -1;
        }
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefabs, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length) return null;

        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); // Lấy từ slot đang chọn
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                    ChangeSelectedSlot(-1); // Bỏ chọn slot khi vật phẩm bị xóa
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }

    // Sử dụng vật phẩm trong slot đang chọn
    private void UseSelectedItem()
    {
        if (selectedSlot < 0 || selectedSlot >= inventorySlots.Length)
        {
            Debug.Log("Không có slot nào được chọn để sử dụng vật phẩm!");
            return;
        }

        Item item = GetSelectedItem(true); // `true` để giảm count và xóa nếu count về 0
        if (item != null)
        {
            player.UseItem(item);
            Debug.Log($"Đã sử dụng vật phẩm: {item.displayName}");
        }
        else
        {
            Debug.Log("Không có vật phẩm trong slot được chọn để sử dụng!");
        }
    }

    // Triển khai IDataPersistence
    public void LoadData(GameData data)
    {
        // Kiểm tra ItemDatabase
        if (ItemDatabase.Instance == null)
        {
            Debug.LogError("ItemDatabase.Instance là null! Không thể tải inventory items. Tạo mới...");
            GameObject databaseGO = new GameObject("ItemDatabase");
            ItemDatabase database = databaseGO.AddComponent<ItemDatabase>();
            // Bạn cần gán thủ công danh sách allItems nếu cần
            // database.allItems = ... (gán danh sách Item nếu có)
        }

        // Kiểm tra inventorySlots
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            Debug.LogError("inventorySlots chưa được gán hoặc trống trong InventoryManager! Không thể tải inventory.");
            return;
        }

        // Kiểm tra data.inventorySlots
        if (data.inventorySlots == null)
        {
            Debug.LogError("data.inventorySlots là null! Khởi tạo với giá trị mặc định.");
            data.inventorySlots = new InventorySlotData[inventorySlots.Length];
            for (int i = 0; i < data.inventorySlots.Length; i++)
            {
                data.inventorySlots[i] = new InventorySlotData();
            }
        }

        // Xóa toàn bộ vật phẩm hiện có trong inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }

        // Khôi phục inventory từ GameData
        int slotsToLoad = Mathf.Min(inventorySlots.Length, data.inventorySlots.Length);
        for (int i = 0; i < slotsToLoad; i++)
        {
            InventorySlotData slotData = data.inventorySlots[i];
            if (slotData == null)
            {
                Debug.LogWarning($"data.inventorySlots[{i}] là null! Bỏ qua slot.");
                continue;
            }

            if (slotData.itemID != -1 && slotData.count > 0) // Có vật phẩm trong slot
            {
                // Lấy Item từ ItemDatabase dựa trên itemID
                Item item = ItemDatabase.Instance.GetItemByID(slotData.itemID);
                if (item != null)
                {
                    // Tạo lại InventoryItem trong slot
                    GameObject newItemGO = Instantiate(inventoryItemPrefabs, inventorySlots[i].transform);
                    InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
                    inventoryItem.InitialiseItem(item);
                    inventoryItem.SetCount(slotData.count); // Đặt số lượng từ dữ liệu lưu
                }
                else
                {
                    Debug.LogWarning($"Vật phẩm với ID {slotData.itemID} không tìm thấy trong ItemDatabase!");
                }
            }
        }

        Debug.Log("Đã tải inventory thành công!");
    }

    public void SaveData(ref GameData data)
    {
        // Đảm bảo data.inventorySlots có đủ chiều dài
        if (data.inventorySlots.Length != inventorySlots.Length)
        {
            Debug.LogWarning($"Chênh lệch độ dài inventorySlots. Dự kiến: {inventorySlots.Length}, Tìm thấy: {data.inventorySlots.Length}. Điều chỉnh kích thước...");
            Array.Resize(ref data.inventorySlots, inventorySlots.Length);
            for (int i = 0; i < data.inventorySlots.Length; i++)
            {
                if (data.inventorySlots[i] == null)
                {
                    data.inventorySlots[i] = new InventorySlotData();
                }
            }
        }

        // Lưu trạng thái inventory vào GameData
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                // Có vật phẩm trong slot, lưu ID và số lượng
                data.inventorySlots[i].itemID = itemInSlot.item.ID;
                data.inventorySlots[i].count = itemInSlot.count;
            }
            else
            {
                // Slot trống, đặt itemID = -1
                data.inventorySlots[i].itemID = -1;
                data.inventorySlots[i].count = 0;
            }
        }
    }
}