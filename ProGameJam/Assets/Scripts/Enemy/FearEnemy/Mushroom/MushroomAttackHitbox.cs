using System.Collections;
using UnityEngine;

public class MushroomAttackHitbox : MonoBehaviour
{
    [SerializeField] private MushroomHitbox hitbox;

    public void EnableDamage()
    {
        Debug.Log("EnableDamage called");
        Debug.Log("Hitbox reference: " + hitbox);
        hitbox.CanDamage = true;
        Debug.Log("hitbox.canDamage: " + hitbox.CanDamage);
    }

    public void DisableDamage()
    {
        Debug.Log("DisableDamage called");
        hitbox.CanDamage = false;
    }
}