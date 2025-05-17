using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption = false;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        if (dataHandler.SaveFileExists())
        {
            continueButton.gameObject.SetActive(true);
        }
    }
}
