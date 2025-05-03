using System.Collections;
using UnityEngine;

public class FlyingEye : Enemy, IDamageable, IAttackableEnemy
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody2D _rb;
    private bool _moveRight = true;
    private bool _isIdle = false;
    private bool _isAttack = false;
    private bool _isDead = false;
    private Transform _target;
    private Coroutine _attackCoroutine;
    public int Health { get; set; }
    [SerializeField] private float flyDistance = 5f;
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

    private void Patrol()
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

    private void Flip()
    {
        _moveRight = !_moveRight;
        sprite.flipX = !sprite.flipX;
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

    private void FaceTarget()
    {
        if (_target == null) return;
        if (_target.position.x > transform.position.x && !_moveRight)
        {
            Flip();
        }
        else if (_target.position.x < transform.position.x && _moveRight)
        {
            Flip();
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_isAttack)
            {
                anim.SetTrigger("Attack");
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
