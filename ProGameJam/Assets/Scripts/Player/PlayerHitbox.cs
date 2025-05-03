using System.Collections;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private bool _isUltimate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable enemy = collision.GetComponent<IDamageable>();
        if (enemy != null) {
            Debug.Log("Hit: " + collision.name);
            enemy.Damage();
            if (_isUltimate) {
                enemy.Damage();
            }
        }
    }
}
