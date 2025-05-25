using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public System.Action OnEnemyDeath;
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] private int coins = 1;
    [SerializeField] private GameObject _coinPrefabs;
    [SerializeField] protected LayerMask _groundLayer;
    protected bool _isDead = false;
    protected Rigidbody2D _rb;
    protected Transform _target;
    protected bool _isIdle = false;
    protected bool _isAttack = false;
    protected bool _moveRight = true;
    protected bool _canFlip = true;
    protected Animator anim;
    protected SpriteRenderer sprite;
    protected Player player;

    void Start()
    {
        Init();
    }
    void Update()
    {
        
    }
     public virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        if (_coinPrefabs == null)
        {
            Debug.LogWarning("HopeBoss: Coin Prefab is not assigned!", this);
        }
    }

    public void SpawnCoin()
    {
        if (_coinPrefabs == null)
        {
            Debug.LogWarning("SpawnCoin: Coin Prefab is not assigned!", this);
            return;
        }
        GameObject coin = Instantiate(_coinPrefabs, transform.position, Quaternion.identity);
        Coin coinComponent = coin.GetComponent<Coin>();
        if (coinComponent != null)
        {
            coinComponent.SetCoinQuantity(coins);
        }
        else
        {
            Debug.LogWarning("SpawnCoin: Coin Prefab does not have Coin component!", this);
        }
    }
    protected virtual void Move() {
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);
        anim.SetBool("Moving", true);
    }
    protected virtual void Flip() {
        _moveRight = !_moveRight;
        sprite.flipX = !sprite.flipX;
    }
    protected virtual IEnumerator IdleToFlip() {
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
    protected virtual void Patrol() {
        if (_isIdle) {
            return;
        }
        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        bool checkWall = Physics2D.Raycast(originPosition, direction, 1.0f, _groundLayer);
        Debug.DrawRay(transform.position, direction, Color.red);
        if (!checkGround || checkWall) {
            if (_canFlip) {
                StartCoroutine(IdleToFlip());
            }
            return;
        }
        Move();
    }
    protected virtual void MoveToTarget()
    {
        if (_target == null) {
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
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 2.5f, _groundLayer);
        bool checkWall = Physics2D.Raycast(originPosition, moveDirection, 1.0f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 2.5f, Color.green);
        Debug.DrawRay(transform.position, moveDirection * 1.0f, Color.red);
        if (!checkGround || checkWall) {
            anim.SetBool("Moving", false);
            return;
        }
        if (moveRight != _moveRight) {
            Flip();
        }
        Move();
    }
    protected virtual void FaceTarget() {
        if (_target == null) return;
        if (_target.position.x > transform.position.x && !_moveRight) {
            Flip();
        } else if (_target.position.x < transform.position.x && _moveRight) {
            Flip();
        }
    }
}
