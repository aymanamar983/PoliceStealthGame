using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 12f, -8f);

    public Vector3 rotation = new Vector3(55f, 0f, 0f);

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(rotation);
    }
}