using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable, IDataPersistence
{
    //Audio
    AudioManager audioManager;
    private bool _isRunningSFX = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    //Movement
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpForce = 7.0f;
    [SerializeField] private LayerMask _groundLayer;
    private float _move;
    private bool _grounded = false;
    private bool _resetJump = false;
    [SerializeField] private bool _canAirJump = false;
    [SerializeField] private bool _hasAirJump = false;
    [SerializeField] private int _coin = 0;
    [SerializeField] private bool _canSwim = false;

    //Dash
    [SerializeField] private float _dashForce = 12.0f;
    [SerializeField] private bool _canDash = false;
    private bool _isDash = false;

    //Combat
    private int _comboStep = 0;
    private float _comboTimer = 0f;
    [SerializeField] private float _comboDelay = 0.5f;
    private bool _canAttack = true;
    private bool _isAttackDash = false;

    //Health
    [SerializeField] private int _health;
    private bool _isDead = false;
    private float _healingHoldTime = 0f;
    [SerializeField] private float _requiredHoldTime = 1.0f;
    [SerializeField] private int _manaCostToHeal = 30;
    private float _healCooldown = 0.5f;
    private float _healCooldownTimer = 0f;

    //Components
    private PlayerAnimation _playerAnimator;
    private SpriteRenderer _playerSprite;

    //Hitbox
    [SerializeField] private Transform _hitbox;
    private bool _facingRight = true;

    //Environment Interact
    [SerializeField] private bool _inWater = false;
    [SerializeField] private bool _inLava = false;
    [SerializeField] private float _waterJumpForce = 3.0f;
    [SerializeField] private Transform _checkpoint;
    private bool _environmentDamaged = false;
    [SerializeField] private float _maxUnderwaterTime = 5f;
    [SerializeField] private float _underwaterTimer;
    public int Health { get; set; }

    //UI
    [SerializeField] private HeartUI _heartUI;
    [SerializeField] private ManaUI _manaUI;
    [SerializeField] private int _maxMana = 100;
    [SerializeField] private int _currentMana;
    [SerializeField] private Text _coinText;
    [SerializeField] private PauseUI _pauseUI;

    // Inventory
    [SerializeField] private InventoryManager _inventoryManager;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _heartUI.SetMaxHealth(_health);
        _heartUI.UpdateHearts(Health);
        _manaUI.SetMaxMana(_maxMana);
        _manaUI.UpdateMana(_currentMana);
        _coinText.text = "" + _coin;
        _underwaterTimer = _maxUnderwaterTime;
    }

    void Update()
    {
        if (_isDead) return;
        HandleSwim();
        if (_isDash) return;

        HandleAttack();
        HandleComboTimer();

        if (Input.GetKeyDown(KeyCode.L) && _canDash)
        {
            StartCoroutine(DashRoutine());
        }
        else if (_isAttackDash)
        {
            return;
        }
        else
        {
            HandleMovement();
        }
        HandleHealing();
    }

    //MOVEMENT
    void HandleMovement()
    {
        _move = Input.GetAxisRaw("Horizontal");
        FlipSprite();
        _grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_inLava)
            {
                return;
            }
            if (_inWater)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _waterJumpForce);
                _playerAnimator.Jump(true);
                return;
            }
            if (_grounded)
            {
                audioManager.PlaySFX(audioManager.jump);
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpForce);
                StartCoroutine(ResetJump());
                _playerAnimator.Jump(true);
                _hasAirJump = false;
            }
            else if (_canAirJump && !_hasAirJump)
            {
                audioManager.PlaySFX(audioManager.jump);
                _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpForce);
                StartCoroutine(ResetJump());
                _playerAnimator.Jump(true);
                _hasAirJump = true;
            }
        }

        _rb.linearVelocity = new Vector2(_move * _speed, _rb.linearVelocityY);
        if (_grounded && Mathf.Abs(_move) > 0.1f)
        {
            if (!_isRunningSFX)
            {
                audioManager.Run();
                _isRunningSFX = true;
            }
        }
        else
        {
            if (_isRunningSFX)
            {
                audioManager.StopRun();
                _isRunningSFX = false;
            }
        }
        _playerAnimator.Move(_move);
        _playerAnimator.Fall(_rb.linearVelocityY);
    }

    void FlipSprite()
    {
        if (_move > 0)
        {
            _playerSprite.flipX = false;
            if (!_facingRight)
            {
                _facingRight = true;
                FlipHitbox();
            }
        }
        else if (_move < 0)
        {
            _playerSprite.flipX = true;
            if (_facingRight)
            {
                _facingRight = false;
                FlipHitbox();
            }
        }
    }

    bool IsGrounded()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        Vector2 origin = capsule.bounds.center;
        Vector2 size = capsule.bounds.size;
        float extraHeight = 0.1f;

        RaycastHit2D hit = Physics2D.CapsuleCast(origin, size, capsule.direction, 0f, Vector2.down, extraHeight, _groundLayer);

        Color rayColor = hit.collider != null ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector2.down * (size.y / 2 + extraHeight), rayColor);

        if (hit.collider != null && !_resetJump)
        {
            _playerAnimator.Jump(false);
            _hasAirJump = false;
            return true;
        }

        return false;
    }

    IEnumerator ResetJump()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.5f);
        _resetJump = false;
    }

    private void HandleSwim()
    {
        if (_inWater && _canSwim)
        {
            _underwaterTimer -= Time.deltaTime;
            if (_underwaterTimer <= 0f)
            {
                if (!_environmentDamaged)
                {
                    Damage();
                    _environmentDamaged = true;
                    StartCoroutine(RespawnToCheckpoint());
                }
                _underwaterTimer = _maxUnderwaterTime;
            }
        }
        else if (_underwaterTimer < _maxUnderwaterTime)
        {
            _underwaterTimer += Time.deltaTime;
            _underwaterTimer = Mathf.Min(_underwaterTimer, _maxUnderwaterTime);
        }
    }

    //COMBAT
    void HandleAttack()
    {
        if (!_canAttack) return;
        if (Input.GetKeyDown(KeyCode.U))
        {
            audioManager.PlaySFX(audioManager.attack2);
            _playerAnimator.Attack(3);
            _comboStep = 0;
            _comboTimer = 0;
            StartCoroutine(AttackCoolDown());
            return;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (_comboStep == 0)
            {
                audioManager.PlaySFX(audioManager.attack1);
                _playerAnimator.Attack(1);
                _comboStep = 1;
                _comboTimer = 0;
            }
            else if (_comboStep == 1 && _comboTimer < _comboDelay)
            {
                audioManager.PlaySFX(audioManager.attack1);
                _playerAnimator.Attack(2);
                _comboStep = 0;
                _comboTimer = 0;
                StartCoroutine(DashWhenAttack());
                StartCoroutine(AttackCoolDown());
            }
        }
    }

    void HandleComboTimer()
    {
        if (_comboStep > 0)
        {
            _comboTimer += Time.deltaTime;
            if (_comboTimer > _comboDelay)
            {
                _comboStep = 0;
                _comboTimer = 0;
            }
        }
    }

    private void FlipHitbox()
    {
        Vector3 scale = _hitbox.localScale;
        scale.x = _facingRight ? 1 : -1;
        _hitbox.localScale = scale;
    }

    IEnumerator AttackCoolDown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(0.7f);
        _canAttack = true;
    }

    IEnumerator DashWhenAttack()
    {
        _isAttackDash = true;
        float dashForce = 6.0f;
        float direction = _playerSprite.flipX ? -1 : 1;
        _rb.linearVelocity = new Vector2(direction * dashForce, _rb.linearVelocityY);
        yield return new WaitForSeconds(0.5f);
        _isAttackDash = false;
    }

    //DASH
    IEnumerator DashRoutine()
    {
        audioManager.PlaySFX(audioManager.dash);
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
    public void Damage()
    {
        if (_isDead) return;
        Health--;
        audioManager.PlaySFX(audioManager.hitSound);
        _heartUI.UpdateHearts(Health);
        Debug.Log("Player's HP lefts: " + Health);
        _playerAnimator.Hit();
        if (Health < 1)
        {
            audioManager.PlaySFX(audioManager.death);
            _isDead = true;
            _playerAnimator.Death();
            Debug.Log("Current Real Health: " + Health);
        }
    }
    public void DestroyPlayer()
    {
        Health = _health;
        _heartUI.UpdateHearts(Health);
        _currentMana = _maxMana;
        _manaUI.UpdateMana(_currentMana);
        _isDead = false;
        _playerAnimator.Reborn();
        _coin = 0;
        _coinText.text = "" + _coin;
        DataPersistenceManager.Instance.SaveGame();
        DataPersistenceManager.Instance.LoadGame();
    }

    //HEALTH AND MANA
    private void HandleHealing()
    {
        if (_isDead) return;

        if (Input.GetKey(KeyCode.I))
        {
            _healingHoldTime += Time.deltaTime;
            if (_healingHoldTime >= _requiredHoldTime)
            {
                _healCooldownTimer -= Time.deltaTime;
                if (_currentMana >= _manaCostToHeal && Health < _health)
                {
                    _currentMana -= _manaCostToHeal;
                    _manaUI.UpdateMana(_currentMana);
                    Health++;
                    audioManager.PlaySFX(audioManager.HealingSound);
                    _heartUI.UpdateHearts(Health);
                    Debug.Log("Healed 1 HP. Current HP: " + Health);
                    _healCooldownTimer = _healCooldown;
                }

                _healingHoldTime = 0f;
            }
        }
        else
        {
            _healingHoldTime = 0f;
            _healCooldownTimer = 0f;
        }
    }

    //GETTER
    public bool GetIsGround()
    {
        return _grounded;
    }

    public void SetCoin(int coinQuantity)
    {
        _coin += coinQuantity;
        _coinText.text = "" + _coin;
        audioManager.PlaySFX(audioManager.coinCollect);
    }

    public Transform Checkpoint
    {
        get => _checkpoint;
        set => _checkpoint = value;
    }

    //ENVIRONMENT
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (!_canSwim)
            {
                if (!_environmentDamaged)
                {
                    Damage();
                    _environmentDamaged = true;
                    StartCoroutine(RespawnToCheckpoint());
                }
            }
            else
            {
                _inWater = true;
                _rb.gravityScale = 0.5f;
            }
        }
        if (other.CompareTag("Lava"))
        {
            _inLava = true;
            if (!_environmentDamaged)
            {
                Damage();
                _environmentDamaged = true;
                StartCoroutine(RespawnToCheckpoint());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("OnTriggerExit2D Water");
            _inWater = false;
            _rb.gravityScale = 5f;
        }
        if (other.CompareTag("Lava"))
        {
            _inLava = false;
        }
        _environmentDamaged = false;
    }
    private IEnumerator RespawnToCheckpoint()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = _checkpoint.position;
        _inLava = false;
        _inWater = false;
        _environmentDamaged = false;
    }

    // BUY ITEM
    public int GetCoin()
    {
        return _coin;
    }
    public void DeductCoin(int itemPrice)
    {
        if (_coin - itemPrice < 0) _coin = 0;
        else _coin -= itemPrice;
        SetCoin(0);
    }

    // USE ITEM
    public void UseItem(Item item)
    {
        item.Use(this);
    }

    //GETTER AND SETTER
    public int GetMaxHealth()
    {
        return _health;
    }
    public void SetMaxHealth(int healthAmount)
    {
        _health = healthAmount;
        _heartUI.SetMaxHealth(_health);
        _heartUI.UpdateHearts(Health);
    }
    public int GetMana()
    {
        return _currentMana;
    }
    public void SetMana(int manaAmount)
    {
        _currentMana = manaAmount;
        _manaUI.UpdateMana(_currentMana);
    }

    public void PlayerCanAirJump()
    {
        _canAirJump = true;
    }
    public void PlayerCanSwim()
    {
        _canSwim = true;
    }
    public void PlayerCanDash()
    {
        _canDash = true;
    }

    //Load save data
    public void LoadData(GameData data)
    {
        this.Health = data.currentHealth;
        this._health = data.maxHealth;
        this.transform.position = data.playerPosition;
        this._currentMana = data.currentMana;
        this._maxMana = data.maxMana;
        this._coin = data.coin;
        _coinText.text = "" + _coin;
        this._canAirJump = data.canDoubleJump;
        this._canDash = data.canDash;
        this._canSwim = data.canSwim;
    }

    public void SaveData(ref GameData data)
    {
        data.currentHealth = this.Health;
        data.maxHealth = this._health;
        if (_checkpoint != null)
            data.playerPosition = _checkpoint.position;
        else
            data.playerPosition = new Vector3(-57f, -25f, 0f); ;
        data.currentMana = this._currentMana;
        data.maxMana = this._maxMana;
        data.coin = this._coin;
        data.canDoubleJump = this._canAirJump;
        data.canDash = this._canDash;
        data.canSwim = this._canSwim;
    }
}
