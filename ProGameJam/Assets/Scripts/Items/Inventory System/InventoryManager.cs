using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Player player;
    public int maxStackedItems = 9;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefabs;
    int selectedSlot = -1;

    void Start()
    {
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

        // Sử dụng vật phẩm trong slot đang chọn bằng phím E
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
            inventorySlots[newValue].Select();
            selectedSlot = newValue;
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
        Item item = GetSelectedItem(true); // `true` để giảm count và xóa nếu count về 0
        if (item != null)
        {
            player.UseItem(item);
            Debug.Log($"Used item: {item.displayName}");
        }
        else
        {
            Debug.Log("No item in selected slot to use!");
        }
    }
}