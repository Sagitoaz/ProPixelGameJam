using UnityEngine;

public class _meleeAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // damageable.Damage();
            Debug.Log("Melee hit: " + collision.name);
        }
    }
}