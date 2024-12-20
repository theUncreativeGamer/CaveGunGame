using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAlertDistance : MonoBehaviour
{
    public Collider2D alertTarget;
    public float alertDistance = 30;
    public float forgetDistance = 50;
    public UnityEvent alertEvent;
    public UnityEvent forgetEvent;

    public bool isAlerted;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, alertDistance);

        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, forgetDistance);
    }
#endif

    public void Alert()
    {
        if (!isAlerted)
        {
            isAlerted = true;
            alertEvent.Invoke();
        }
    }

    private void Start()
    {
        isAlerted = false;
    }

    private void Update()
    {
        if (alertTarget == null)
        {
            if (PlayerSingleInstance.HitpointInstance != null)
                alertTarget = PlayerSingleInstance.HitpointInstance.GetComponent<Collider2D>();
            else return;
        }
        if (isAlerted)
        {
            if (Vector2.Distance(alertTarget.bounds.center, transform.position) > forgetDistance)
            {
                isAlerted = false;
                forgetEvent.Invoke();
            }
        }
        else
        {
            if (Vector2.Distance(alertTarget.bounds.center, transform.position) < alertDistance)
            {
                isAlerted = true;
                alertEvent.Invoke();
            }
        }

    }
}
