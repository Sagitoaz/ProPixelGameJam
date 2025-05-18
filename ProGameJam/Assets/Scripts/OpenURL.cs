using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string url;
    public void OpenURLLink()
    {
        Application.OpenURL(url);
    }
}
