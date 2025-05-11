using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] private int coins = 1;
    [SerializeField] private GameObject _coinPrefabs;
    
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
}
