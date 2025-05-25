using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    [Header("Video Components")]
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public RenderTexture renderTexture;
    public AudioManager audioManager;
    public Image image;

    [Header("----- Ending Videos -----")]
    public VideoClip Ending1;
    public VideoClip Ending2;
    public VideoClip Ending3;

    private string nextSceneName;
    void Start()
    {
        rawImage.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }
    public void PlayVideoED(VideoClip videoClip, string sceneToLoad = "MainMenu")
    {
        // Kích hoạt hiển thị video
        image.gameObject.SetActive(true);
        rawImage.gameObject.SetActive(true);
        rawImage.texture = renderTexture;
        videoPlayer.targetTexture = renderTexture;
        audioManager.musicVolume = 0f;
        audioManager.sfxVolume = 0f;

        // Cài đặt video và scene cần load
        nextSceneName = sceneToLoad;
        videoPlayer.clip = videoClip;

        // Xóa đăng ký cũ (nếu có), rồi đăng ký sự kiện mới
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.loopPointReached += OnVideoEnd;

        // Phát video
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Ẩn RawImage sau khi video kết thúc
        image.gameObject.SetActive(false);
        rawImage.gameObject.SetActive(false);
        audioManager.musicVolume = 1f;
        audioManager.sfxVolume = 1f;
        // Chuyển Scene
        SceneManager.LoadScene(nextSceneName);
    }
}
