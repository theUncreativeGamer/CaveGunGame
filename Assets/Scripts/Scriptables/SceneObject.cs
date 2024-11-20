using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(order = 5)]
public class SceneObject : ScriptableObject
{
    ///Implicitly converts a scene object into a Scene reference.
    public static implicit operator Scene(SceneObject self)
    {
        return self.Scene;
    }
    /// Implicitly converts a scene object to a string containing the scene name.
    public static implicit operator string(SceneObject self)
    {
        return self.sceneName;
    }
    // -------------------------------------------------------------
    public string sceneName;
    public Scene Scene { get => SceneManager.GetSceneByName(sceneName); }

#if UNITY_EDITOR
    public UnityEditor.SceneAsset sceneAsset;
    // -------------------------------------------------------------

    public void OnValidate()
    {
        sceneName = sceneAsset.name;
    }
#endif

}