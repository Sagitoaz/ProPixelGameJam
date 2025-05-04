using UnityEngine;

public class _meleeAttackHitbox : MonoBehaviour
{
    [SerializeField] private float _attackCoolDown = 0.5f;
    private float _lastAttack = 0f;
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     DamageTo(collision);
    // }
    void OnTriggerStay2D(Collider2D collision)
    {
        DamageTo(collision);
    }
    private void DamageTo(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (Time.time - _lastAttack >= _attackCoolDown) {
                Debug.Log("Golem1 hit: " + collision.name);
                IDamageable player = collision.GetComponent<IDamageable>();
                player.Damage();
                _lastAttack = Time.time;
            }
        }
    }
}