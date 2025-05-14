using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 9;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefabs;
    int selectedSlot = -1;
    void Start()
    {
        ChangeSelectedSlot(0);
    }
    private void Update(){
        if (Input.inputString != null){
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 6){
                ChangeSelectedSlot(number - 1);
            }
        }
    }
    void ChangeSelectedSlot(int newValue){
        if (selectedSlot >= 0){
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    public bool AddItem(Item item){
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                    itemInSlot.item == item &&
                    itemInSlot.count < maxStackedItems &&
                    itemInSlot.item.stackable == true){
                itemInSlot.count++;
                itemInSlot.RefreshCount();
            return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null){
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot){
        GameObject newItemGO = Instantiate(inventoryItemPrefabs, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use){
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null){
            Item item = itemInSlot.item;
            if (use == true){
                itemInSlot.count--;
                if (itemInSlot.count <= 0){
                    Destroy(itemInSlot.gameObject);
                }
                else{
                    itemInSlot.RefreshCount();
                }
            }
            return itemInSlot.item;
        }
        return null;
    }
}
