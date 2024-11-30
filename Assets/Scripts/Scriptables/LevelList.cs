using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(order = 5)]
public class LevelList : ScriptableObject
{
    public const string PPK_SavedLevel = "SavedLevel";
    public List<SceneObject> levels = new List<SceneObject>();

    public SceneObject GetLevel(string sceneName)
    {
        return levels.Find(scene => scene.sceneName == sceneName);
    }

    public static void ClearData()
    {
        PlayerPrefs.DeleteKey(PPK_SavedLevel);
        Debug.Log("Level data cleared!");
    }

    Scene GetLevelScene()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name.StartsWith("Level_")) // Adjust the condition as needed
            {
                return scene;
            }
        }
        return default; // Return default if no matching scene is found
    }

    public void SaveCurrentLevel()
    {
        string sceneName = GetLevelScene().name;
        if (GetLevel(sceneName) == null)
        {
            Debug.LogError($"Cannot find level with name {sceneName} in {name}!");
            return;
        }
        PlayerPrefs.SetString(PPK_SavedLevel, sceneName);
        Debug.Log("Level saved.");
    }

    public void LoadSavedLevel()
    {
        string levelId = PlayerPrefs.GetString(PPK_SavedLevel);
        if (string.IsNullOrEmpty(levelId))
        {
            Debug.LogError("Old save doesn't exist!");
            return;
        }

        SceneObject searchResult = GetLevel(levelId);
        if (searchResult == null)
        {
            Debug.LogError($"Level with id \"{levelId}\" doesn't exist in {name}!");
            return;
        }

        SceneManager.LoadScene(searchResult);
    }

    public void LoadLevel(SceneObject level)
    {
        SceneManager.LoadScene(level);
    }
}
