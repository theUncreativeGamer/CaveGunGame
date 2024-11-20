using UnityEngine;

public class TeleportToCheckpoint : MonoBehaviour
{
    private void Awake()
    {
        int checkpointIndex = PlayerPrefs.GetInt(CheckpointManager.PlayerPrefKey, -1);
        if (checkpointIndex >= 0)
        {
            transform.position = CheckpointManager.instance.Checkpoints[checkpointIndex].transform.position;
        }
    }
}
