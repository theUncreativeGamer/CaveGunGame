using UnityEngine;

public class CircularProgressBar : MonoBehaviour
{
    [SerializeField] Transform leftSpriteMask;
    [SerializeField] Transform rightSpriteMask;

    public float minValue = 0;
    public float maxValue = 1;
    [SerializeField] float currentValue;

    private void OnValidate()
    {
        if (leftSpriteMask != null && rightSpriteMask != null)
            SetValue(currentValue);
    }

    public void SetValue(float value)
    {
        currentValue = value;
        float progress = (value-minValue)/(maxValue-minValue);

        float leftProgress = Mathf.Clamp(progress, 0f, 0.5f);
        float leftMaskRotationDegree = -180f + 360f * leftProgress;
        leftSpriteMask.localRotation = Quaternion.Euler(
            leftSpriteMask.localRotation.x, 
            leftSpriteMask.localRotation.y, 
            leftMaskRotationDegree
            );

        float rightProgress = Mathf.Clamp(progress, 0.5f, 1f);
        float rightMaskRotationDegree = 360f * rightProgress;
        rightSpriteMask.localRotation = Quaternion.Euler(
            rightSpriteMask.localRotation.x,
            rightSpriteMask.localRotation.y,
            rightMaskRotationDegree
            );
    }
}
