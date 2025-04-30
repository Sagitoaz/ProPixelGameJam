using System.Collections;
using UnityEngine;

public class Mushroom : Enemy, IDamageable
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody2D _rb;
    private bool _moveRight = true;
    private bool _canFlip = true;
    private bool _isIdle = false;
    private bool _isAttack = false;
    private Transform _target;
    [SerializeField] private GameObject _firePrefab;
    private Coroutine _attackCoroutine;

    public int Health { get; set; }

    void Start()
    {
        base.Init();
        Health = base.health;
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (!_isAttack) {
            Patrol();
        } else {
            FaceTarget();
        }
    }
    private void Patrol() {
        if (_isIdle) {
            return;
        }
        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 2.0f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        bool checkWall = Physics2D.Raycast(originPosition, direction, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, direction, Color.red);
        if (!checkGround || checkWall) {
            if (_canFlip) {
                StartCoroutine(IdleToFlip());
            }
            return;
        }
        Move();
    }
    private void Move() {
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);
        anim.SetBool("Walk", true);
    }
    private void Flip() {
        _moveRight = !_moveRight;
        sprite.flipX = !sprite.flipX;
    }
    IEnumerator IdleToFlip() {
        _isIdle = true;
        anim.SetBool("Walk", false); 
        yield return new WaitForSeconds(1.0f);
        Flip();
        _canFlip = false;
        Move();
        yield return new WaitForSeconds(1.0f);
        _canFlip = true;
        _isIdle = false;
        anim.SetBool("Walk", true);
    }

    public void Damage()
    {
        Health--;
        Debug.Log("Health point lefts: " + Health);
        anim.SetTrigger("TakeHit");
        if (Health < 1) {
            Destroy(gameObject);
        }
    }
    public void StartAttack(Transform player) {
        _isAttack = true;
        _isIdle = false;
        _target = player;
        anim.SetBool("Walk", false);
        if (_attackCoroutine == null) {
            _attackCoroutine = StartCoroutine(AttackRoutine());
        }
    }
    public void StopAttack() {
        _isAttack = false;
        _isIdle = false;
        anim.SetBool("Walk", true);
    }
    private void FaceTarget() {
        if (_target == null) return;
        if (_target.position.x > transform.position.x && !_moveRight) {
            Flip();
        } else if (_target.position.x < transform.position.x && _moveRight) {
            Flip();
        }
    }
    IEnumerator AttackRoutine() {
        while (true) {
            if (_isAttack) {
                anim.SetTrigger("Attack");
                SpawnFire();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    private void SpawnFire() {
        if (_firePrefab != null && _target != null) {
            GameObject fire = Instantiate(_firePrefab, transform.position, Quaternion.identity);
            Vector2 direction = _target.position - transform.position;
            fire.GetComponent<Fire>().SetDirection(direction);
        }
    }
}
