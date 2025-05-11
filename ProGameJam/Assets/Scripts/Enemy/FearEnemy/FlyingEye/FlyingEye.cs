using System.Collections;
using UnityEngine;

public class FlyingEye : Enemy, IDamageable, IAttackableEnemy
{
    private bool _isDead = false;
    private Coroutine _attackCoroutine;
    public int Health { get; set; }
    [SerializeField] private float flyDistance = 5f;
    [SerializeField] private GameObject _hitbox;
    private Vector3 _startPos;
    private Vector3 _leftLimit;
    private Vector3 _rightLimit;

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();

        _startPos = transform.position;
        _leftLimit = _startPos - new Vector3(flyDistance / 2f, 0, 0);
        _rightLimit = _startPos + new Vector3(flyDistance / 2f, 0, 0);
    }

    void Update()
    {
        if (!_isAttack)
        {
            Patrol();
        }
        else
        {
            FaceTarget();
        }
    }

    protected override void Patrol()
    {
        if (_isIdle) return;

        Vector3 targetPos = _moveRight ? _rightLimit : _leftLimit;
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
        anim.SetBool("Walk", true);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            StartCoroutine(IdleToFlipDirection());
        }
    }

    IEnumerator IdleToFlipDirection()
    {
        _isIdle = true;
        anim.SetBool("Walk", false);
        yield return new WaitForSeconds(1.0f);
        Flip();
        _isIdle = false;
        anim.SetBool("Walk", true);
    }
    public void Damage()
    {
        if (_isDead) return;
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("TakeHit");
        if (Health < 1)
        {
            _isDead = true;
            anim.SetTrigger("Death");
            _isAttack = false;
            _isIdle = true;
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }
    }

    public void StartAttack(Transform player)
    {
        _isAttack = true;
        _isIdle = false;
        _target = player;
        anim.SetBool("Walk", false);
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    public void StopAttack()
    {
        _isAttack = false;
        _isIdle = false;
        anim.SetBool("Walk", true);
    }
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_isAttack)
            {
                _hitbox.SetActive(false);
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                _hitbox.SetActive(true);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
    
}
