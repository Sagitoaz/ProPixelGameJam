using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _speed = 2.0f;
    private SpriteRenderer _sprite;
    private Vector2 _direction;

    private Animator anim;
    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    public void SetDirection(Vector2 direct) {
        if (direct.x < 0) {
            _sprite.flipX = true;
        }
        _direction = direct.normalized;
    }
    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            Debug.Log("Fire hit: " + collision.name);
            IDamageable player = collision.GetComponent<IDamageable>();
            player.Damage();
            anim.SetTrigger("Explosion");
            Destroy(gameObject);
        } else if (collision.CompareTag("Ground")) {
            anim.SetTrigger("Explosion");
            Destroy(gameObject);
        }
    }
}
