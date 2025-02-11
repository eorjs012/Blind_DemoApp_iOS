using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.Accessibility;

public class AvataVoice : MonoBehaviour
{
    private void OnEnable()
    {
        RoadController.Instance.startavata();

    }
   
}
