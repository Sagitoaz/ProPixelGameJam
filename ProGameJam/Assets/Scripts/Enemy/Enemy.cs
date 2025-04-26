using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected int coins;
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
    }
}
