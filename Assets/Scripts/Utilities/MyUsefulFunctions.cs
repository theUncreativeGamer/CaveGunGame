using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyUsefulFunctions 
{
    public static bool CompareTags(this Component component, List<string> tags)
    {
        foreach (string tag in tags)
        {
            if (component.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }


}
