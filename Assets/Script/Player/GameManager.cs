using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startPanel;

    public PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement.enabled = false;
        startPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        playerMovement.enabled = true;
        Time.timeScale = 1f;
    }
}
