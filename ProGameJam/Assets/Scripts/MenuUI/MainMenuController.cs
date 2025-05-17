using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string fileName = "save.json";
    [SerializeField] private bool useEncryption = false;
    [SerializeField] private string sceneToLoad = "GameScene"; // Tên scene chơi chính

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
        SceneManager.LoadScene(sceneToLoad);
    }
}
