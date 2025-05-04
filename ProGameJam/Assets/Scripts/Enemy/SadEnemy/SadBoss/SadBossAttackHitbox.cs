using System.Collections;
using UnityEngine;

public class SadBossAttackHitbox : MonoBehaviour
{
    [SerializeField] private SadBossHitbox hitbox;

    public void EnableDamage()
    {
        hitbox.canDamage = true;
    }

    public void DisableDamage()
    {
        hitbox.canDamage = false;
    }
}