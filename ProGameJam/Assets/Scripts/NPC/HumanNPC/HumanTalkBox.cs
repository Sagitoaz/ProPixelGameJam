using UnityEngine;
using TMPro;

public class HumanTalkBox : MonoBehaviour
{
    public static HumanTalkBox Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    void Start()
    {
        dialogueText.fontSize = 40;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueBox.SetActive(false);
    }

    public void ShowDialogue(string text)
    {
        dialogueBox.SetActive(true);
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueText.text = "";
    }
}
