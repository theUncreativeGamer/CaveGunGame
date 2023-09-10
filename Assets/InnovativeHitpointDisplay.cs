using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovativeHitpointDisplay : MonoBehaviour
{
    public GameObject heartPrefab;
    public List<Vector2> heartPositions;
    public int heartCount;

    private List<GameObject> sprites = new List<GameObject>();

    public void UpdateDisplay()
    {
        foreach(var stuff in sprites)
        {
            stuff.SetActive(false);
        }
        sprites.Clear();

        for(int i = 0; i < heartCount; i++)
        {
            if (i >= heartPositions.Count) break;
            if(i<sprites.Count)
            {
                sprites[i].SetActive(true);
            }
            else
            {
                GameObject newSpriteObject = Instantiate(heartPrefab, (Vector2)transform.position + heartPositions[i], Quaternion.identity, transform);
                sprites.Add(newSpriteObject);
            }
        }
    }

    public void OnUpdateHitpoint(float hitPoint)
    {
        //Debug.Log("Ouch!");
        heartCount = Mathf.CeilToInt(hitPoint);
        UpdateDisplay();
    }

    private void Start()
    {
        UpdateDisplay();
    }
}
