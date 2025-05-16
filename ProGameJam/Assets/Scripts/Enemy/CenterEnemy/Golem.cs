using System.Collections;
using UnityEngine;

public class Golem : Enemy, IDamageable
{
    [SerializeField] protected GameObject _firePrefab;
    protected Coroutine _attackCoroutine;
    protected bool _isChasing = false;
    public int Health { get; set; }

    protected void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
    }
    protected void Update()
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
        if (_isDead) return;
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("Hit");
        if (Health < 1) {
            SpawnCoin();
            anim.SetTrigger("Death");
            _isDead = true;
            OnEnemyDeath?.Invoke();
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }
    }
    public virtual void StartAttack(Transform player) {
        _isAttack = true;
        _isIdle = false;
        _isChasing = false;
        _target = player;
        anim.SetBool("Moving", false);
        if (_attackCoroutine == null) {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }
    public virtual void StopAttack() {
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
    public virtual IEnumerator AttackRoutine() {
        while (true) {
            if (_isAttack) {
                anim.SetTrigger("Attack");
                SpawnFire();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    private void SpawnFire() {
        if (_firePrefab != null && _target != null) {
            GameObject fire = Instantiate(_firePrefab, transform.position, Quaternion.identity);
            Vector2 direction = _target.position - transform.position;
            fire.GetComponent<Fire>().SetDirection(direction);
        }
    }
    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
