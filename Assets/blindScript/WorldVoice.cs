using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.Accessibility;

public class WorldVoice : MonoBehaviour
{
    private void OnEnable()
    {
        RoadController.Instance.startworld();
    }
   
}
