using System.Collections;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private GameObject PlayerIngame;
    private Player _playerScript;
    [SerializeField] private bool _isUltimate;
    void Start()
    {
        _playerScript = PlayerIngame.GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable enemy = collision.GetComponent<IDamageable>();
        if (enemy != null) {
            Debug.Log("Hit: " + collision.name);
            enemy.Damage();
            _playerScript.SetMana(_playerScript.GetMana() + 1);
            if (_isUltimate)
            {
                enemy.Damage();
                _playerScript.SetMana(_playerScript.GetMana() + 1);
            }
        }
    }
}
