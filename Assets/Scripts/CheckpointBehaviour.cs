using UnityEngine;
using UnityEngine.Events;

public class CheckpointBehaviour : MonoBehaviour
{
    [SerializeField] private CheckpointManager _manager = null;
    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeactivate;
    [SerializeField] private bool isActivated;

    private void OnValidate()
    {
        if (this.enabled && _manager == null)
        {
            _manager = transform.GetComponentInParent<CheckpointManager>();
            if (_manager == null)
            {
                Debug.LogWarning("Cannot find CheckpointManager in parent! Component disabled.");
                enabled = false;
            }
        }
    }
    public void Activate()
    {
        if (isActivated) return;
        isActivated = true;
        _manager.DeactivateAll();
        _manager.SetCheckpoint(this);
        OnActivate.Invoke();
    }

    public void Deactivate()
    {
        if (!isActivated) return;
        isActivated = false;
        OnDeactivate.Invoke();
    }

    private void OnEnable()
    {
        
    }
}
