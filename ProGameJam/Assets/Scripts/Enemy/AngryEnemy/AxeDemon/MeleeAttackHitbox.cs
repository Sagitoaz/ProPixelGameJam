using UnityEngine;

public class _meleeAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamageable player = collision.GetComponent<IDamageable>();
            player.Damage();
            Debug.Log("Melee hit: " + collision.name);
        }
    }
}