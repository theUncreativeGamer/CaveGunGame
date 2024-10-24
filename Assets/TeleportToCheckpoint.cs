using UnityEngine;

public class TeleportToCheckpoint : MonoBehaviour
{
    public CheckpointManager checkpointManager;

    private void Awake()
    {
        int checkpointIndex = PlayerPrefs.GetInt(CheckpointManager.PlayerPrefKey, -1);
        if (checkpointIndex >= 0)
        {
            transform.position = checkpointManager.Checkpoints[checkpointIndex].transform.position;
        }
    }
}
