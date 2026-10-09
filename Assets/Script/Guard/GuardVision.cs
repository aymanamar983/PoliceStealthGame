using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public Transform player;
    public GuardAI guardAI;

    public float viewDistance = 10f;
    public float viewAngle = 90f;

    public LayerMask obstacleMask;

    private bool playerVisible;

    private void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        Vector3 directionToPlayer =
            player.position - transform.position;

        float distance = directionToPlayer.magnitude;

        // Outside vision distance
        if (distance > viewDistance)
        {
            PlayerNotVisible();
            return;
        }

        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        // FOV check
        float dot =
            Vector3.Dot(
                transform.forward,
                directionToPlayer
            );

        float angle =
            Mathf.Cos(
                viewAngle * 0.5f * Mathf.Deg2Rad
            );

        if (dot < angle)
        {
            PlayerNotVisible();
            return;
        }

        // Wall check
        Vector3 rayOrigin =
            transform.position + Vector3.up * 1f;

        if (Physics.Raycast(
            rayOrigin,
            directionToPlayer,
            distance,
            obstacleMask))
        {
            PlayerNotVisible();
            return;
        }

        // Player is visible
        if (!playerVisible)
        {
            Debug.Log("PLAYER DETECTED!");
        }

        playerVisible = true;

        guardAI.PlayerDetected();
    }

    private void PlayerNotVisible()
    {
        if (!playerVisible)
            return;

        playerVisible = false;

        Debug.Log("PLAYER LOST!");

        guardAI.PlayerLost();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 left =
            Quaternion.Euler(
                0f,
                -viewAngle * 0.5f,
                0f
            ) * transform.forward;

        Vector3 right =
            Quaternion.Euler(
                0f,
                viewAngle * 0.5f,
                0f
            ) * transform.forward;

        Gizmos.DrawRay(
            transform.position,
            left * viewDistance
        );

        Gizmos.DrawRay(
            transform.position,
            right * viewDistance
        );

        Gizmos.DrawRay(
            transform.position,
            transform.forward * viewDistance
        );
    }
}