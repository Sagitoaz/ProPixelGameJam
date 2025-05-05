using System.Collections;
using UnityEngine;

public class BossVisibility : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float invisibleAlpha = 0.3f;
    [SerializeField] private float delayBeforeInvisible = 10f;

    private bool isInvisible = false;
    private Coroutine invisibilityCoroutine;

    public bool IsInvisible => isInvisible;

    public void StartCountdown()
    {
        if (invisibilityCoroutine == null)
        {
            invisibilityCoroutine = StartCoroutine(InvisibilityRoutine());
        }
    }

    private IEnumerator InvisibilityRoutine()
    {
        yield return new WaitForSeconds(delayBeforeInvisible);
        BecomeInvisible();
    }

    public void BecomeInvisible()
    {
        isInvisible = true;
        SetAlpha(invisibleAlpha);
        Debug.Log("Boss is now invisible");
    }

    public void BecomeVisible()
    {
        isInvisible = false;
        SetAlpha(1f);
        Debug.Log("Boss is now visible");
        if (invisibilityCoroutine != null)
        {
            StopCoroutine(invisibilityCoroutine);
            invisibilityCoroutine = null;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
