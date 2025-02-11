using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apple.Accessibility;

public class AvataPanel : MonoBehaviour
{
    private static AvataPanel instance;

    public static AvataPanel Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<AvataPanel>();
            if (instance != null) return instance;
            var container = new GameObject("AvataPanel");
            return instance;
        }
    }

    void Start()
    {
        startavatasetting();
        startblindlabel();
        wmmainpanel(DataController.Instance.wmsureindex);
        wface[DataController.Instance.witemfaceindex].SetActive(true);
        checkhair();
        wemotion[DataController.Instance.witememotionindex].SetActive(true);
        wtop[DataController.Instance.witemtopindex].SetActive(true);
        face[DataController.Instance.itemfaceindex].SetActive(true);
        emotion[DataController.Instance.itememotionindex].SetActive(true);
        top[DataController.Instance.itemtopindex].SetActive(true);
      
    }
    public void startavatasetting()
    {
        DataController.Instance.wmindex = DataController.Instance.wmsureindex;

        DataController.Instance.itememotionindex = DataController.Instance.itememotion;
        DataController.Instance.itemfaceindex = DataController.Instance.itemface;
        DataController.Instance.itemtopindex = DataController.Instance.itemtop;

        DataController.Instance.witememotionindex = DataController.Instance.witememotion;
        DataController.Instance.witemfaceindex = DataController.Instance.witemface;
        DataController.Instance.witemtopindex = DataController.Instance.witemtop;
    }
    public GameObject[] wmbtn = new GameObject[2];
    public GameObject[] WMsettingpanel = new GameObject[2];
    public GameObject[] wimage = new GameObject[2];
    public GameObject[] mimage = new GameObject[2];


    //애플 플러그인 보이스오버 여성이벤트버튼  0 왼쪽 , 1 오른쪽
    public GameObject[] wemotionbtn = new GameObject[2];
    public GameObject[] wfacebtn = new GameObject[2];
    public GameObject[] wtopbtn = new GameObject[2];

    //애플 플러그인 보이스오버 남성이벤트버튼   0 왼쪽 , 1 오른쪽
    public GameObject[] emotionbtn = new GameObject[2];
    public GameObject[] facebtn = new GameObject[2];
    public GameObject[] topbtn = new GameObject[2];

    //아바타 저장버튼 보이스오버 이벤트
    public GameObject surebtn;



    public void wmmainpanel(int index) //성별
    {
        RoadTTS.Instance.audioSource.Stop();
        for(int i=0; i<2; i++)
        {
            wmbtn[i].SetActive(false);
            WMsettingpanel[i].SetActive(false);
            wimage[i].SetActive(false);
            mimage[i].SetActive(false);
        }
        if (index == 0)  //여성선택.
        {
            
        }
        else if (index == 1) //남성선택..
        {

        }
        wmbtn[index].SetActive(true);
        WMsettingpanel[index].SetActive(true);
        wimage[index].SetActive(true);
        mimage[index].SetActive(true);
        DataController.Instance.wmindex = index;
    }

    public GameObject blindwbtn;
    public GameObject blindmbtn;
  
    public void wbtn()// 여성선택버튼  
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 성별 선택 활성화";
            Invoke("blindwbtnon", 0.5f);
        }
        
    }

    public void blindwbtnon() //보이스오버 여성선택버튼 라벨변경 
    {
        blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 성별 선택 버튼 활성화 상태 입니다";
        blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 성별 선택 버튼 비 활성화 상태 입니다";
        CancelInvoke("blindwbtnon");
    }
  
    public void mbtn()// 남성선택버튼  
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 성별 선택 활성화";
            Invoke("blindmbtnon", 0.5f);
        }
    }

    public void blindmbtnon()  //보이스오버 남성선택버튼 라벨변경
    {
        blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 성별 선택 버튼 활성화 상태 입니다";
        blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 성별 선택 버튼 비 활성화 상태 입니다";
        CancelInvoke("blindmbtnon");
    }

    public void startblindlabel()
    {
        if (DataController.Instance.wmindex == 0)
        {
            blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 성별 선택 버튼 활성화 상태 입니다";
            blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 성별 선택 버튼 비 활성화 상태 입니다";
        }
        else
        {
            blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 성별 선택 버튼 활성화 상태 입니다";
            blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 성별 선택 버튼 비 활성화 상태 입니다";
        }
    }
    public void avatasurebtn()
    {
        RoadTTS.Instance.audioSource.Stop();
        SmartSecurity.SetInt("wmsureindex", DataController.Instance.wmindex);
        SmartSecurity.SetInt("itememotion", DataController.Instance.itememotionindex);
        SmartSecurity.SetInt("itemface", DataController.Instance.itemfaceindex);
        SmartSecurity.SetInt("itemtop", DataController.Instance.itemtopindex);
        SmartSecurity.SetInt("witememotion", DataController.Instance.witememotionindex);
        SmartSecurity.SetInt("witemtop", DataController.Instance.witemtopindex);
        SmartSecurity.SetInt("witemface", DataController.Instance.witemfaceindex);
        avatacheckTt();
        checksendcharimageindex();
        BackEndController.Instance.InsertPublicItemData();
        BackEndController.Instance.InsertPrivateData();
        //저장이 완료됐습니다(TTS).
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            surebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장 버튼 아바타가 저장 되었습니다";
        }
        Invoke("avatasurettson", 1f);
    }

    public void avatasurettson()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            surebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장 버튼";
        }
    }

    public void checksendcharimageindex()
    {
        if (SmartSecurity.GetInt("wmsureindex") == 0) //여성캐릭터.
        {
            if (SmartSecurity.GetInt("witemface") == 0)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 1);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 2);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 3);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 4);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 5);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 6);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 7);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 8);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 9);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 10);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 1)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 11);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 12);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 13);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 14);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 15);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 16);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 17);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 18);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 19);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 20);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 2)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 21);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 22);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 23);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 24);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 25);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 26);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 27);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 28);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 29);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 30);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 3)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 31);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 32);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 33);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 34);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 35);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 36);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 37);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 38);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 39);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 40);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 4)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 41);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 42);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 43);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 44);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 45);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 46);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 47);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 48);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 49);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 50);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 5)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 51);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 52);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 53);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 54);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 55);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 56);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 57);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 58);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 59);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 60);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 6)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 61);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 62);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 63);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 64);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 65);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 66);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 67);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 68);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 69);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 70);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 7)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 71);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 72);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 73);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 74);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 75);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 76);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 77);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 78);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 79);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 80);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 8)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 81);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 82);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 83);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 84);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 85);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 86);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 87);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 88);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 89);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 90);
                }
            }
            else if (SmartSecurity.GetInt("witemface") == 9)
            {
                if (SmartSecurity.GetInt("witemtop") == 0)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 91);
                }
                else if (SmartSecurity.GetInt("witemtop") == 1)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 92);
                }
                else if (SmartSecurity.GetInt("witemtop") == 2)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 93);
                }
                else if (SmartSecurity.GetInt("witemtop") == 3)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 94);
                }
                else if (SmartSecurity.GetInt("witemtop") == 4)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 95);
                }
                else if (SmartSecurity.GetInt("witemtop") == 5)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 96);
                }
                else if (SmartSecurity.GetInt("witemtop") == 6)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 97);
                }
                else if (SmartSecurity.GetInt("witemtop") == 7)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 98);
                }
                else if (SmartSecurity.GetInt("witemtop") == 8)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 99);
                }
                else if (SmartSecurity.GetInt("witemtop") == 9)
                {
                    SmartSecurity.SetInt("dotwcharimageindex", 100);
                }
            }
        }
        else if (SmartSecurity.GetInt("wmsureindex") == 1) //남성캐릭터.
        {
            if (SmartSecurity.GetInt("itemface") == 0)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 1);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 2);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 3);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 4);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 5);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 6);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 7);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 8);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 9);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 10);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 1)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 11);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 12);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 13);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 14);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 15);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 16);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 17);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 18);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 19);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 20);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 2)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 21);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 22);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 23);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 24);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 25);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 26);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 27);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 28);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 29);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 30);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 3)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 31);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 32);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 33);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 34);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 35);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 36);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 37);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 38);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 39);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 40);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 4)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 41);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 42);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 43);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 44);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 45);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 46);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 47);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 48);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 49);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 50);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 5)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 51);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 52);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 53);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 54);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 55);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 56);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 57);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 58);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 59);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 60);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 6)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 61);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 62);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 63);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 64);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 65);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 66);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 67);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 68);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 69);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 70);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 7)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 71);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 72);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 73);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 74);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 75);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 76);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 77);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 78);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 79);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 80);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 8)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 81);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 82);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 83);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 84);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 85);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 86);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 87);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 88);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 89);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 90);
                }
            }
            else if (SmartSecurity.GetInt("itemface") == 9)
            {
                if (SmartSecurity.GetInt("itemtop") == 0)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 91);
                }
                else if (SmartSecurity.GetInt("itemtop") == 1)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 92);
                }
                else if (SmartSecurity.GetInt("itemtop") == 2)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 93);
                }
                else if (SmartSecurity.GetInt("itemtop") == 3)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 94);
                }
                else if (SmartSecurity.GetInt("itemtop") == 4)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 95);
                }
                else if (SmartSecurity.GetInt("itemtop") == 5)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 96);
                }
                else if (SmartSecurity.GetInt("itemtop") == 6)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 97);
                }
                else if (SmartSecurity.GetInt("itemtop") == 7)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 98);
                }
                else if (SmartSecurity.GetInt("itemtop") == 8)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 99);
                }
                else if (SmartSecurity.GetInt("itemtop") == 9)
                {
                    SmartSecurity.SetInt("dotmcharimageindex", 100);
                }
            }
        }

    }

    public void avatacheckTt() //아바타 아이템설명
    {
        if (SmartSecurity.GetInt("wmsureindex") == 0) //여성캐릭터.
        {
            var wcharemotionTt = stemotion[DataController.Instance.witememotionindex];
            var wcharfaceTt = wstface[DataController.Instance.witemfaceindex];
            var wchartopTt = wsttop[DataController.Instance.witemtopindex];
            SmartSecurity.SetString("iteminfoTt", wcharemotionTt + "에 " + wcharfaceTt + "에 " + wchartopTt);
        }
        else if (SmartSecurity.GetInt("wmsureindex") == 1) //남성캐릭터.
        {
            var mcharemotionTt = stemotion[DataController.Instance.itememotionindex];
            var mcharfaceTt = mstface[DataController.Instance.itemfaceindex];
            var mchartopTt = msttop[DataController.Instance.itemtopindex];
            SmartSecurity.SetString("iteminfoTt", mcharemotionTt + "에 " + mcharfaceTt + "에 " + mchartopTt);
        }
    }
    public void backpanel(int index)
    {
        AvataTopPanel avata = new AvataTopPanel();
        avata.AvataMainPanel(DataController.Instance.avatapanelindex);
        DataController.Instance.avatapanelindex = index;
    }

    public GameObject[] emotion = new GameObject[30];
    public GameObject[] face = new GameObject[10];
    public GameObject[] top = new GameObject[10];
    #region 아바타 데이터 저장
    public void changeavataui()
    {
        BackEndController.Instance.InsertPublicItemData();
    }
    #endregion

    #region 남성 아바타 선택
    string[] stemotion = {"기쁨-웃음 감정", "슬픔-웃음 감정", "당황-웃음 감정", "화남-웃음 감정", "역겨움-웃음 감정", "중립-웃음 감정",
                           "기쁨-울음 감정","슬픔-울음 감정", "당황-울음 감정", "화남-울음 감정","역겨움-울음 감정", "중립-울음 감정",
                           "기쁨-외침 감정", "슬픔-외침 감정","당황-외침 감정","화남-외침 감정","역겨움-외침 감정", "중립-외침 감정",
                           "기쁨-침묵 감정", "슬픔-침묵 감정","당황-침묵 감정" ,"화남-침묵 감정","역겨움-침묵 감정", "중립-침묵 감정",
                           "기쁨-두근 감정","슬픔-두근 감정", "당황-두근 감정", "화남-두근 감정","역겨움-두근 감정", "중립-두근 감정"

    };
    string[] mstface = {"하얀색의 웨이브 앞 머리 스타일에 파란 눈의 이국적인 얼굴",
                        "브라운색 2대8 가름마 스타일에 브라운 색 눈이 어울리는 얼굴",
                        "민트색의 웨이브가 있는 갈라진 앞머리와 민트 색 눈이 잘 어울리는 눈에 띄는 얼굴",
                        "카키색의 2대8 가름마 앞머리에 웨이브가 있는 비스듬한 앞 머리와 밝은 갈색 눈이 어울리는 얼굴",
                        "검은색 2대8 가르마 펌에 초녹 눈이 어울리는 얼굴",
                        "검은색 3대7 가르마 펌에 검은 눈이 어울리는 차분한 얼굴",
                        "뒤로 넘겨 살짝 묶은 올 백 머리 스타일에 연 보라빛 눈이 어울리는 개성있는 얼굴",
                        "빨간 비니를 쓴 머리에 까만 눈이 어울리는 스타일 리쉬한 얼굴",
                        "양 옆으로 살짝 내려온 검은색 앞머리에 짙은 갈색 눈동자가 어울리는 얼굴",
                        "검은색 댄디펌에 짙은 회색 눈이 어울리는 얼굴"
    };
    string[] msttop = { "회색 계열 블루 종 재킷에 회색 조거 팬츠와 운동화를 깔 맞춤한 복장",
                        "후드가 달린 야구 잠바 재킷에 청 회색 진을 매칭한 캐주얼한 복장",
                        "주황색 셔츠에 깔끔한 검은 넥타이를 매칭한 연 갈색의 세로 줄무늬가 있는 짙은 갈색 면 바지 복장",
                        "검정 블루 종 재킷에 밝은 녹색 셔츠와 흰 면티 그리고 검은 바지와 빨간색 운동화를 매칭한 스타일 리쉬한 복장",
                        "검정 무채색의 야구 잠바 자켓에 하얀 면티와 검은 반 바지를 매칭한 가벼운 복장",
                        "초록색과 베이지가 어울린 야구 잠바 자켓에 검은 바지를 매칭한 캐주얼한 복장",
                        "멋진 갈색 가디건에 세로 줄무늬가 있는 갈색 티셔츠와 연 갈색 바지와 갈색 구두로 댄디한 스타일 복장",
                        "가을 느낌나는 트렌치 롱 코트에 하얀셔츠와 검정 정장 바지 그리고 검정 구두를 매칭한 깔끔한 복고풍 복장",
                        "앞에 주머니가 달린 분홍색과 하얀색의 야구 점퍼 스타일인 후드 재킷과 진 청바지에 회색 운동화를 매칭한 밝은톤의 캐주얼 복장",
                        "하얀 와이셔츠에 분홍 넥타이 포인트를 주고 녹색 정장 바지와 검정 구두로 멋을 낸 댄디한 회사원 복장"
    };
  
    //남성감정이모티콘선택
    public void previousemotonbtn()
    {
        DataController.Instance.itememotionindex--;
        if (DataController.Instance.itememotionindex <= -1)
        {
            DataController.Instance.itememotionindex = 29;
            emotion[0].SetActive(false);
            emotion[DataController.Instance.itememotionindex].SetActive(true);
        }
        else
        {
            emotion[DataController.Instance.itememotionindex + 1].SetActive(false);
            emotion[DataController.Instance.itememotionindex].SetActive(true);
        }


        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            emotionbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 감정 선택 버튼 "+ stemotion[DataController.Instance.itememotionindex];
        }
        Invoke("checkemotioncloseimage", 1f);
    }
    public void nextemotionbtn()
    {
        DataController.Instance.itememotionindex++;
        if(DataController.Instance.itememotionindex >= 30)
        {
            DataController.Instance.itememotionindex = 0;
            emotion[29].SetActive(false);
            emotion[DataController.Instance.itememotionindex].SetActive(true);
        }
        else
        {
            emotion[DataController.Instance.itememotionindex - 1].SetActive(false);
            emotion[DataController.Instance.itememotionindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            emotionbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 감정 선택 버튼 "+ stemotion[DataController.Instance.itememotionindex];
        }
        Invoke("checkemotioncloseimage", 1f);
    }
    public void checkemotioncloseimage()
    {
        emotionbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 감정 선택 버튼";
        emotionbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 감정 선택 버튼";
    }

    //남성얼굴선택
    public void previousfacebtn()
    {
        DataController.Instance.itemfaceindex--;
        if(DataController.Instance.itemfaceindex <= -1)
        {
            DataController.Instance.itemfaceindex = 9;
            face[0].SetActive(false);
            face[DataController.Instance.itemfaceindex].SetActive(true);
        }
        else
        {
            face[DataController.Instance.itemfaceindex + 1].SetActive(false);
            face[DataController.Instance.itemfaceindex].SetActive(true);
        }
        checkhair();
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            facebtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 얼굴 선택 버튼 " + mstface[DataController.Instance.itemfaceindex];
        }
        Invoke("checkfacecloseimage", 1f);
    }
    public void nextfacebtn()
    {
        DataController.Instance.itemfaceindex++;
        if(DataController.Instance.itemfaceindex >=10)
        {
            DataController.Instance.itemfaceindex = 0;
            face[9].SetActive(false);
            face[DataController.Instance.itemfaceindex].SetActive(true);
        }
        else
        {
            face[DataController.Instance.itemfaceindex-1].SetActive(false);
            face[DataController.Instance.itemfaceindex].SetActive(true);
        }
        checkhair();
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            facebtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 얼굴 선택 버튼 " + mstface[DataController.Instance.itemfaceindex];
        }
        Invoke("checkfacecloseimage", 1f);
    }
    public void checkfacecloseimage()
    {
        facebtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 얼굴 선택 버튼";
        facebtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 얼굴 선택 버튼";
    }

    //남성몸통선택
    public void previoustopbtn()
    {
        DataController.Instance.itemtopindex--;
        if (DataController.Instance.itemtopindex <= -1)
        {
            DataController.Instance.itemtopindex = 9;
            top[0].SetActive(false);
            top[DataController.Instance.itemtopindex].SetActive(true);
        }
        else
        {
            top[DataController.Instance.itemtopindex + 1].SetActive(false);
            top[DataController.Instance.itemtopindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            topbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 의상 선택 버튼 " + msttop[DataController.Instance.itemtopindex];
        }
        Invoke("checktopcloseimage", 1f);
    }
    public void nexttopbtn()
    {
        DataController.Instance.itemtopindex++;
        if(DataController.Instance.itemtopindex >= 10)
        {
            DataController.Instance.itemtopindex = 0;
            top[9].SetActive(false);
            top[DataController.Instance.itemtopindex].SetActive(true);
        }
        else
        {
            top[DataController.Instance.itemtopindex - 1].SetActive(false);
            top[DataController.Instance.itemtopindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            topbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 의상 선택 버튼 " + msttop[DataController.Instance.itemtopindex];
        }
        Invoke("checktopcloseimage", 1f);
    }
    public void checktopcloseimage()
    {
        topbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 의상 선택 버튼";
        topbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "남성 의상 선택 버튼";
    }
    #endregion

    #region 여성 아바타 선택.
    public GameObject[] wemotion = new GameObject[30];
    public GameObject[] wface = new GameObject[10];
    public GameObject[] wtop = new GameObject[10];
    public GameObject[] whair = new GameObject[7];

    //감정이모티콘선택.
    public void wpreviousemoitionbtn()
    {
        DataController.Instance.witememotionindex--;
        if (DataController.Instance.witememotionindex <= -1)
        {
            DataController.Instance.witememotionindex = 29;
            wemotion[0].SetActive(false);
            wemotion[DataController.Instance.witememotionindex].SetActive(true);
        }
        else
        {
            wemotion[DataController.Instance.witememotionindex + 1].SetActive(false);
            wemotion[DataController.Instance.witememotionindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            wemotionbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 감정 선택 버튼 " + stemotion[DataController.Instance.witememotionindex];
        }
        Invoke("wcheckemotioncloseimage", 1f);
    }
    public void wnextemotionbtn()
    {
        DataController.Instance.witememotionindex++;
        if (DataController.Instance.witememotionindex >= 30)
        {
            DataController.Instance.witememotionindex = 0;
            wemotion[29].SetActive(false);
            wemotion[DataController.Instance.witememotionindex].SetActive(true);
        }
        else
        {
            wemotion[DataController.Instance.witememotionindex - 1].SetActive(false);
            wemotion[DataController.Instance.witememotionindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            wemotionbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 감정 선택 버튼 " + stemotion[DataController.Instance.witememotionindex];
        }
        Invoke("wcheckemotioncloseimage", 1f);
    }

    public void wcheckemotioncloseimage()
    {
        wemotionbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 감정 선택 버튼";
        wemotionbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 감정 선택 버튼";
    }
    //아바타 얼굴선택.
    public void checkhair()
    {
        whair[0].SetActive(false);
        whair[1].SetActive(false);
        whair[2].SetActive(false);
        whair[3].SetActive(false);
        whair[4].SetActive(false);
        whair[5].SetActive(false);
        whair[6].SetActive(false);
        if (DataController.Instance.witemfaceindex == 0 )
        {
            whair[0].SetActive(true);
        }
        else if(DataController.Instance.witemfaceindex == 2)
        {
            whair[1].SetActive(true);
        }
        else if (DataController.Instance.witemfaceindex == 3)
        {
            whair[2].SetActive(true);
        }
        else if (DataController.Instance.witemfaceindex == 5)
        {
            whair[3].SetActive(true);
        }
        else if (DataController.Instance.witemfaceindex == 6)
        {
            whair[4].SetActive(true);
        }
        else if (DataController.Instance.witemfaceindex == 7)
        {
            whair[5].SetActive(true);
        }
        else if (DataController.Instance.witemfaceindex == 8)
        {
            whair[6].SetActive(true);
        }
       
    }
    public void wnextfacebtn()
    {
        DataController.Instance.witemfaceindex++;
        if(DataController.Instance.witemfaceindex >= 10)
        {
            DataController.Instance.witemfaceindex = 0;
            wface[9].SetActive(false);
            wface[DataController.Instance.witemfaceindex].SetActive(true);
        }
        else
        {
            wface[DataController.Instance.witemfaceindex - 1].SetActive(false);
            wface[DataController.Instance.witemfaceindex].SetActive(true);
        }
        checkhair();
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            wfacebtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 얼굴 선택 버튼 " +wstface[DataController.Instance.witemfaceindex];
        }
        Invoke("wcheckfacecloseimage", 1f);
    }

    public void wpreviousfacebtn()
    {
        DataController.Instance.witemfaceindex--;
        if(DataController.Instance.witemfaceindex <= -1)
        {
            DataController.Instance.witemfaceindex = 9;
            wface[0].SetActive(false);
            wface[DataController.Instance.witemfaceindex].SetActive(true);
        }
        else
        {
            wface[DataController.Instance.witemfaceindex + 1].SetActive(false);
            wface[DataController.Instance.witemfaceindex].SetActive(true);
        }
        checkhair();
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            wfacebtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 얼굴 선택 버튼 "+ wstface[DataController.Instance.witemfaceindex];
        }
        Invoke("wcheckfacecloseimage", 1f);
    }
    public void wcheckfacecloseimage()
    {
        wfacebtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 얼굴 선택 버튼";
        wfacebtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 얼굴 선택 버튼";
    }
    //아바타 몸통선택
    public void wnexttopbtn()
    {
        DataController.Instance.witemtopindex++;
        if (DataController.Instance.witemtopindex >= 10)
        {
            DataController.Instance.witemtopindex = 0;
            wtop[9].SetActive(false);
            wtop[DataController.Instance.witemtopindex].SetActive(true);
        }
        else
        {
            wtop[DataController.Instance.witemtopindex - 1].SetActive(false);
            wtop[DataController.Instance.witemtopindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            wtopbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 의상 선택 버튼 "+ wsttop[DataController.Instance.witemtopindex];
        }
        Invoke("wchecktopcloseimage", 1f);
    }
    
    public void wprevioustopbtn()
    {
        DataController.Instance.witemtopindex--;
        if (DataController.Instance.witemtopindex <= -1)
        {
            DataController.Instance.witemtopindex = 9;
            wtop[0].SetActive(false);
            wtop[DataController.Instance.witemtopindex].SetActive(true);
        }
        else
        {
            wtop[DataController.Instance.witemtopindex + 1].SetActive(false);
            wtop[DataController.Instance.witemtopindex].SetActive(true);
        }
        if (AccessibilitySettings.IsVoiceOverRunning == true) //보이스오버
        {
            wtopbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 의상 선택 버튼 "+ wsttop[DataController.Instance.witemtopindex];
        }
        Invoke("wchecktopcloseimage", 1f);
    }
    public void wchecktopcloseimage()
    {
        wtopbtn[0].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 의상 선택 버튼";
        wtopbtn[1].GetComponent<AccessibilityNode>().AccessibilityLabel = "여성 의상 선택 버튼";
    }
    #endregion

    #region 여자 아바타선택 TTS
    string[] wstface = { "갈색 긴 생머리와 가르마 앞머리에 짙은 갈색 눈동자가 어울리는 얼굴",
                           "진 회색 양갈래 머리를 길게 딴 애교 머리에 짙은 파란색 눈이 어울리는 얼굴",
                           "붉은색 머리를 뒤로 따올려 묶고 앞 머리로 귀여운 스타일을 연출한 초록눈의 얼굴",
                           "짙은 갈색의 가르마 앞 머리에 아래로 양쪽으로 둥글게 묶은 연갈색 눈이 어울리는 얼굴",
                           "밝은 갈색의 앞 머리와 양갈래로 묶은 머리에 검정 눈이 어울리는 얼굴",
                           "앞 머리가 있는 회색 긴 생머리에 파란 브릿지가 섞인 파란 눈의 이색적인 얼굴",
                           "검정 앞 머리와 웨이브가 있는 긴 머리에 검정 눈이 잘 어울리는 얼굴",
                           "연갈색 가르마 앞 머리와 짧은 단발 펌에 녹색 눈이 잘 어울리는 얼굴",
                           "검은색 앞머리와 곱슬 펌 머리에 갈색 눈이 잘 어울리는 얼굴",
                           "연갈색 가르마 앞머리와 양갈래로 길게 웨이브가 있는 묶음 머리에 녹색 눈이 잘 어울리는 얼굴"
    };
    string[] wsttop = { "스쿨룩 스타일의 연한 갈색 재킷과 하얀 브라우스 그리고 짙은 갈색 짧은 치마로 매칭한 복장",
                         "파란색 가디건에 하얀 셔츠 그리고 검정 넥타이와 연 회색의 면 바지를 매칭한 깔끔한 복장",
                         "톰보이 스타일의 검은색 짧은 가죽 재킷과 하얀 브라우스 그리고 짙은 회색 짧은 스커트와 긴 검정 가죽부츠를 신은 스타일 리쉬한 복장",
                         "짙은 갈색 재킷에 연한 베이지 면티 그리고 갈색 체크 무늬 스커트와 갈색 가죽부츠를 신은 세련된 복장",
                         "밝은 녹색 재킷 안에 하얀 면티와 오렌지색 베스트를 입고 청 회색 진을 입은 밝은 분위기의 캐주얼한 복장",
                         "검정 면 티위에 트레이닝 복 스타일의 재킷과 검정 운동복 하의와 흰 운동화를 신은 활동적인 복장",
                         "하얀색 모자가 달린 보라색 후드티와 보라색 카고 면 바지를 입은 귀여운 스타일의 복장",
                         "하얀 브라우스에 분홍색 조끼와 얇은 검정 넥타이 그리고 분홍색 짧은 원피스를 입고 검정 밸트와 검정 부츠로 포인트를 준 복장",
                         "분홍색 후드티에 긴 청 치마를 입은 일상적인 복장",
                         "청 재킷에 베이지 니트 브라우스 짧은 청 치마를 입은 발랄한 복장"
    };

    #endregion

}
