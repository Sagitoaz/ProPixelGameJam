using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpforce = 7.0f;
    [SerializeField] LayerMask _groundLayer;
    private PlayerAnimation _playerAnimator;
    private SpriteRenderer _playerSprite;
    private bool _grounded = false;
    private bool _resetJump = false;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        Movement();
    }
    void Movement() {
        float move = Input.GetAxisRaw("Horizontal");
        Flip(move);
        _grounded = IsGround();
        if (Input.GetKeyDown(KeyCode.Space) && IsGround()) {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, _jumpforce);
            StartCoroutine(ResetJump());
            _playerAnimator.Jump(true);
        }
        _rb.linearVelocity = new Vector2(move * _speed, _rb.linearVelocityY);
        _playerAnimator.Move(move);
        _playerAnimator.Fall(_rb.linearVelocityY);
    }
    void Flip(float move) {
        if (move > 0) {
            _playerSprite.flipX = false;
        } else if (move < 0) {
            _playerSprite.flipX = true;
        }
    }
    bool IsGround() {
        RaycastHit2D hitInfor = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);
        if (hitInfor.collider != null) {
            Debug.Log("Hit: " + hitInfor.collider.name);
            if (!_resetJump) {
                _playerAnimator.Jump(false);
                return true;
            }
        }
        Debug.Log("Hit: Nothing");
        return false;
    }
    IEnumerator ResetJump() {
        _resetJump = true;
        yield return new WaitForSeconds(0.5f);
        _resetJump = false;
    }
}
