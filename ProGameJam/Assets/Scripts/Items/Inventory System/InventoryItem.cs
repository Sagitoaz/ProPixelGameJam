using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")] public Image image;
    public Text countText;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    private void Start(){
        InitialiseItem(item);
    }
    public void InitialiseItem(Item newItem){
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }
    public void RefreshCount(){
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    // Drag and drop item
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
