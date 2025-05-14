using System.Collections;
using UnityEngine;

public class FireWorm : Enemy, IDamageable
{
    [SerializeField] private GameObject _firePrefab;
    private Coroutine _attackCoroutine;
    protected bool _isChasing = false;

    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
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
        if (_isDead) return;
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("Hit");
        if (Health < 1) {
            anim.SetTrigger("Death");
            _isDead = true;
            OnEnemyDeath?.Invoke();
            Destroy(gameObject);
        }
    }
    public void StartAttack(Transform player) {
        _isAttack = true;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", false);
        if (_attackCoroutine == null) {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }
    public void StopAttack() {
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
    IEnumerator AttackRoutine() {
        while (true) {
            if (_isAttack) {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.55f);
                SpawnFire();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    private void SpawnFire() {
        if (_firePrefab != null && _target != null) {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y + 0.4005f);
            GameObject fire = Instantiate(_firePrefab, spawnPosition, Quaternion.identity);
            Vector2 direction = _target.position - transform.position;
            fire.GetComponent<Fireball>().SetDirection(direction);
        }
    }
}