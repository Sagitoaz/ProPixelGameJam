using System.Collections;
using UnityEngine;

public class MushroomAttackHitbox : MonoBehaviour
{
    [SerializeField] private MushroomHitbox hitbox;

    public void EnableDamage()
    {
        hitbox.canDamage = true;
    }

    public void DisableDamage()
    {
        hitbox.canDamage = false;
    }
}