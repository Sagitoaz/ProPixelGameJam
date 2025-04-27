using System.Collections;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable enemy = collision.GetComponent<IDamageable>();
        if (enemy != null) {
            Debug.Log("Hit: " + collision.name);
            enemy.Damage();
        }
    }
}
