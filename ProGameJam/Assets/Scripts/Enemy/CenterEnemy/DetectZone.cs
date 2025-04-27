using UnityEngine;

public class DetectZone : MonoBehaviour
{
    [SerializeField] private Golem1 _enemy;
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
