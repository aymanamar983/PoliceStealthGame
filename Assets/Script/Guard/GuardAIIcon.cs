
using UnityEngine;

public class GuardAIIcon : MonoBehaviour
{
    public GameObject suspiciousIcon;
    public GameObject shootingIcon;

    public void ShowSuspicious()
    {
        suspiciousIcon.SetActive(true);

        if (shootingIcon != null)
            shootingIcon.SetActive(false);
    }

    public void ShowShooting()
    {
        suspiciousIcon.SetActive(false);

        if (shootingIcon != null)
            shootingIcon.SetActive(true);
    }

    public void HideIcons()
    {
        suspiciousIcon.SetActive(false);

        if (shootingIcon != null)
            shootingIcon.SetActive(false);
    }
}
