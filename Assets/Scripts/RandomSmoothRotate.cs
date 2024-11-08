using UnityEngine;

public class RandomSmoothRotate : MonoBehaviour
{
    public float rotateSpeedMin;
    public float rotateSpeedMax;
    public float speedChangeWaveLength;
    public float axisRotateSpeed;

    private Vector3 rotateAxis = Vector3.up;
    private Vector3 targetRotateAxis = Vector3.up;

    private float currentRotateSpeed
    {
        get
        {
            float yValue = Mathf.Sin((Time.time / speedChangeWaveLength) * 2 * Mathf.PI);
            return Mathf.Lerp(rotateSpeedMin, rotateSpeedMax, (yValue + 1) / 2);
        }
    }

    private void Update()
    {
        transform.Rotate(rotateAxis, currentRotateSpeed * Time.deltaTime);

        if(Vector3.Distance(targetRotateAxis, rotateAxis) < 0.01f )
        {
            targetRotateAxis = Random.onUnitSphere;
        }

        rotateAxis = Vector3.Slerp(rotateAxis, targetRotateAxis, Time.deltaTime * axisRotateSpeed);
    }
}
