using System.Collections;
using UnityEngine;

public class HopeBoss : Enemy, IDamageable
{
    [SerializeField] private GameObject _hitbox;
    protected bool _isDead = false;
    protected Coroutine _attackCoroutine;
    protected bool _isChasing = false;
    [SerializeField] private float _teleportDistance = 3.0f;
    private bool _isActivated = false;
    public int Health { get; set; }
    public Animator Animator => anim;

    protected void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        anim.SetBool("Moving", false);
        if (anim == null)
        {
            Debug.LogError("HopeBoss: Animator (anim) is null!", this);
        }
    }

    protected void Update()
    {
        if (_isDead || !_isActivated) return;

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
        if (_isIdle || _isAttack) return;

        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        Vector2 rayPosition = _moveRight ? new Vector2(originPosition.x + 1.0f, originPosition.y) : new Vector2(originPosition.x - 1.0f, originPosition.y);
        bool checkGround = Physics2D.Raycast(rayPosition, Vector2.down, 5.0f, _groundLayer);
        bool checkWall = Physics2D.Raycast(rayPosition, direction, 1.0f, _groundLayer);
        Debug.DrawRay(rayPosition, Vector2.down * 5.0f, Color.green);
        Debug.DrawRay(rayPosition, direction * 1.0f, Color.red);

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
        speed *= multiSpeed;
        Debug.Log($"Speed increased to: {speed}");
    }

    public void speedDown(float multiSpeed)
    {
        speed /= multiSpeed;
        Debug.Log($"Speed decreased to: {speed}");
    }

    protected override void MoveToTarget()
    {
        if (_target == null || _isAttack)
        {
            Debug.LogWarning("MoveToTarget failed: _target is null or boss is attacking!");
            _isChasing = false;
            return;
        }

        Vector2 originPosition = transform.position;
        Vector2 directionToTarget = _target.position - transform.position;
        bool moveRight = directionToTarget.x > 0;
        float distanceX = Mathf.Abs(directionToTarget.x);

        if (distanceX < 0.1f)
        {
            anim.SetBool("Moving", false);
            return;
        }

        Vector2 rayPosition = moveRight ? new Vector2(originPosition.x + 1.0f, originPosition.y) : new Vector2(originPosition.x - 1.0f, originPosition.y);
        bool checkGround = Physics2D.Raycast(rayPosition, Vector2.down, 5.0f, _groundLayer);
        bool checkWall = Physics2D.Raycast(rayPosition, moveRight ? Vector2.right : Vector2.left, 1.0f, _groundLayer);
        Debug.DrawRay(rayPosition, Vector2.down * 5.0f, Color.green);
        Debug.DrawRay(rayPosition, (moveRight ? Vector2.right : Vector2.left) * 1.0f, Color.red);

        if (!checkGround || checkWall)
        {
            anim.SetBool("Moving", false);
            Debug.Log($"MoveToTarget stopped: Grounded={checkGround}, Blocked={checkWall}");
            return;
        }

        if (moveRight != _moveRight)
        {
            Flip();
        }

        Move();
    }
    protected override IEnumerator IdleToFlip()
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
            Debug.Log($"Health point lefts: {Health}");
            anim.SetTrigger("Hit");
            TryTeleportBehindTarget();
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

    public void TryTeleportBehindTarget()
    {
        if (!_isActivated)
        {
            Debug.Log("Teleport skipped: Boss is not activated!");
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
            yield return new WaitForSeconds(5.0f);
            if (!_isDead && !_isAttack && _isActivated)
            {
                anim.SetTrigger("Teleport");
            }
        }
    }

    public virtual IEnumerator DeathRoutine()
    {
        _isAttack = false;
        _isChasing = false;
        _isIdle = false;
        _target = null;
        _hitbox.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Death");
    }

    public virtual void StartAttack(Transform player)
    {
        if (_isDead || !_isActivated) return;

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

        Debug.Log($"StartChase: Chasing player {player.name}");
        _isActivated = true;
        _isChasing = true;
        _isAttack = false;
        _isIdle = false;
        _target = player;
        anim.SetBool("Moving", true);
        anim.SetTrigger("Teleport");
        StartCoroutine(PeriodicTeleportRoutine());
    }

    public virtual void StopChase()
    {
        if (_isAttack || _isDead) return;

        Debug.Log("StopChase: Stopping chase");
        _isChasing = false;
        _target = null;
        anim.SetBool("Moving", false);
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