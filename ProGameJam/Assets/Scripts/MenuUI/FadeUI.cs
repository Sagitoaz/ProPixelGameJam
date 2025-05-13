using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void FadeUIOut(float second) {
        StartCoroutine(FadeOut(second));
    }
    public void FadeUIIn(float second) {
        StartCoroutine(FadeIn(second));
    }
    IEnumerator FadeOut(float second) {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 1;
        while(_canvasGroup.alpha > 0) {
            _canvasGroup.alpha -= Time.unscaledDeltaTime / second;
            yield return null;
        }
        yield return null;
    }
    IEnumerator FadeIn(float second) {
        _canvasGroup.alpha = 0;
        while(_canvasGroup.alpha < 1) {
            _canvasGroup.alpha += Time.unscaledDeltaTime / second;
            yield return null;
        }
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        yield return null;
    }
}
