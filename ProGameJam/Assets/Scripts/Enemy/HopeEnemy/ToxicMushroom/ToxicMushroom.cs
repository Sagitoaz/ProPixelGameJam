using System.Collections;
using UnityEngine;

public class ToxicMushroom : Enemy, IDamageable
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody2D _rb;
    private bool _moveRight = true;
    private bool _canFlip = true;
    private bool _isIdle = false;
    private bool _isAttack = false;
    private Transform _target;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private GameObject _pointA;
    [SerializeField] private GameObject _pointB;
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
        if (_isAttack)
        {
            FaceTarget();
        }
        else if (_isChasing)
        {
            MoveToTarget();
        }
        else
        {
            Patrol();
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
        // bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 3.0f, _groundLayer);
        // Debug.DrawRay(transform.position, Vector2.down, Color.green);
        // bool checkWall = Physics2D.Raycast(originPosition, direction, 1.0f, _groundLayer);
        // Debug.DrawRay(transform.position, direction, Color.red);
        if (_pointA != null && _pointB != null)
        {
            float distanceToA = Vector2.Distance(transform.position, _pointA.transform.position);
            float distanceToB = Vector2.Distance(transform.position, _pointB.transform.position);
            if (_canFlip && (distanceToA < 1.0f || distanceToB < 1.0f))
            {
                StartCoroutine(IdleToFlip());
                return;
            }
        }
        Move();
    }
    private void MoveToTarget()
    {
        if (_target == null)
        {
            return;
        }
        Vector2 originPosition = transform.position;
        Vector2 directionToTarget = _target.position - transform.position;
        bool moveRight = directionToTarget.x > 0;
        Vector2 moveDirection = moveRight ? Vector2.right : Vector2.left;
        float distanceX = Mathf.Abs(directionToTarget.x); // Khoảng cách theo trục X

        if (distanceX < 0.1f) // Nếu rất gần thì đứng lại
        {
            anim.SetBool("Moving", false);
            return;
        }
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 1.5f, _groundLayer);
        bool checkWall = Physics2D.Raycast(originPosition, moveDirection, 1.0f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.green);
        Debug.DrawRay(transform.position, moveDirection * 1.0f, Color.red);
        if (!checkGround || checkWall)
        {
            anim.SetBool("Moving", false);
            return;
        }
        if (moveRight != _moveRight)
        {
            Flip();
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
    }
    IEnumerator IdleToFlip()
    {
        _isIdle = true;
        anim.SetBool("Moving", false);
        yield return new WaitForSeconds(1.0f);
        Flip();
        _canFlip = false;
        Move();
        // yield return new WaitForSeconds(1.0f);
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
        anim.SetBool("Moving", false);
        _isIdle = false;
        _isAttack = true;
        _target = player;
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
    public virtual void StartChase(Transform player)
    {
        if (_isAttack) return;
        _isChasing = true;
        _isAttack = false;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", true);
    }
    public virtual void StopChase()
    {
        if (_isAttack) return;
        _isChasing = false;
        _target = null;
        anim.SetBool("Moving", false);
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
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    public void SpawnFire()
    {
        if (_firePrefab != null && _target != null)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
            GameObject fire = Instantiate(_firePrefab, spawnPosition, Quaternion.identity);
            Vector2 direction = _target.position - transform.position;
            fire.GetComponent<Toxicball>().SetDirection(direction);
        }
    }
}