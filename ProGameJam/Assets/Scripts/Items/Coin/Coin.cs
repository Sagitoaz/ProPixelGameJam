using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _coinQuantity = 0;

    private void Start()
    {
        if (!gameObject.CompareTag("Coin"))
        {
            Debug.LogWarning("Coin: GameObject should have 'Coin' tag for clarity!", this);
        }
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("Coin: Collider2D is missing or not set to Is Trigger!", this);
        }
        if (_coinQuantity <= 0)
        {
            Debug.LogWarning($"Coin: _coinQuantity is {_coinQuantity}. Consider setting a positive value!", this);
        }
    }

    public void SetCoinQuantity(int newCoinQuantity)
    {
        _coinQuantity = newCoinQuantity;
        Debug.Log($"Coin: SetCoinQuantity to {_coinQuantity}", this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            Debug.LogError("Coin: OnTriggerEnter2D received null collision!", this);
            return;
        }

        Debug.Log($"Coin: Trigger entered by {collision.name} with tag {collision.tag}", this);

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log($"Coin: Adding {_coinQuantity} coins to player {player.name}", this);
                player.SetCoin(_coinQuantity);
                Destroy(gameObject);
                Debug.Log("Coin: Destroyed after player collision", this);
            }
            else
            {
                Debug.LogError("Coin: Player component not found on GameObject with 'Player' tag!", this);
            }
        }
        else
        {
            Debug.Log($"Coin: Ignored collision with non-Player object {collision.name}", this);
        }
    }
}