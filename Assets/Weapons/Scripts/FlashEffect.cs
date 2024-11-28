using UnityEngine;
using Utilities;

[RequireComponent(typeof(MeshFilter))]
public class FlashEffect : MonoBehaviour
{
    public LayerMask layerMask;
    public float fovDegrees = 30f;
    [Tooltip("Higher the value, higher the cost and accuracy.")]
    public int raycastCount = 10;
    public float maxDistance = 300f;
    [Tooltip("Enable this to automatically redraw in every LateUpdate.")]
    public bool autoRedraw = false;

    private Vector3 _lastPosition = new(Mathf.NegativeInfinity, Mathf.NegativeInfinity);
    private Quaternion _lastRotation;
    private Mesh _dynamicMesh;

    private MeshFilter meshFilter;

    private void OnDrawGizmos()
    {
        int rayCount = Mathf.Clamp((int)(fovDegrees / 10f), 3, raycastCount);

        float halfFov = fovDegrees / 2;
        float angle = transform.rotation.eulerAngles.z + halfFov;
        float angleIncrease = fovDegrees / rayCount;

        Vector3 origin = transform.position;
        Vector3 prevPoint = origin;

        // Set Gizmos color
        Gizmos.color = Color.yellow;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 raycastDir = MathUtilities.RotationToVector2(angle).normalized;
            Vector3 endPoint;

            RaycastHit2D raycastHit = Physics2D.Raycast(origin, raycastDir, maxDistance, layerMask);
            if (raycastHit.collider == null)
            {
                endPoint = origin + raycastDir * maxDistance;
            }
            else
            {
                endPoint = raycastHit.point;
            }

            // Draw the ray
            Gizmos.DrawLine(origin, endPoint);

            // Draw the outline of the spotlight
            if (i > 0)
            {
                Gizmos.DrawLine(prevPoint, endPoint);
            }
            prevPoint = endPoint;

            angle -= angleIncrease;
        }

        // Optionally close the spotlight shape
        Gizmos.DrawLine(prevPoint, origin);
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        _dynamicMesh = null;
    }

    private void ReDraw()
    {
        if (_dynamicMesh == null)
        {
            _dynamicMesh = new Mesh() { name = "Generated Mesh" };
            meshFilter.mesh = _dynamicMesh;
        }

        _dynamicMesh.Clear();
        GenerateMesh(_dynamicMesh);
    }

    public void GenerateMesh(Mesh mesh)
    {
        float angle = transform.rotation.eulerAngles.z + (fovDegrees / 2);
        float angleIncrease = fovDegrees / raycastCount;

        Vector3[] vertices = new Vector3[raycastCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[raycastCount * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0, 0);

        int verticesIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= raycastCount; i++)
        {
            Vector3 raycastDir = MathUtilities.RotationToVector2(angle);
            Vector3 vertex;

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, raycastDir, maxDistance, layerMask);
            if (raycastHit.collider == null)
            {
                // No Hit
                vertex = raycastDir * maxDistance;
            }
            else
            {
                // Hit
                vertex = new Vector3(raycastHit.point.x, raycastHit.point.y, transform.position.z) - (transform.position);
            }

            Quaternion reverseRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);
            vertex = reverseRotation * vertex;

            vertices[verticesIndex] = vertex;
            uv[verticesIndex] = Vector2.right * (vertex.magnitude / maxDistance);

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = verticesIndex - 1;
                triangles[triangleIndex + 2] = verticesIndex;
                triangleIndex += 3;
            }

            verticesIndex++;

            angle -= angleIncrease;
        }



        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    // Start is called before the first frame update
    void Start()
    {
        ReDraw();
    }

    private void LateUpdate()
    {
        if (autoRedraw && HasMoved())
        {
            ReDraw();
        }
    }

    private bool HasMoved()
    {
        if (transform.position != _lastPosition || transform.rotation != _lastRotation)
        {
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
            return true;
        }
        return false;
    }
}
