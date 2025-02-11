using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    public GameObject exitpanel;

    public void exit()
    {
        exitpanel.SetActive(false);
    }
}
