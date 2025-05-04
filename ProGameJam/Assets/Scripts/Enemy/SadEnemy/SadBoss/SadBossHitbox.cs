using UnityEngine;

public class SadBossHitbox : MonoBehaviour
{
    [SerializeField] private float _attackCoolDown = 0.5f;
    private float _lastAttack = 0f;
    [HideInInspector] public bool canDamage = false;
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     DamageTo(collision);
    // }
    void OnTriggerStay2D(Collider2D collision)
    {
        DamageTo(collision);
    }
    private void DamageTo(Collider2D collision) {
        if (!canDamage) return;
        if (collision.CompareTag("Player"))
        {
            if (Time.time - _lastAttack >= _attackCoolDown)
            {
                Debug.Log("Hitbox hit: " + collision.name);
                _lastAttack = Time.time;
            }
        }
    }
}