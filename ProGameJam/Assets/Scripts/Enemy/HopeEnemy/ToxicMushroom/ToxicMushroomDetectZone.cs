using UnityEngine;

public class ToxicMushroomDectectZone : MonoBehaviour
{
    [SerializeField] private ToxicMushroom _enemy;
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
