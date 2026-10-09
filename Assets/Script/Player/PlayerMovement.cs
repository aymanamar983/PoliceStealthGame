using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public Animator animator;

    private Rigidbody rb;

    private string currentAnimation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(x, 0f, z).normalized;

        Vector3 movement = input * moveSpeed;

        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );

        if(input.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * 100f * Time.fixedDeltaTime));
        }
    }

    private void UpdateAnimation()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(x, 0f, z);

        if (input.sqrMagnitude < 0.01f)
        {
            PlayAnimation("Idle");
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayAnimation("Run");
        }
        else
        {
            PlayAnimation("Walk");
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
            return;

        currentAnimation = animationName;

        animator.CrossFade(
            animationName,
            0.15f,
            0,
            0f
        );
    }
}