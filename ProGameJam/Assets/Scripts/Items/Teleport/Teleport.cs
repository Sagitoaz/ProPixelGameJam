using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject Portal, Player;
    [SerializeField] float tpTime = 0.5f;

    private bool playerInRange = false;
    private bool isTeleporting = false;

    private Player playerScript;

   private void Start()
    {
        playerScript = Player.GetComponent<Player>();
    }

    private void Update()
    {
        if (playerInRange && !isTeleporting && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(tp());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator tp()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(tpTime);
        Player.transform.position = Portal.transform.position;
        if (playerScript != null)
        {
            playerScript.Checkpoint = Portal.transform;
            DataPersistenceManager.Instance.SaveGame();
        }
        isTeleporting = false;
    }
}
