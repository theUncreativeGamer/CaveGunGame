using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This exist solely because there are some problems with the rotations in 
// transform when I try to draw meshes.
// This MonoBehaviour is kinda stupid, I know.
public class MyRotation : MonoBehaviour
{
    private float _daRotato = 0;

    public float Value { 
        get
        {
            _daRotato += transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.identity;
            return _daRotato;
        } 
    }
    
}
