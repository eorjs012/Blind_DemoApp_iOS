using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Apple.Accessibility;

public class Loading : MonoBehaviour
{
    private static Loading instance;

    public static Loading Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<Loading>();
            if (instance != null) return instance;
            var container = new GameObject("Loading");
            instance = container.AddComponent<Loading>();
            return instance;
        }
    }
    public Text text_Loading;
    public Image image_fill;
    public Slider slider;
    private float time_loading = 5;
    private float checktime_loading = 10;
    private float time_current;
    private float time_start;
    private bool isEnded = true;
    public GameObject roadingpanel;
    private bool sceneLoaded = false;

    public GameObject blindloadingTt;

    void Start()
    {
       
        if (SmartSecurity.GetInt("ttscontrolindex") == 1)
        {
            if (SmartSecurity.GetInt("photonroomindex") == 1)
            {
                // Invoke("blindonefloortts", 1f);
                blindloadingTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "월드 일층 로딩중 입니다.잠시만 기다려 주세요";
            }
            else if (SmartSecurity.GetInt("photonroomindex") == 2)
            {
                //Invoke("blindtwofloortts", 1f);
                blindloadingTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "월드 이층 로딩중 입니다.잠시만 기다려 주세요";
            }
        }
        slider.value = 0;
        Reset_Loading();
        SmartSecurity.SetInt("checkloading", 0);
        
    }

    public void blindonefloortts()
    {
        testtts.Instance.audioSource.Stop();
        testtts.Instance.alltts("월드 1층 로딩중 입니다.잠시만 기다려 주세요");
        CancelInvoke("blindonefloortts");
    }

    public void blindtwofloortts()
    {
        testtts.Instance.audioSource.Stop();
        testtts.Instance.alltts("월드 2층 로딩중 입니다.잠시만 기다려 주세요");
        CancelInvoke("blindtwofloortts");
    }


    void Update()
    {
        if (isEnded)
        {
            if(roadingpanel.activeSelf && !sceneLoaded)
            {
                if (Time.time > checktime_loading)
                {
                     LoadNextScene();
                }
            }
            else
            {
                return;
            }
        }
        else 
        {
            Check_Loading();
        }
    }

    private void Check_Loading()
    {
        time_current = Time.time - time_start;
        if (time_current < time_loading)
        {
            Set_FillAmount(time_current / time_loading);
        }
        else if (!isEnded)
        {
            End_Loading();
        }
    }
    private void End_Loading() //로딩이 끝났을떄.
    {
        Set_FillAmount(1);
        isEnded = true;
       
        SmartSecurity.SetInt("ttsindex1", 0);
        SmartSecurity.SetInt("ttsindex3", 0);
        SmartSecurity.SetInt("ttsindex5", 0);
        SmartSecurity.SetInt("ttsindex7", 0);
        TextController.Instance.topsetting.SetActive(true);
        if (SmartSecurity.GetInt("loadingindex") == 1)
        {
            roadingpanel.SetActive(false);
            SmartSecurity.SetInt("checkloading", 1);
            SmartSecurity.SetInt("checkuserevent", 0);
            SmartSecurity.SetInt("checkgworldtts", 0);
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                if (SmartSecurity.GetInt("photonroomindex") == 1)
                {
                     Invoke("startblindon", 1f);
                    string st = "국립고궁박물관 메타월드 1층 안으로 들어오셨습니다.국립고궁박물관 내에서 이동은 태블릿을 전후좌우로 기울이면 태블릿이 기울어진 방향으로 아바타가 이동합니다.이동시 방향 안내음과 발걸음 소리가 나옵니다.이동중 주변에 타 아바타 또는 전시물이 있는 경우 알림음이 나오며 이때 손에 들고 있는 태블릿을 두손으로 흔들면 이벤트 페이지로 이동합니다.전시월드 안에서의 내위치정보와 전시물의 이미지 정보를닷패드와 연동하여 촉감 디스플레이로 만져 볼 수 있습니다.내위치정보는 태블릿 화면 왼쪽 상단 영역을 손가락으로 터치하면 됩니다.전시월드를 나가시려면 태블릿 화면 오른쪽 상단 영역 월드 나가기 버튼을 터치하시면 됩니다.";
                    
                    TextController.Instance.blindexitbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
                    TextController.Instance.blindcharposbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
                    TextController.Instance.blindworldnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
                    blindloadingTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "";
                    
                    Invoke("loadingon", 2f);
                }
                else if (SmartSecurity.GetInt("photonroomindex") == 2) 
                {
                    Invoke("startblindon", 1f);
                    string st = "국립고궁박물관 메타월드 2층 안으로 들어오셨습니다.";
                    TextController.Instance.blindexitbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
                    TextController.Instance.blindcharposbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
                    TextController.Instance.blindworldnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
                    blindloadingTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "";

                    Invoke("loadingon", 2f);
                }
            }
        }
       
    }

    public void loadingon()
    {
        AccessibilityNotification.PostLayoutChangedNotification();
        TextController.Instance.blindexitbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "월드 나가기 버튼";
        TextController.Instance.blindcharposbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "내위치 버튼";
        TextController.Instance.blindworldnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "국립 고궁박물관";
        CancelInvoke("loadingon");
    }
    public void startblindon()
    {
        SmartSecurity.SetInt("startblindindex", 1);
        CancelInvoke("startblindon");
    }
    private void Reset_Loading() //로딩리셋.
    {
      
        time_current = time_loading;
        time_start = Time.time;
        Set_FillAmount(0);
        isEnded = false;
    }
    private void Set_FillAmount(float _value) //로딩퍼센트표시
    {
        image_fill.fillAmount = _value;
        slider.value = _value;
        string txt = (_value.Equals(1) ? "Finished.. " : "Loading.. ") + (_value).ToString("P0"); 
        text_Loading.text = txt;
    }
    private void LoadNextScene()
    {
        SmartSecurity.SetInt("checkloading", 1);
        roadingpanel.SetActive(false);
        SceneManager.LoadScene(1); // Replace "YourSceneName" with the actual name of your scene
        sceneLoaded = true;
    }
}
