using UnityEngine;

public interface IAttackableEnemy
{
    void StartAttack(Transform player);
    void StopAttack();
}
