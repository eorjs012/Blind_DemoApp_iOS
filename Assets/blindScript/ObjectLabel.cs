using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectLabel : MonoBehaviour
{
    private static ObjectLabel instance;

    public static ObjectLabel Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<ObjectLabel>();
            if (instance != null) return instance;
            var container = new GameObject("ObjectLabel");
            return instance;
        }
    }
    public Text objTt;
    public int objint;
    void Start()
    {
       
    }
    
    
  
}

