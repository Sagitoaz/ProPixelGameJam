using System.Collections;
using UnityEngine;

public class SadBoss : Enemy, IDamageable, IAttackableEnemy, IDataPersistence
{
    private Coroutine _attackCoroutine;
    [SerializeField] private GameObject _hitbox;
    private bool _isActive = false;
    private bool _isPerformingAction = false;
    [SerializeField] private float teleportInterval = 5f;
    [SerializeField] private float teleportDistance = 2f;
    private Coroutine _teleportCoroutine;
    [SerializeField] private float fallSpeed = 5f;
    private bool _isFalling = false;
    private bool _isGrounded = false;
    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
        _moveRight = false;
        sprite.flipX = true;
    }
    void Update()
    {
        if (!_isActive || _isDead || _isPerformingAction) return;

        if (_isFalling)
        {
            anim.SetBool("Fall", _isFalling);
            return;
        }

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
            if (!_isFalling && checkGround == false)
            {
                StartFalling();
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

    private void StartFalling()
    {
        if (_isFalling || _isDead) return;

        _isFalling = true;
        anim.SetBool("Fall", _isFalling);
        StartCoroutine(FallRoutine());
    }

    IEnumerator FallRoutine()
    {
        while (!_isGrounded && !_isDead)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            yield return null;
        }

        _isFalling = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isGrounded = true;
            _isFalling = false;
            anim.SetBool("Fall", _isFalling);
        }
    }


    public void Damage()
    {
        if (_isDead) return;

        Health--;
        Debug.Log("Health point lefts: " + Health);
        if (Health < 1)
        {
            SpawnCoin();
            _isDead = true;
            Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            if (player != null)
            {
                player.PlayerCanSwim();
            }
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
        while (true)
        {
            if (_isAttack)
            {
                _isPerformingAction = true;
                _hitbox.SetActive(false);

                int attackIndex = Random.Range(1, 4);

                switch (attackIndex)
                {
                    case 1:
                        anim.SetTrigger("Attack1");
                        break;
                    case 2:
                        anim.SetTrigger("Attack2");
                        break;
                    case 3:
                        anim.SetTrigger("Attack3");
                        break;
                }

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
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        while (!stateInfo.IsName("Death"))
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }
        yield return new WaitForSeconds(stateInfo.length);

        Destroy(gameObject);
    }


    public void ActivateBoss(Transform player)
    {
        _isActive = true;
        _target = player;

        if (!_isAttack)
        {
            StopAttack();
        }

        if (_teleportCoroutine == null)
        {
            _teleportCoroutine = StartCoroutine(TeleportRoutine());
        }

        _isIdle = false;
        anim.SetBool("Run", true);
    }

    public void DeactivateBoss()
    {
        if (!_isActive) return;

        _isActive = false;
        _target = null;
        if (_teleportCoroutine != null)
        {
            StopCoroutine(_teleportCoroutine);
            _teleportCoroutine = null;
        }
        _isAttack = false;
        _isIdle = true;
        anim.SetBool("Run", false);

        Health = base.health;
        _isFalling = false;
        _isGrounded = false;

        anim.Rebind();
    }

    IEnumerator TeleportRoutine()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(teleportInterval);

            if (_isPerformingAction) continue;

            Vector3 playerPos = _target.position;
            Vector3 newPosition = playerPos + new Vector3(Random.value > 0.5f ? teleportDistance : -teleportDistance, 0, 0);

            transform.position = newPosition;
            FlipToFacePlayer();
        }
    }

    private void FlipToFacePlayer()
    {
        if (_target == null) return;
        bool playerOnRight = _target.position.x > transform.position.x;

        if (playerOnRight != _moveRight)
        {
            Flip();
        }
    }
    //Load save data
    public void LoadData(GameData data)
    {
        this._isDead = data.isSadBossDeath;
    }

    public void SaveData(ref GameData data)
    {
        data.isSadBossDeath = this._isDead;
    }  
}
