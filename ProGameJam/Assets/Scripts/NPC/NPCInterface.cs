using UnityEngine;

public class NPCInterface : MonoBehaviour
{
    public GameObject interactionUI;
    protected bool playerInRange = false;

    protected virtual void Start()
    {
        interactionUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            interactionUI.SetActive(false);
            Talk();
        }
    }

    protected virtual void Talk()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player in");
            interactionUI.SetActive(true);
            playerInRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player out");
            interactionUI.SetActive(false);
            playerInRange = false;
        }
    }
}
