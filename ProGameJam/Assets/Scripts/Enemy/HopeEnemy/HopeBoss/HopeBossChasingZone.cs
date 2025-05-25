using UnityEngine;

public class HopeBossChasingZone : MonoBehaviour
{
    [SerializeField] private HopeBoss _enemy;

    private void Start()
    {
        if (_enemy == null)
        {
            Debug.LogError("HopeBossChasingZone: HopeBoss (_enemy) is not assigned in the Inspector!", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
        {
            Debug.LogError("OnTriggerEnter2D: Collision is null!", this);
            return;
        }

        if (collision.CompareTag("Player"))
        {
            if (_enemy == null)
            {
                Debug.LogError("HopeBossChasingZone: HopeBoss (_enemy) is not assigned in the Inspector!", this);
                return;
            }

            Debug.Log($"ChasingZone: Player entered, activating HopeBoss {_enemy.name}");
            _enemy.speedUp(2);
            _enemy.StartChase(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
        {
            Debug.LogError("OnTriggerExit2D: Collision is null!", this);
            return;
        }

        if (collision.CompareTag("Player"))
        {
            if (_enemy == null)
            {
                Debug.LogError("HopeBossChasingZone: HopeBoss (_enemy) is not assigned in the Inspector!", this);
                return;
            }

            Debug.Log($"ChasingZone: Player exited, deactivating chase for {_enemy.name}");
            _enemy.speedDown(2);
            _enemy.StopChase();
        }
    }
}