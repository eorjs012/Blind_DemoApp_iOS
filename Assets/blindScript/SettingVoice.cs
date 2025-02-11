using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.Accessibility;

public class SettingVoice : MonoBehaviour
{
    private void OnEnable()
    {
        if (SmartSecurity.GetInt("checksettingindex") == 0)
        {
            SmartSecurity.SetInt("checksettingindex", 1);
            RoadController.Instance.startsetting();
        }
     
    }
   
}
