using System.Collections;
using UnityEngine;

public class FishBig : Enemy, IDamageable
{
    public int Health { get; set; }

    [SerializeField] private float flyDistance = 5f;
    private Vector3 _startPos;
    private Vector3 _leftLimit;
    private Vector3 _rightLimit;

    void Start()
    {
        base.Init();
        Health = 1;
        _rb = GetComponent<Rigidbody2D>();

        _startPos = transform.position;
        _leftLimit = _startPos - new Vector3(flyDistance / 2f, 0, 0);
        _rightLimit = _startPos + new Vector3(flyDistance / 2f, 0, 0);
    }

    void Update()
    {
        if (!_isIdle && !_isDead)
        {
            Patrol();
        }
    }

    protected override void Patrol()
    {
        Vector3 targetPos = _moveRight ? _rightLimit : _leftLimit;
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            StartCoroutine(IdleToFlipDirection());
        }
    }

    IEnumerator IdleToFlipDirection()
    {
        _isIdle = true;
        yield return new WaitForSeconds(0.1f);
        Flip();
        _isIdle = false;
    }
    public void Damage()
    {
        if (_isDead) return;
        Health--;
        Debug.Log("Fish hit! Remaining health: " + Health);
        if (Health < 1)
        {
            SpawnCoin();
            _isDead = true;
            OnEnemyDeath?.Invoke();
            _isIdle = true;
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
