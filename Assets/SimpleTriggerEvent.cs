using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class SimpleTriggerEvent : MonoBehaviour
{
    public List<string> targetTags = new List<string>();
    public UnityEvent<Collider2D> OnEnterEvent = null;
    public UnityEvent<Collider2D> OnStayEvent = null;
    public UnityEvent<Collider2D> OnExitEvent = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTags(targetTags)) OnEnterEvent?.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTags(targetTags)) OnStayEvent?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTags(targetTags)) OnExitEvent?.Invoke(collision);
    }
}
