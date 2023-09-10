using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(MeshFilter), typeof(MyRotation))]
public class BeamEffect : MonoBehaviour
{
    public LayerMask layerMask;
    public float beamWidth = 30f;
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
        float angle = theRotationComponent.Value;
        Vector2 raycastDir = MathUtilities.RotationToVector2(angle);
        Vector2 leftward = MathUtilities.RotationToVector2(angle + 90) * beamWidth / 2;

        Vector3 endPoint;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, raycastDir, maxDistance, layerMask);
        if (raycastHit.collider == null)
        {
            // No Hit
            endPoint = raycastDir * maxDistance;
        }
        else
        {
            // Hit
            endPoint = new Vector3(raycastHit.point.x, raycastHit.point.y, transform.position.z) - (transform.position);
        }



        float completeness = raycastHit.distance / maxDistance;

        Vector3[] vertices = {  new Vector3(leftward.x, leftward.y, transform.position.z),
                                endPoint + (Vector3)leftward,
                                new Vector3(-leftward.x, -leftward.y, -transform.position.z),
                                endPoint - (Vector3)leftward};
        Vector2[] uv = { new Vector2(0, 0), new Vector2(completeness, 0), new Vector2(0, 1), new Vector2(completeness, 1) };
        int[] triangles = { 0, 1, 2, 2, 1, 3 };

        //Debug.Log("Trying to draw a quad with these vertices: " + vertices[0] + vertices[1] + vertices[2] + vertices[3]);

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.uv = uv;
        meshFilter.mesh.triangles = triangles;
    }

    void Start()
    {
        ReDraw();
    }

    private void LateUpdate()
    {
        if (autoRedraw)
        {
            ReDraw();
        }
    }
}
