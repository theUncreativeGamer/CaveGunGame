using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public struct MoveObjectInstruction
{
    public string objectName;
    public Transform destination;
}

public class LevelEssentialsLoader : MonoBehaviour
{
    public SceneObject SceneToLoad;
    public List<MoveObjectInstruction> PrefabsToMove;
    public UnityEvent OnSceneLoaded;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);
        Debug.Log(SceneToLoad.name + " loaded.");
        MoveObjects();
        OnSceneLoaded?.Invoke();
    }

    private void MoveObjects()
    {
        foreach (MoveObjectInstruction instruction in PrefabsToMove)
        {
            var obj = GameObject.Find(instruction.objectName);
            if (obj == null)
            {
                Debug.LogWarning($"Can't find {instruction.objectName}.");
                continue;
            }
            obj.transform.position = instruction.destination.position;
        }
    }
}
