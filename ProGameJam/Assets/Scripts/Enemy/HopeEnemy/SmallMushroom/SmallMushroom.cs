using System.Collections;
using UnityEngine;

public class SmallMushroom : Enemy, IDamageable
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _hitbox;
    private Rigidbody2D _rb;
    protected bool _moveRight = true;
    protected bool _canFlip = true;
    protected bool _isIdle = false;
    protected bool _isAttack = false;
    protected Transform _target;
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
    private void Patrol()
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
    private void Move()
    {
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        float moveDistance = speed * Time.deltaTime;
        if (moveDistance > 0.1f) moveDistance = 0.1f; // Giới hạn di chuyển
        transform.Translate(direction * moveDistance);
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
