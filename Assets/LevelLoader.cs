using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    
    public LevelList levelList;
    public UnityEvent OnSaveDetected;

    private void Start()
    {
        if (PlayerPrefs.HasKey(LevelList.PPK_SavedLevel)) OnSaveDetected.Invoke();
    }

    public static void ClearData()
    {
        LevelList.ClearData();
    }

    public void LoadSavedLevel()
    {
        levelList.LoadSavedLevel();
    }

    public void LoadLevel(SceneObject level)
    {
        levelList.LoadLevel(level);
    }
}
