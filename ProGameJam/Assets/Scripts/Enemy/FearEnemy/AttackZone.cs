using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _enemy;
    private IAttackableEnemy _attackableEnemy;
    private void Awake()
    {
        _attackableEnemy = _enemy as IAttackableEnemy;
        if (_attackableEnemy == null)
        {
            Debug.Log("Enemy not implement IAttackableEnemy!");
        }
        else
        {
            Debug.Log("Enemy implement IAttackableEnemy successfully!");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _attackableEnemy?.StartAttack(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _attackableEnemy?.StopAttack();
        }
    }
}
