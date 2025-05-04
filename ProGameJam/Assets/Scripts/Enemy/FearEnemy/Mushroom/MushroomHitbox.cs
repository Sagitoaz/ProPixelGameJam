using UnityEngine;
using System.Collections;

public class MushroomHitbox : MonoBehaviour
{
    [SerializeField] private float _attackCoolDown = 0.5f;
    private float _lastAttack = 0f;
    private bool _canDamage = false; // Đổi tên biến nội bộ

    public bool CanDamage
    {
        get => _canDamage;
        set => _canDamage = value;
    }
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     DamageTo(collision);
    // }
    void OnTriggerStay2D(Collider2D collision)
    {
        DamageTo(collision);
    }
    private void DamageTo(Collider2D collision)
    {
        Debug.Log("value: " + _canDamage);
        if (collision.CompareTag("Player"))
        {
            if (Time.time - _lastAttack >= _attackCoolDown)
            {
                IDamageable player = collision.GetComponent<IDamageable>();
                player.Damage();
                Debug.Log("Hitbox hit: " + collision.name);
                _lastAttack = Time.time;
            }
        }
    }

}