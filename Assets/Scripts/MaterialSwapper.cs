using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    public List<Material> materials;

    public void SetMaterial(int index)
    {
        if (index < 0 || index >= materials.Count)
        {
            Debug.LogError("Index out of bounds");
            return;
        }
        renderer.material = materials[index];
    }
}
