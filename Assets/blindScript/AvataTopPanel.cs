using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvataTopPanel : MonoBehaviour
{
    public GameObject[] AvataBtn = new GameObject[2]; //추후 index ++ (3).
    public GameObject[] AvataPanel = new GameObject[2];
    void Start()
    {
        AvataMainPanel(0);
    }

   public void AvataMainPanel(int index)
   {
        if (index == 0)
        {
            AvataPanel[0].SetActive(true);
            AvataPanel[1].SetActive(false);
        }
        else if (index == 1)
        {
            AvataPanel[0].SetActive(false);
            AvataPanel[1].SetActive(true);
        }
        else if (index == 2)
        {

        }
        else if (index == 3)
        {

        }
        DataController.Instance.avatapanelindex = index;
   }
    
}
