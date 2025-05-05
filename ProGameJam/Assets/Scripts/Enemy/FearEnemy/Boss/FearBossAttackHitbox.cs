using System.Collections;
using UnityEngine;

public class FearBossAttackHitbox : MonoBehaviour
{
    [SerializeField] private FearBossHitbox hitbox;

    public void EnableDamage()
    {
        hitbox.canDamage = true;
    }

    public void DisableDamage()
    {
        hitbox.canDamage = false;
    }
}