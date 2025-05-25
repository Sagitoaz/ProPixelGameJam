using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string fileName = "data.json";
    [SerializeField] private bool useEncryption = false;
    [SerializeField] private string sceneToLoad = "Game"; // Tên scene chơi chính

    public void OnClickNewGame()
    {
        // Tạo game data mới
        DataPersistenceManager.Instance.NewGame();

        // (Tuỳ chọn) Ghi đè file cũ ngay lập tức
        DataPersistenceManager.Instance.SaveGame();

        // Chuyển scene
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnClickContinue()
    {
        // Không cần làm gì thêm vì LoadGame đã được gọi ở Start() của DataPersistenceManager
        DataPersistenceManager.Instance.LoadGame();
        SceneManager.LoadScene(sceneToLoad);
    }
    public void OnClickQuit()
    {
        Debug.Log("App Quit");
        Application.Quit();
    }
}
