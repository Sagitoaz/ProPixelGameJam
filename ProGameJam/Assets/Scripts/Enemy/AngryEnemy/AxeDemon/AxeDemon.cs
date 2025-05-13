using System.Collections;
using UnityEngine;

public class AxeDemon : Enemy, IDamageable
{
    [SerializeField] private GameObject _meleeAttackHitbox;
    private CircleCollider2D _meleeCollider;
    private Coroutine _attackCoroutine;
    private bool _isChasing = false;
    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        _meleeCollider = _meleeAttackHitbox.GetComponent<CircleCollider2D>();
        _meleeAttackHitbox.SetActive(false);
    }
   void Update()
    {
        if (_isAttack) {
            FaceTarget();
        } else if (_isChasing) {
            MoveToTarget();
        } else {
            Patrol();
        }
    }
    public void Damage()
    {
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("Hit");
        if (Health < 1)
        {
            anim.SetTrigger("Death");
            _isDead = true;
            OnEnemyDeath?.Invoke();
            Destroy(gameObject);
        }
    }
    public void StartAttack(Transform player)
    {
        _isAttack = true;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", false);
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }
    public void StopAttack()
    {
        _isAttack = false;
        _isIdle = false;
        anim.SetBool("Moving", true);
    }
    public virtual void StartChase(Transform player) {
        if (_isAttack) return;
        _isChasing = true;
        _isAttack = false;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", true);
    }
    public virtual void StopChase() {
        if (_isAttack) return;
        _isChasing = false;
        _target = null;
        anim.SetBool("Moving", false);
    }
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_isAttack)
            {
                _meleeAttackHitbox.SetActive(true);
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                _meleeAttackHitbox.SetActive(false);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
