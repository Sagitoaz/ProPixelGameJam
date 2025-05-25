using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFadeController : MonoBehaviour
{
    private FadeUI _fadeUI;
    [SerializeField] private float _fadeTime;
    void Start()
    {
        _fadeUI = GetComponent<FadeUI>();
        _fadeUI.FadeUIOut(_fadeTime);
    }
    public void CallStartGame(int _sceneIndex) {
        StartCoroutine(FadeAndStartGame(_sceneIndex));
    }
    IEnumerator FadeAndStartGame(int _sceneIndex) {
        _fadeUI.FadeUIIn(_fadeTime);
        yield return new WaitForSeconds(_fadeTime);
        SceneManager.LoadScene(_sceneIndex);
    }
}
