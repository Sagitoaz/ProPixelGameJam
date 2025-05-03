using UnityEngine;

public class FireWormDectectZone : MonoBehaviour
{
    [SerializeField] private FireWorm _enemy;
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
