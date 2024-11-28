using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError($"Multiple instances of {nameof(CheckpointManager)} are loaded at the same time.");
        }
        instance = this;
    }


    public static readonly string PlayerPrefKey = "CheckpointIndex";

    [SerializeField] private bool resetCheckpoint = false;
    [SerializeField] private List<CheckpointBehaviour> checkpoints;
    [SerializeField] private Transform player;

    private Transform Player 
    {  
        get 
        { 
            if( player == null ) player = PlayerSingleInstance.Instance.transform;
            return player;
        } 
    }

    public List<CheckpointBehaviour> Checkpoints { get { return checkpoints; } }

    public bool SetCheckpoint(CheckpointBehaviour checkpoint)
    {
        int index = checkpoints.FindIndex(obj => obj == checkpoint);
        if (index < 0)
        {
            Debug.LogWarning($"Set checkpoint failed: checkpoint {checkpoint.gameObject.name} " +
                $"at {checkpoint.transform.position} doesn't belong in {this.gameObject.name}");
            return false;
        }
        PlayerPrefs.SetInt(PlayerPrefKey, index);
        return true;
    }

    public void DeactivateAll()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.Deactivate();
        }
    }

    public void ResetCheckpoint()
    {
        PlayerPrefs.DeleteKey(PlayerPrefKey);
    }

    public void TeleportToNextCheckpoint()
    {
        int checkpointIndex = PlayerPrefs.GetInt(CheckpointManager.PlayerPrefKey, -1) + 1;
        if (checkpointIndex >= Checkpoints.Count) checkpointIndex -= Checkpoints.Count;

        Player.position = Checkpoints[checkpointIndex].transform.position;
    }

    private void OnValidate()
    {
        if (resetCheckpoint)
        {
            resetCheckpoint = false;
            ResetCheckpoint();
        }
    }
}
