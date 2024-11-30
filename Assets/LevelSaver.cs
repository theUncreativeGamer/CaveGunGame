using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{
    public LevelList LevelList;

    private void Start()
    {
        Debug.Log("Saving level...");
        LevelList.SaveCurrentLevel();
    }
}
