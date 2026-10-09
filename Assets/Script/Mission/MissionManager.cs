using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public bool targetRetrieved {  get; private set; }

    public bool missionComplete { get; private set; }

    public GameWonUI gameWonUI;

    public KeyUI keyUI;

    public void RetrieveTarget()
    {
        targetRetrieved = true;

        Debug.Log("Target retrieved!");
    }

    public void TryExtraction()
    {
        if (missionComplete)
            return;

        if (!targetRetrieved)
        {
            Debug.Log("Key Required.");

            if(keyUI != null)
                keyUI.ShowKeyPopup();

            return;
        }

        missionComplete = true;

        Debug.Log("Mission Complete");

        if(gameWonUI != null)
        {
            gameWonUI.ShowGameWon();
        }
    }
}
