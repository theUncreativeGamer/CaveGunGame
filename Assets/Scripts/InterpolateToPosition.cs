using UnityEngine;

public class InterpolateToPosition : MonoBehaviour
{
    public AnimationCurve interpolationCurve = new AnimationCurve();
    public bool changeParent;
    public Transform newParent;
    [Space(5)]
    public Vector3 newPosition;
    public Vector3 newRotation;
    public Vector3 newScale = Vector3.one;
    public float duration;

    private Quaternion NewRotationQuaternion { get => Quaternion.Euler(newRotation); }

    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private Vector3 oldScale;
    private bool isRunning = false;
    private float timer = 0;

    private bool useRectTransform = false;
    private RectTransform rectTransform = null;

    public void Play()
    {
        if (changeParent) transform.parent = newParent;
        if (useRectTransform) oldPosition = rectTransform.anchoredPosition;
        else oldPosition = transform.localPosition;
        oldRotation = transform.localRotation;
        oldScale = transform.localScale;
        isRunning = true;
        timer = 0;
    }

    private void Awake()
    {
        if (TryGetComponent<RectTransform>(out rectTransform)) useRectTransform = true;
    }

    private void Update()
    {
        if (!isRunning) return;
        timer += Time.deltaTime;

        if (useRectTransform) UpdateRectTransform();
        else UpdateNormalTransform();
    }

    private void UpdateNormalTransform()
    {
        if (timer >= duration)
        {
            transform.SetLocalPositionAndRotation(newPosition, NewRotationQuaternion);
            transform.localScale = newScale;
            isRunning = false;
            return;
        }

        float progress = interpolationCurve.Evaluate(timer / duration);
        Vector3 nextPosition = Vector3.Lerp(oldPosition, newPosition, progress);
        Quaternion nextRotation = Quaternion.Lerp(oldRotation, NewRotationQuaternion, progress);
        Vector3 nextScale = Vector3.Lerp(oldScale, newScale, progress);
        transform.SetLocalPositionAndRotation(nextPosition, nextRotation);
        transform.localScale = nextScale;
    }

    private void UpdateRectTransform()
    {
        if (timer >= duration)
        {
            rectTransform.anchoredPosition = newPosition;
            rectTransform.localRotation = NewRotationQuaternion;
            rectTransform.localScale = newScale;
            isRunning = false;
            return;
        }

        float progress = interpolationCurve.Evaluate(timer / duration);
        Vector3 nextPosition = Vector3.Lerp(oldPosition, newPosition, progress);
        Quaternion nextRotation = Quaternion.Lerp(oldRotation, NewRotationQuaternion, progress);
        Vector3 nextScale = Vector3.Lerp(oldScale, newScale, progress);
        rectTransform.anchoredPosition = nextPosition;
        rectTransform.localRotation = nextRotation;
        rectTransform.localScale = nextScale;
    }
}
