using System.Collections;
using UnityEngine;

public class Golem1 : Golem
{
    [SerializeField] private GameObject _hitbox;
    public override void StartAttack(Transform player)
    {
        base.StartAttack(player);
    }
    public override void StopAttack()
    {
        base.StopAttack();
    }
    public override IEnumerator AttackRoutine() {
        while (true) {
            if (_isAttack) {
                _hitbox.SetActive(true);
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                _hitbox.SetActive(false);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
