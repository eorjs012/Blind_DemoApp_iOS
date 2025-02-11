using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Apple.Accessibility;

public class BotPanl : MonoBehaviour 
{
    public GameObject[] botbtn = new GameObject[4];
    public GameObject[] botpanel = new GameObject[4];

    void Start()
    {
        if(SmartSecurity.GetInt("changescene") == 1)
        {
            BotMainPanel(2);
        }
        else
        {
            BotMainPanel(0);
        }
       
    }
   
    public void BotMainPanel(int index)
    {
        for (int i = 0; i <4; i++)
        {
           // botbtn[i].SetActive(true);
            botpanel[i].SetActive(false);
        }
        if (index == 0) //홈.
        {
          
        }
        else if (index == 1) // 캐릭터.
        {
            //startavata();
        }
        else if (index == 2) //월드.
        {
           
        }
        else if (index == 3)//세팅
        {
            RoadController.Instance.privacychangebackbtn();
            RoadController.Instance.settingpanelstart();
        }
        botpanel[index].SetActive(true);
    }


    // 뒤로가기버튼 이벤트 

    public void worldbackbtn() //월드패널 뒤로가기(홈화면) 버튼
    {
        BotMainPanel(0);
    }
    public void avatabackbtn() //아바타패널 뒤로가기(홈화면) 버튼
    {
        BotMainPanel(0);
    }
  

  
}
