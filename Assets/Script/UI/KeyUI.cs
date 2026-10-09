
using UnityEngine;
using System.Collections;

public class KeyUI : MonoBehaviour
{
    public GameObject keyPanel;
    public float animationDuration = 0.35f;

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = keyPanel.transform.localScale;
        keyPanel.SetActive(false);
    }

    public void ShowKeyPopup()
    {
        keyPanel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PopupAnimation());
    }

    private IEnumerator PopupAnimation()
    {
        Transform panel = keyPanel.transform;
        panel.localScale = Vector3.zero;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);
            float scale = 1f + 0.15f * Mathf.Sin(t * Mathf.PI);

            panel.localScale = Vector3.Lerp(
                Vector3.zero,
                targetScale,
                t
            ) * scale;

            yield return null;
        }

        panel.localScale = targetScale;
    }
}
