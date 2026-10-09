using UnityEngine;

public class ExtractionZone : MonoBehaviour
{
    public MissionManager missionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        missionManager.TryExtraction();
    }
}