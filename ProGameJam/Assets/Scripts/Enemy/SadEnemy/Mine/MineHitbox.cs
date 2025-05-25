using UnityEngine;

public class MineHitbox : MonoBehaviour
{
    [SerializeField] private float _attackCoolDown = 0.5f;
    private float _lastAttack = 0f;
    void OnTriggerStay2D(Collider2D collision)
    {
        DamageTo(collision);
    }
    private void DamageTo(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (Time.time - _lastAttack >= _attackCoolDown) {
                IDamageable player = collision.GetComponent<IDamageable>();
                player.Damage();
                Debug.Log("Hitbox hit: " + collision.name);
                _lastAttack = Time.time;
            }
        }
    }
}