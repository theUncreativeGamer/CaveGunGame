using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(MeshFilter), typeof(MyRotation))] 
public class FlashEffect : MonoBehaviour
{
    public LayerMask layerMask;
    public float fovDegrees = 30f;
    [Tooltip("Higher the value, higher the cost and accuracy.")]
    public int raycastCount = 10;
    public float maxDistance = 300f;
    [Tooltip("Enable this to automatically redraw in every LateUpdate.")]
    public bool autoRedraw = false;

    private MeshFilter meshFilter;
    private MyRotation theRotationComponent;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        theRotationComponent = GetComponent<MyRotation>();
    }

    public void ReDraw()
    {
        float myRotation = theRotationComponent.Value;

        float angle = myRotation + (fovDegrees / 2);
        float angleIncrease = fovDegrees / raycastCount;
        Vector3 origin = Vector3.zero;

        Vector3[] vertices = new Vector3[raycastCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[raycastCount * 3];

        vertices[0] = origin;
        uv[0] = new Vector2(0, 0);

        int verticesIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= raycastCount; i++)
        {
            Vector3 raycastDir = MathUtilities.RotationToVector2(angle);
            Vector3 vertex;

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position + origin, raycastDir, maxDistance, layerMask);
            if(raycastHit.collider == null)
            {
                // No Hit
                vertex = origin + raycastDir * maxDistance;
            }
            else
            {
                // Hit
                vertex = new Vector3(raycastHit.point.x, raycastHit.point.y, transform.position.z) - (transform.position + origin);
            }
            vertices[verticesIndex] = vertex;
            uv[verticesIndex] = Vector2.right * (vertex.magnitude / maxDistance);

            if(i>0) 
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = verticesIndex - 1;
                triangles[triangleIndex + 2] = verticesIndex;
                triangleIndex += 3;
            }

            verticesIndex++;

            angle -= angleIncrease;
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.uv = uv;
        meshFilter.mesh.triangles = triangles;
    }

    // Start is called before the first frame update
    void Start()
    {
        ReDraw();
    }

    private void LateUpdate()
    {
        if(autoRedraw)
        {
            ReDraw();
        }
    }
}
