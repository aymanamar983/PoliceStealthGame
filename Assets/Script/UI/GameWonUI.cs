
using UnityEngine;
using System.Collections;

public class GameWonUI : MonoBehaviour
{
    public GameObject gameWonPanel;
    public float animationDuration = 0.4f;

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = gameWonPanel.transform.localScale;
        gameWonPanel.SetActive(false);
    }

    public void ShowGameWon()
    {
        gameWonPanel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PopupAnimation());
    }

    private IEnumerator PopupAnimation()
    {
        Transform panel = gameWonPanel.transform;

        panel.localScale = Vector3.zero;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);

            // Smooth pop with a slight overshoot
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
