using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Objects/ Inventory Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")] 
    public int ID;
    public string displayName;
    [TextArea(4,4)] public string description;
    public int price;
    // public TileBase tile;
    public ItemType itemType;
    public ActionType actionType;
    // public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}

public enum ItemType{
    Food,
    Potion
}

public enum ActionType{
    Heal,
    Buff
}
