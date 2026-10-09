using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public bool IsDead { get; private set; }

    private Animator animator;
    private Rigidbody playerRigidbody;
    private PlayerMovement playerMovement;

    public GameOverUI gameOverUI;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Die(Vector3 hitDirection)
    {
        if (IsDead)
            return;

        IsDead = true;

        Debug.Log("PLAYER DIED");

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        if (animator != null)
        {
            Debug.Log("PLAYING DEATH ANIMATION");

            animator.Play("Death", 0, 0f);
        }
        else
        {
            Debug.LogError("PLAYER ANIMATOR IS NULL!");
        }

        if(gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
    }
}