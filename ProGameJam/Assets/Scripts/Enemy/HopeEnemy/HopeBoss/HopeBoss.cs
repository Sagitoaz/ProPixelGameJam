using System.Collections;
using UnityEngine;

public class HopeBoss : Enemy, IDamageable
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _hitbox;
    private Rigidbody2D _rb;
    protected bool _isActivated = false;
    protected bool _moveRight = true;
    protected bool _canFlip = true;
    protected bool _isIdle = false;
    protected bool _isAttack = false;
    protected bool _isDead = false;
    protected Transform _target;
    protected Coroutine _attackCoroutine;
    protected bool _isChasing = false;
    [SerializeField] private float _teleportDistance; // Khoảng cách dịch chuyển phía sau player
    [SerializeField] private float _teleportInterval; // Khoảng thời gian giữa các lần dịch chuyển tự động
    private float _teleportCooldownTimer = 0f; // Theo dõi thời gian cooldown
    public int Health { get; set; }

    public Animator Animator => anim;
    protected void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        _teleportCooldownTimer = _teleportInterval; // Cho phép dịch chuyển ngay lần đầu
        StartCoroutine(PeriodicTeleportRoutine());
    }

    protected void Update()
    {
        if (_isDead || !_isActivated) return;
        // Cập nhật cooldown timer
        if (_teleportCooldownTimer < _teleportInterval)
        {
            _teleportCooldownTimer += Time.deltaTime;
        }
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
        if (_isIdle || _isAttack) return;

        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        Vector2 rayPosition;
        if (_moveRight)
        {
            rayPosition = new Vector2(originPosition.x + 1.0f, originPosition.y);
        }
        else
        {
            rayPosition = new Vector2(originPosition.x - 1.0f, originPosition.y);
        }
        bool checkGround = Physics2D.Raycast(rayPosition, Vector2.down, 5.0f, _groundLayer);
        Debug.DrawRay(rayPosition, Vector2.down * 5.0f, Color.green);
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

    public void speedUp(float multiSpeed)
    {
        speed *= 2;
    }

    public void speedDown(float multiSpeed)
    {
        speed /= 2;
    }

    private void MoveToTarget()
    {
        if (_target == null || _isAttack) return;

        Vector2 originPosition = transform.position;
        Vector2 directionToTarget = _target.position - transform.position;
        bool moveRight = directionToTarget.x > 0;
        Vector2 moveDirection = moveRight ? Vector2.right : Vector2.left;
        float distanceX = Mathf.Abs(directionToTarget.x);

        if (distanceX < 0.1f)
        {
            anim.SetBool("Moving", false);
            return;
        }
        Vector2 rayPosition;
        if (_moveRight)
        {
            rayPosition = new Vector2(originPosition.x + 1.0f, originPosition.y);
        }
        else
        {
            rayPosition = new Vector2(originPosition.x - 1.0f, originPosition.y);
        }
        bool checkGround = Physics2D.Raycast(rayPosition, Vector2.down, 5.0f, _groundLayer);
        bool checkWall = Physics2D.Raycast(rayPosition, moveDirection, 1.0f, _groundLayer);
        Debug.DrawRay(rayPosition, Vector2.down * 5.0f, Color.green);
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
        if (_isAttack) return;

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
        _canFlip = true;
        _isIdle = false;
        anim.SetBool("Moving", true);
    }

    public void Damage()
    {
        if (!_isAttack && !_isDead)
        {
            Health--;
            Debug.Log("Health point lefts: " + Health);
            anim.SetTrigger("Hit");
            TeleportBehindTarget();
            if (Health < 1)
            {
                _isDead = true;
                StopAllCoroutines();
                StartCoroutine(DeathRoutine());
            }
        }
        else
        {
            Debug.Log("Boss is attacking or dead, no damage taken!");
        }
    }

    public void TeleportBehindTarget()
    {
        if (_teleportCooldownTimer < _teleportInterval)
        {
            Debug.Log($"Teleport skipped: Cooldown not finished. Time remaining: {(_teleportInterval - _teleportCooldownTimer):F2}s");
            return;
        }

        if (_target == null)
        {
            Debug.LogWarning("Teleport failed: _target is null!");
            return;
        }

        Vector3 playerDirection = _target.position - transform.position;
        float directionSign = playerDirection.x > 0 ? -1f : 1f;
        Vector3 teleportOffset = new Vector3(directionSign * _teleportDistance, 0, 0);
        Vector3 teleportPosition = _target.position + teleportOffset;

        Debug.Log($"Teleport target position: {teleportPosition}, Distance: {_teleportDistance}");

        Vector2 rayPosition = new Vector2(teleportPosition.x, teleportPosition.y);
        RaycastHit2D groundHit = Physics2D.Raycast(rayPosition, Vector2.down, 5.0f, _groundLayer);
        RaycastHit2D wallRightHit = Physics2D.Raycast(rayPosition, Vector2.right, 1.0f, _groundLayer);
        RaycastHit2D wallLeftHit = Physics2D.Raycast(rayPosition, Vector2.left, 1.0f, _groundLayer);

        bool isGrounded = groundHit.collider != null;
        bool isBlocked = wallRightHit.collider != null || wallLeftHit.collider != null;

        Debug.Log($"Teleport check - Grounded: {isGrounded}, Blocked: {isBlocked}, GroundHit: {groundHit.collider?.name}, WallRight: {wallRightHit.collider?.name}, WallLeft: {wallLeftHit.collider?.name}");

        if (isGrounded && !isBlocked)
        {
            transform.position = teleportPosition;
            Debug.Log("Boss teleported behind player!");
            _teleportCooldownTimer = 0f; // Reset cooldown
            if (playerDirection.x > 0 && _moveRight)
            {
                Flip();
            }
            else if (playerDirection.x < 0 && !_moveRight)
            {
                Flip();
            }
        }
        else
        {
            for (float offset = 0.5f; offset <= _teleportDistance; offset += 0.5f)
            {
                Vector3 altTeleportPosition = _target.position + new Vector3(directionSign * (_teleportDistance - offset), 0, 0);
                rayPosition = new Vector2(altTeleportPosition.x, altTeleportPosition.y);
                groundHit = Physics2D.Raycast(rayPosition, Vector2.down, 5.0f, _groundLayer);
                wallRightHit = Physics2D.Raycast(rayPosition, Vector2.right, 1.0f, _groundLayer);
                wallLeftHit = Physics2D.Raycast(rayPosition, Vector2.left, 1.0f, _groundLayer);

                isGrounded = groundHit.collider != null;
                isBlocked = wallRightHit.collider != null || wallLeftHit.collider != null;

                if (isGrounded && !isBlocked)
                {
                    transform.position = altTeleportPosition;
                    Debug.Log($"Boss teleported to alternative position: {altTeleportPosition}");
                    _teleportCooldownTimer = 0f; // Reset cooldown
                    FaceTarget();
                    if (playerDirection.x > 0 && _moveRight)
                    {
                        Flip();
                    }
                    else if (playerDirection.x < 0 && !_moveRight)
                    {
                        Flip();
                    }
                    return;
                }
            }
            Debug.LogWarning("Teleport failed: No valid position found!");
        }
    }

    private IEnumerator PeriodicTeleportRoutine()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(_teleportInterval);
            if (!_isDead && !_isAttack) // Chỉ dịch chuyển nếu không tấn công
            {
                anim.SetTrigger("Teleport");
            }
        }
    }

    public virtual IEnumerator DeathRoutine()
    {
        anim.SetTrigger("Death");
        _isAttack = false;
        _isChasing = false;
        _isIdle = false;
        _target = null;
        _hitbox.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        // Animation Event sẽ xử lý hủy GameObject
    }

    public virtual void StartAttack(Transform player)
    {
        if (_isDead) return;

        anim.SetBool("Moving", false);
        _isAttack = true;
        _isIdle = false;
        _isChasing = false;
        _target = player;
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }

    public virtual void StopAttack()
    {
        if (_isDead) return;

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
        if (_isAttack || _isDead) return;
        _isActivated = true;
        _isChasing = true;
        _isAttack = false;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", true);
    }

    public virtual void StopChase()
    {
        if (_isAttack || _isDead) return;

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

    public void ActivateHitbox()
    {
        _hitbox.SetActive(true);
    }

    public void DeactivateHitbox()
    {
        _hitbox.SetActive(false);
    }

    public virtual IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_isAttack)
            {
                Debug.Log("Triggering Attack animation");
                anim.SetTrigger("Attack");
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}