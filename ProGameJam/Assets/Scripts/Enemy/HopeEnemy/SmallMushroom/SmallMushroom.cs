using System.Collections;
using UnityEngine;

public class SmallMushroom : Enemy, IDamageable
{
    [SerializeField] private GameObject _hitbox;
    protected Coroutine _attackCoroutine;
    protected bool _isChasing = false;
    public int Health { get; set; }

    protected void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        if (_hitbox != null)
        {
            _hitbox.SetActive(false);
        }
    }
    protected void Update()
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
    protected override void Patrol()
    {
        if (_isIdle)
        {
            return;
        }
        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        Vector2 rayPosition;
        if (_moveRight) {
            rayPosition = new Vector2(originPosition.x + 0.2f, originPosition.y);
        }
        else{
            rayPosition = new Vector2(originPosition.x - 0.2f, originPosition.y);
        }
        bool checkGround = Physics2D.Raycast(rayPosition, Vector2.down, 1.5f, _groundLayer); // Giảm xuống 1.5f
        Debug.DrawRay(rayPosition, Vector2.down * 1.5f, Color.green);
        bool checkWall = Physics2D.Raycast(rayPosition, direction, 1.0f, _groundLayer);
        Debug.DrawRay(rayPosition, direction, Color.red);
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
    protected override void MoveToTarget()
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
        Vector2 rayPosition;
        if (_moveRight) {
            rayPosition = new Vector2(originPosition.x + 0.2f, originPosition.y);
        }
        else{
            rayPosition = new Vector2(originPosition.x - 0.2f, originPosition.y);
        }
        bool checkGround = Physics2D.Raycast(rayPosition, Vector2.down, 1.5f, _groundLayer);
        bool checkWall = Physics2D.Raycast(rayPosition, moveDirection, 1.0f, _groundLayer);
        Debug.DrawRay(rayPosition, Vector2.down * 1.5f, Color.green);
        Debug.DrawRay(rayPosition, moveDirection * 1.0f, Color.red);
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
    protected override void Move()
    {
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        float moveDistance = speed * Time.deltaTime;
        if (moveDistance > 0.1f) moveDistance = 0.1f; // Giới hạn di chuyển
        transform.Translate(direction * moveDistance);
        anim.SetBool("Moving", true);
    }
    public void Damage()
    {
        if (_isDead) return;
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("Hit");
        if (Health < 1)
        {
            _isDead = true;
            OnEnemyDeath?.Invoke();
            Destroy(gameObject);
        }
    }
    public virtual void StartAttack(Transform player)
    {
        _isAttack = true;
        _isIdle = false;
        _isChasing = false;
        _target = player;
        anim.SetBool("Moving", false);
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }
    public virtual void StopAttack()
    {
        _isAttack = false;
        _isIdle = false;
        anim.SetBool("Moving", true);
        _hitbox.SetActive(false);
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
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
    public virtual IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_isAttack)
            {
                Debug.Log("Triggering Attack animation");
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                _hitbox.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                _hitbox.SetActive(false);
            }
            yield return new WaitForSeconds(1.0f); // Khoảng nghỉ giữa các đợt tấn công
        }
    }
}
