
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartButtonAnimation : MonoBehaviour
{
    public Button button;
    public float pulseSpeed = 2f;
    public float scaleAmount = 1.08f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;

        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(PlayClickAnimation);
        StartCoroutine(PulseAnimation());
    }

    private IEnumerator PulseAnimation()
    {
        while (true)
        {
            float t = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f;

            transform.localScale = Vector3.Lerp(
                originalScale,
                originalScale * scaleAmount,
                t
            );

            yield return null;
        }
    }

    private void PlayClickAnimation()
    {
        StopAllCoroutines();
        transform.localScale = originalScale * 0.9f;
    }
}
