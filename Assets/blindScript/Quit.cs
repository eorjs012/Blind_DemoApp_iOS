using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Quit : MonoBehaviour
{
    public GameObject quit;
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                quit.SetActive(true);
                //종료패널켜기
            }
        }
    }
}
