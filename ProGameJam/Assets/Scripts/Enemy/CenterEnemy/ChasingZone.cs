using UnityEngine;

public class ChasingZone : MonoBehaviour
{
    [SerializeField] private Golem _enemy;
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
