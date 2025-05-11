using System.Collections;
using UnityEngine;

public class FearBoss : Enemy, IDamageable, IAttackableEnemy
{
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private float invisibleTime;
    private Coroutine _attackCoroutine;
    private bool _isDead = false;
    private bool _isActive = false;
    private bool _isPerformingAction = false;
    private Coroutine _invisibleRoutine;
    private bool _isVisible = true;

    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        _invisibleRoutine = StartCoroutine(InvisibleCycle());

        _moveRight = true;
        sprite.flipX = false;
    }
    void Update()
    {
        if (!_isActive || _isDead || _isPerformingAction) return;

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
        if (_isIdle)
        {
            return;
        }
        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 2.0f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        bool checkWall = Physics2D.Raycast(originPosition, direction, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, direction, Color.red);
        if (!checkGround || checkWall)
        {
            if (_canFlip)
            {
                StartCoroutine(IdleToFlip());
            }
            return;
        }
        Move();
    }
    protected override void Move()
    {
        if (_isPerformingAction) return;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);
        anim.SetBool("Run", true);
    }
    protected override IEnumerator IdleToFlip()
    {
        _isIdle = true;
        anim.SetBool("Run", false);
        yield return new WaitForSeconds(1.0f);
        Flip();
        _canFlip = false;
        Move();
        _canFlip = true;
        _isIdle = false;
        anim.SetBool("Run", true);
    }

    public void Damage()
    {
        if (_isDead) return;

        Health--;
        Debug.Log("Health point lefts: " + Health);
        if (!_isVisible)
        {
            SetVisible();
        }
        if (Health < 1)
        {
            _isDead = true;
            anim.SetTrigger("Death");
            _isAttack = false;
            _isIdle = true;
            _canFlip = false;
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }
        else
        {
            StartCoroutine(TakeHitRoutine());
            if (_invisibleRoutine != null)
            {
                StopCoroutine(_invisibleRoutine);
                _invisibleRoutine = StartCoroutine(InvisibleCycle());
            }
        }
    }

    public void StartAttack(Transform player)
    {
        _isAttack = true;
        _isIdle = false;
        anim.SetBool("Run", false);
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }
    public void StopAttack()
    {
        _isAttack = false;
        _isIdle = false;
        anim.SetBool("Run", true);
    }
    IEnumerator AttackRoutine()
    {
        bool useAttack1 = true;

        while (true)
        {
            if (_isAttack)
            {
                _isPerformingAction = true;
                _hitbox.SetActive(false);

                if (useAttack1)
                {
                    anim.SetTrigger("Attack1");
                }
                else
                {
                    anim.SetTrigger("Attack2");
                }

                useAttack1 = !useAttack1;

                yield return new WaitForSeconds(0.5f);
                _hitbox.SetActive(true);
                _isPerformingAction = false;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator TakeHitRoutine()
    {
        _isPerformingAction = true;
        anim.SetTrigger("TakeHit");
        yield return new WaitForSeconds(0.5f);
        _isPerformingAction = false;
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    IEnumerator InvisibleCycle()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(invisibleTime);

            if (!_isVisible)
                continue;

            SetInvisible();
        }
    }

    private void SetInvisible()
    {
        _isVisible = false;
        Color color = sprite.color;
        color.a = 0.01f; // mờ
        sprite.color = color;
    }

    private void SetVisible()
    {
        _isVisible = true;
        Color color = sprite.color;
        color.a = 1f; // rõ lại
        sprite.color = color;
    }

    public void ActivateBoss(Transform player)
    {
        _isActive = true;
        _target = player;
        if (!_isAttack)
        {
            StopAttack();
        }
        _isIdle = false;
        anim.SetBool("Run", true);
    }

    public void DeactivateBoss()
    {
        if (!_isActive) return;

        _isActive = false;
        _target = null;
        _isAttack = false;
        _isIdle = true;
        _isDead = false;
        _isPerformingAction = false;
        anim.SetBool("Run", false);

        Health = base.health;

        anim.Rebind();
    }

}
