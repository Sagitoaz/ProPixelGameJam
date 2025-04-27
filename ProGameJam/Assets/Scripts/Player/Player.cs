using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Movement
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpForce = 7.0f;
    [SerializeField] private LayerMask _groundLayer;
    private float _move;
    private bool _grounded = false;
    private bool _resetJump = false;
    [SerializeField] private bool _canAirJump = true;
    [SerializeField] private bool _hasAirJump = false;

    //Dash
    [SerializeField] private float _dashForce = 12.0f;
    private bool _canDash = true;
    private bool _isDash = false;

    //Combat
    private int _comboStep = 0;
    private float _comboTimer = 0f;
    [SerializeField] private float _comboDelay = 0.5f;
    private bool _canAttack = true;
    private bool _isAttackDash = false;

    //Health
    [SerializeField] private int _health = 3;
    private bool _isDead = false;

    //Components
    private PlayerAnimation _playerAnimator;
    private SpriteRenderer _playerSprite;

    //Hitbox
    [SerializeField] private Transform _hitbox;
    private bool _facingRight = true;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update() {
        if (HandleDeath()) return;
        if (_isDash) return;

        HandleAttack();
        HandleComboTimer();

        if (Input.GetKeyDown(KeyCode.L) && _canDash) {
            StartCoroutine(DashRoutine());
        } else if (_isAttackDash) {
            return;
        } else {
            HandleMovement();
        }
    }

    //MOVEMENT
    void HandleMovement() {
        _move = Input.GetAxisRaw("Horizontal");
        FlipSprite();
        _grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_grounded) {
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpForce);
                StartCoroutine(ResetJump());
                _playerAnimator.Jump(true);
                _hasAirJump = false;
            } else if (_canAirJump && !_hasAirJump) {
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpForce);
                StartCoroutine(ResetJump());
                _playerAnimator.Jump(true);
                _hasAirJump = true;
            }
        }

        _rb.linearVelocity = new Vector2(_move * _speed, _rb.linearVelocityY);
        _playerAnimator.Move(_move);
        _playerAnimator.Fall(_rb.linearVelocityY);
    }

    void FlipSprite() {
        if (_move > 0) {
            _playerSprite.flipX = false;
            if (!_facingRight) {
                _facingRight = true;
                FlipHitbox();
            }
        } else if (_move < 0) {
            _playerSprite.flipX = true;
            if (_facingRight) {
                _facingRight = false;
                FlipHitbox();
            }
        }
    }

    bool IsGrounded() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (hit.collider != null && !_resetJump) {
            _playerAnimator.Jump(false);
            _hasAirJump = false;
            return true;
        }

        return false;
    }

    IEnumerator ResetJump() {
        _resetJump = true;
        yield return new WaitForSeconds(0.5f);
        _resetJump = false;
    }

    //COMBAT
    void HandleAttack() {
        if (!_canAttack) return;
        if (Input.GetKeyDown(KeyCode.U)) {
            _playerAnimator.Attack(3);
            _comboStep = 0;
            _comboTimer = 0;
            StartCoroutine(AttackCoolDown());
            return;
        } 
        if (Input.GetKeyDown(KeyCode.J)) {
            if (_comboStep == 0) {
                _playerAnimator.Attack(1);
                _comboStep = 1;
                _comboTimer = 0;
            } else if (_comboStep == 1 && _comboTimer < _comboDelay) {
                _playerAnimator.Attack(2);
                _comboStep = 0;
                _comboTimer = 0;
                StartCoroutine(DashWhenAttack());
                StartCoroutine(AttackCoolDown());
            }
        }
    }

    void HandleComboTimer() {
        if (_comboStep > 0) {
            _comboTimer += Time.deltaTime;
            if (_comboTimer > _comboDelay) {
                _comboStep = 0;
                _comboTimer = 0;
            }
        }
    }

    private void FlipHitbox() {
        Vector3 scale = _hitbox.localScale;
        scale.x = _facingRight ? 1 : -1;
        _hitbox.localScale = scale;
    }

    IEnumerator AttackCoolDown() {
        _canAttack = false;
        yield return new WaitForSeconds(0.7f);
        _canAttack = true;
    }

    IEnumerator DashWhenAttack() {
        _isAttackDash = true;
        float dashForce = 6.0f;
        float direction = _playerSprite.flipX ? -1 : 1;
        _rb.linearVelocity = new Vector2(direction * dashForce, _rb.linearVelocityY);
        yield return new WaitForSeconds(0.5f);
        _isAttackDash = false;
    }

    //DASH
    IEnumerator DashRoutine() {
        _isDash = true;
        _canDash = false;

        _playerAnimator.Dash();
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0;

        float direction = _playerSprite.flipX ? -1 : 1;
        _rb.linearVelocity = new Vector2(_dashForce * direction, 0);

        yield return new WaitForSeconds(0.5f);

        _rb.linearVelocity = Vector2.zero;
        _rb.gravityScale = originalGravity;
        _isDash = false;

        yield return new WaitForSeconds(0.3f);
        _canDash = true;
    }

    //DEATH
    bool HandleDeath()  {
        if (_health < 1 && !_isDead) {
            _isDead = true;
            StartCoroutine(DestroyPlayerDelay());
            return true;
        }
        return _isDead;
    }

    IEnumerator DestroyPlayerDelay() {
        _playerAnimator.Death();
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
