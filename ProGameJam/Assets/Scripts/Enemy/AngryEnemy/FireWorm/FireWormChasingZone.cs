using UnityEngine;

public class FireWormChasingZone : MonoBehaviour
{
    [SerializeField] private FireWorm _fireWormEnemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _fireWormEnemy.StartChase(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _fireWormEnemy.StopChase();
        }
    }
}
