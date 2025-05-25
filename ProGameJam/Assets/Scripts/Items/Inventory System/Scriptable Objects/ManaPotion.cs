using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Mana Potion")]
public class ManaPotion : Item
{
    [SerializeField] private int manaAmount; // Số mana hồi phục

    public override void Use(Player player)
    {
        if (player == null) return;

        // Hồi mana cho player
        if (player.GetMana() == 100)
        {
            Debug.Log("Player's mana is full!");
            return;
        }
        else if (player.GetMana() + manaAmount > 100) player.SetMana(100);
        else player.SetMana(player.GetMana() + manaAmount);
        Debug.Log($"Used {displayName}. Restored {manaAmount} Mana. Current Mana: {player.GetMana()}");
    }
}