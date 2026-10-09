using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GuardVisionCone : MonoBehaviour
{
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public int rayCount = 30;

    public LayerMask obstacleMask;

    private Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "Guard Vision Cone";

        GetComponent<MeshFilter>().mesh = mesh;

        CreateVisionCone();
    }

    private void CreateVisionCone()
    {
        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        float angleStep = viewAngle / rayCount;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = -viewAngle * 0.5f + angleStep * i;

            float radians = angle * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(
                Mathf.Sin(radians),
                0f,
                Mathf.Cos(radians)
            );

            float distance = viewDistance;

            // Check for wall
            if (Physics.Raycast(
                transform.position,
                direction,
                out RaycastHit hit,
                viewDistance,
                obstacleMask))
            {
                distance = hit.distance;
            }

            vertices[i + 1] = direction * distance;
        }

        for (int i = 0; i < rayCount; i++)
        {
            int index = i * 3;

            triangles[index] = 0;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}