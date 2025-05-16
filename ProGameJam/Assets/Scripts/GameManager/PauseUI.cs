using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    private bool isPaused = false;
    [SerializeField] private AudioManager audioManager;

    void Start()
    {
        pauseUI.SetActive(false);

        // Gán slider ban đầu
        musicSlider.value = audioManager.GetMusicVolume();
        sfxSlider.value = audioManager.GetSfxVolume();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SetMusicVolume(float value)
    {
        audioManager.SetMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        audioManager.SetSfxVolume(value);
    }
}
