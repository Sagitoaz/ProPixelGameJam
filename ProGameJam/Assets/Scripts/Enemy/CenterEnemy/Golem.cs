using System.Collections;
using UnityEngine;

public class Golem1 : Enemy
{
    [SerializeField] private LayerMask _groundLayer;
    private Rigidbody2D _rb;
    private bool _moveRight = true;
    private bool _canFlip = true;
    void Start()
    {
        base.Init();
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Patrol();
    }
    private void Patrol() {
        Vector2 originPosition = transform.position;
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        bool checkGround = Physics2D.Raycast(originPosition, Vector2.down, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        bool checkWall = Physics2D.Raycast(originPosition, direction, 1.0f, _groundLayer);
        Debug.DrawRay(transform.position, direction, Color.red);
        if (!checkGround || checkWall) {
            if (_canFlip) {
                Flip();
                StartCoroutine(ResetFlip());
            }
        }
        Move();
    }
    private void Move() {
        Vector2 direction = _moveRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);
        anim.SetBool("InCombat", false);
    }
    private void Flip() {
        _moveRight = !_moveRight;
        sprite.flipX = !sprite.flipX;
        anim.SetTrigger("Idle");
    }
    IEnumerator ResetFlip() {
        _canFlip = false;
        yield return new WaitForSeconds(0.5f);
        _canFlip = true;
    }
}
