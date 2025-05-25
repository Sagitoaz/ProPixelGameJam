using UnityEngine;

public class AxeDemonDetectZone : MonoBehaviour
{
    [SerializeField] private AxeDemon _enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _enemy.StartAttack(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _enemy.StopAttack();
        }
    }
}
