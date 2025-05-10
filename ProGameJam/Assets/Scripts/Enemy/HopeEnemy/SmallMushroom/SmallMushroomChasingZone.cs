using UnityEngine;

public class SmallMushroomChasingZone : MonoBehaviour
{
    [SerializeField] private SmallMushroom _enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _enemy.StartChase(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _enemy.StopChase();
        }
    }
}
