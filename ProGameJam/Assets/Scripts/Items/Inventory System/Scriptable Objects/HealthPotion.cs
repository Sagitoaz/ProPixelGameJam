using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Health Potion")]
public class HealthPotion : Item
{
    [SerializeField] private int boostAmount; // Số máu tăng

    public override void Use(Player player)
    {
        if (player == null) return;

        // Tăng máu vĩnh viễn cho player
        player.SetMaxHealth(boostAmount + player.GetMaxHealth());
        Debug.Log($"Used {displayName}. Boosted {boostAmount} HP. Current HP: {player.Health}");
    }
}