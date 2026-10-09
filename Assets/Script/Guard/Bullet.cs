using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;

    private Rigidbody rb;
    private float lifeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        lifeTimer = 0f;

        rb.linearVelocity = transform.forward * speed;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BULLET HIT: " + collision.gameObject.name);

        PlayerDeath playerDeath =
            collision.gameObject.GetComponentInParent<PlayerDeath>();

        if (playerDeath != null)
        {
            playerDeath.Die(transform.forward);
        }

        gameObject.SetActive(false);
    }
}