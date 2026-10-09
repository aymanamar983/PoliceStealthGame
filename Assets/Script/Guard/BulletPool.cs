using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private GameObject[] bullets;

    private void Awake()
    {
        bullets = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, transform);
            bullets[i].SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!bullets[i].activeSelf)
            {
                return bullets[i];
            }
        }

        return null;
    }
}