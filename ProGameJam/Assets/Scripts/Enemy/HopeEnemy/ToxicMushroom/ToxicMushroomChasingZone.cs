using UnityEngine;

public class ToxicMushroomChasingZone : MonoBehaviour
{
    [SerializeField] private ToxicMushroom _toxicMushroomEnemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _toxicMushroomEnemy.StartChase(collision.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            _toxicMushroomEnemy.StopChase();
        }
    }
}
