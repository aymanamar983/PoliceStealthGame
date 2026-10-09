using UnityEngine;

public class RetrievalItem : MonoBehaviour
{
    public MissionManager missionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        missionManager.RetrieveTarget();

        gameObject.SetActive(false);
    }
}