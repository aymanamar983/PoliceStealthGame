
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;

    public float animationDuration = 0.35f;

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = gameOverPanel.transform.localScale;
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PopupAnimation());
    }

    private IEnumerator PopupAnimation()
    {
        Transform panel = gameOverPanel.transform;

        panel.localScale = Vector3.zero;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);

            float scale = 1f + 0.15f *
                Mathf.Sin(t * Mathf.PI);

            panel.localScale = Vector3.Lerp(
                Vector3.zero,
                targetScale,
                t
            ) * scale;

            yield return null;
        }

        panel.localScale = targetScale;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
