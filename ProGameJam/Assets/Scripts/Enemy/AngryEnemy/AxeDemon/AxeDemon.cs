using System.Collections;
using UnityEngine;

public class AxeDemon : Enemy, IDamageable
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _meleeAttackHitbox;

    private CircleCollider2D _meleeCollider;
    private Rigidbody2D _rb;
    private bool _moveRight = true;
    private bool _canFlip = true;
    private bool _isIdle = false;
    private bool _isAttack = false;
    private Transform _target;
    private Coroutine _attackCoroutine;

    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        _meleeCollider = _meleeAttackHitbox.GetComponent<CircleCollider2D>();
        // _meleeAttackHitbox.SetActive(false);
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
        if (_isIdle)
        {
            return;
        }
        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        bool checkWall = Physics2D.Raycast(originPosition, direction, 1.0f, _groundLayer);
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
    private void Move()
    {
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);
        anim.SetBool("Moving", true);
    }
    private void Flip()
    {
        _moveRight = !_moveRight;
        sprite.flipX = !sprite.flipX;
        if (_meleeAttackHitbox != null){
            Vector2 _currentOffset = _meleeCollider.offset;
            _meleeCollider.offset = new Vector2(-_currentOffset.x, _currentOffset.y);
        }
    }
    IEnumerator IdleToFlip()
    {
        _isIdle = true;
        anim.SetBool("Moving", false);
        yield return new WaitForSeconds(1.0f);
        Flip();
        _canFlip = false;
        Move();
        yield return new WaitForSeconds(1.0f);
        _canFlip = true;
        _isIdle = false;
        anim.SetBool("Moving", true);
    }

    public void Damage()
    {
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("Hit");
        if (Health < 1)
        {
            anim.SetTrigger("Death");
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
                _meleeAttackHitbox.SetActive(true);
                anim.SetTrigger("Attack");
                // yield return new WaitForSeconds(0.5f);
                _meleeAttackHitbox.SetActive(false);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
