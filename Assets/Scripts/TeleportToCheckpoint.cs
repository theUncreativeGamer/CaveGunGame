using UnityEngine;

public class TeleportToCheckpoint : MonoBehaviour
{
    private void Start()
    {
        int checkpointIndex = PlayerPrefs.GetInt(CheckpointManager.PPK_Checkpoint, -1);
        if (checkpointIndex >= 0)
        {
            transform.position = CheckpointManager.instance.Checkpoints[checkpointIndex].transform.position;
        }
    }
}
