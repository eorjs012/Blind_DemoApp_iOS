using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using BackEnd;


public class DataController : MonoBehaviour
{
    private static DataController instance;

    public static DataController Instance 
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<DataController>();
            if (instance != null) return instance;
            var container = new GameObject("DataController");
            return instance;
        }
    }

    void Start()
    {
        playerpositionindex = 0;
        one = 0;
        playerindex = 0;
        btnindex = 0;
        positionindex = 0;
        loginindex = 0;
        doubletouch = 0;
        touchjoy = 0;
        doubletapindex = 0;
        eventtouchindex = 0;
        SmartSecurity.SetInt("checkuserevent", 0);
        SmartSecurity.SetInt("bleindex", 0);
        SmartSecurity.SetInt("bleconnectindex", 0);
        SmartSecurity.SetInt("checkbleobjindex0", 0);
        SmartSecurity.SetInt("checkbleobjindex1", 0);
        SmartSecurity.SetInt("checkbleobjindex2", 0);
        SmartSecurity.SetString("checkbleobjname", "");
        SmartSecurity.SetString("checkbleobjname0", "");
        SmartSecurity.SetString("checkbleobjname1", "");
        SmartSecurity.SetString("checkbleobjname2", "");
        SmartSecurity.SetInt("dotimageindex", 0);
        SmartSecurity.SetInt("startblindindex", 0);
        SmartSecurity.SetInt("checkhomeindex", 0);
        SmartSecurity.SetInt("checkuesrevent", 0);
        SmartSecurity.SetInt("checkgworldtts", 0);
        SmartSecurity.SetInt("checkroadindex", 0);
        SmartSecurity.SetInt("playerareaindex", 2);
        SmartSecurity.SetInt("checksettingindex", 0);
        SmartSecurity.SetInt("ttscontrolindex", 1);
        //PlayerPrefs.DeleteAll(); //로컬 데이터 초기화
    }
     private void Awake()
     {
         var obj = FindObjectsOfType<DataController>();
         if (obj.Length == 1)
         { DontDestroyOnLoad(gameObject); }
         else
         { Destroy(gameObject); }
     }


    public int checksettingindex // 처음시작 후 세팅화면 키고 난후 호출 안됨 백버튼 누를시 인덱스 초기화 및 호출가능  
    {
        get { return SmartSecurity.GetInt("checksettingindex", 0); }
        set { SmartSecurity.SetInt("checksettingindex", value); }
    }

    public int checkroadindex //로그인 이후 로드씬으로 이동시 한번만 호출
    {
        get { return SmartSecurity.GetInt("checkroadindex", 0); }
        set { SmartSecurity.SetInt("checkroadindex", value); }
    }


    public int checkhomeindex
    {
        get { return SmartSecurity.GetInt("checkhomeindex", 0); }
        set { SmartSecurity.SetInt("checkhomeindex", value); }
    }

    public int startblindindex
    {
        get { return SmartSecurity.GetInt("startblindindex", 0); }
        set { SmartSecurity.SetInt("startblindindex", value); }
    }

    public int dotmcharimageindex  
    {
        get { return SmartSecurity.GetInt("dotmcharimageindex", 0); }
        set { SmartSecurity.SetInt("dotmcharimageindex", value); }
    }

    public int dotwcharimageindex
    {
        get { return SmartSecurity.GetInt("dotwcharimageindex", 0); }
        set { SmartSecurity.SetInt("dotwcharimageindex", value); }
    }

    public int getdotmcharimageindex
    {
        get { return SmartSecurity.GetInt("getdotmcharimageindex", 0); }
        set { SmartSecurity.SetInt("getdotmcharimageindex", value); }
    }

    public int getdotwcharimageindex
    {
        get { return SmartSecurity.GetInt("getdotwcharimageindex", 0); }
        set { SmartSecurity.SetInt("getdotwcharimageindex", value); }
    }

    public int dotimageindex  //설정메뉴 닷패드 촉각이미지
    {
        get { return SmartSecurity.GetInt("dotimageindex", 0); }
        set { SmartSecurity.SetInt("dotimageindex", value); }
    }

    //블루투스패널 활성화 체크 인덱스
    public int bleindex
    {
        get { return SmartSecurity.GetInt("bleindex", 0); }
        set { SmartSecurity.SetInt("bleindex", value); }
    }

    public int checkbleobjindex0
    {
        get { return SmartSecurity.GetInt("checkbleobjindex0", 0); }
        set { SmartSecurity.SetInt("checkbleobjindex0", value); }
    }
    public int checkbleobjindex1
    {
        get { return SmartSecurity.GetInt("checkbleobjindex1", 0); }
        set { SmartSecurity.SetInt("checkbleobjindex1", value); }
    }
    public int checkbleobjindex2
    {
        get { return SmartSecurity.GetInt("checkbleobjindex2", 0); }
        set { SmartSecurity.SetInt("checkbleobjindex2", value); }
    }

    public string checkbleobjname
    {
        get { return SmartSecurity.GetString("checkbleobjname", ""); }
        set { SmartSecurity.SetString("checkbleobjname", value.ToString()); }
    }

    public string checkbleobjname0
    {
        get { return SmartSecurity.GetString("checkbleobjname0", ""); }
        set { SmartSecurity.SetString("checkbleobjname0", value.ToString()); }
    }

    public string checkbleobjname1
    {
        get { return SmartSecurity.GetString("checkbleobjname1", ""); }
        set { SmartSecurity.SetString("checkbleobjname1", value.ToString()); }
    }

    public string checkbleobjname2
    {
        get { return SmartSecurity.GetString("checkbleobjname2", ""); }
        set { SmartSecurity.SetString("checkbleobjname2", value.ToString()); }
    }

    public int bleconnectindex
    {
        get { return SmartSecurity.GetInt("bleconnectindex", 0); }
        set { SmartSecurity.SetInt("bleconnectindex", value); }
    }

    

    public int boxindex
    {
        get { return SmartSecurity.GetInt("boxindex", 0); }
        set { SmartSecurity.SetInt("boxindex", value); }
    }
    public string nickname 
    {
        get { return SmartSecurity.GetString("nickname", "0"); }
        set { SmartSecurity.SetString("nickname", value.ToString()); }
    }
    public string insideid
    {
        get { return SmartSecurity.GetString("insideid", "0"); }
        set { SmartSecurity.SetString("insideid", value.ToString()); }
    }
    public string myprivateindata
    {
        get { return SmartSecurity.GetString("myprivateindata", "0"); }
        set { SmartSecurity.SetString("myprivateindata", value.ToString()); }
    }
   
    // 아바타 데이터
    public int wmindex
    {
        get { return SmartSecurity.GetInt("wmindex", 0); }
        set { SmartSecurity.SetInt("wmindex", value); }
    }
    public int wmsureindex
    {
        get { return SmartSecurity.GetInt("wmsureindex", 0); }
        set { SmartSecurity.SetInt("wmsureindex", value); }
    }
    public int getwmsureindex
    {
        get { return SmartSecurity.GetInt("getwmsureindex", 0); }
        set { SmartSecurity.SetInt("getwmsureindex", value); }
    }

    //여성캐릭터선택.
 
    public int witememotion    //데이터 저장
    {
        get { return SmartSecurity.GetInt("witememotion", 0); }
        set { SmartSecurity.SetInt("witememotion", value); }
    }
    public int witemtop     //데이터 저장
    {
        get { return SmartSecurity.GetInt("witemtop", 0); }
        set { SmartSecurity.SetInt("witemtop", value); }
    }
    public int witemface   //데이터 저장
    {
        get { return SmartSecurity.GetInt("witemface", 0); }
        set { SmartSecurity.SetInt("witemface", value); }
    }
    public int witemhair
    {
        get { return SmartSecurity.GetInt("witemhair", 0); }
        set { SmartSecurity.SetInt("witemhair", value); }
    }


    public int getwitememotion //데이터 받아오기
    {
        get { return SmartSecurity.GetInt("getwitememotion", 0); }
        set { SmartSecurity.SetInt("getwitememotion", value); }
    }
    public int getwitemtop  //데이터 받아오기
    {
        get { return SmartSecurity.GetInt("getwitemtop", 0); }
        set { SmartSecurity.SetInt("getwitemtop", value); }
    }
    public int getwitemface  //데이터 받아오기
    {
        get { return SmartSecurity.GetInt("getwitemface", 0); }
        set { SmartSecurity.SetInt("getwitemface", value); }
    }


    public int witememotionindex //여성아바타 감정선택
    {
        get { return SmartSecurity.GetInt("witememotionindex", 0); }
        set { SmartSecurity.SetInt("witememotionindex", value); }
    }
    public int witemtopindex //여성아바타 의상선택
    {
        get { return SmartSecurity.GetInt("witemtopindex", 0); }
        set { SmartSecurity.SetInt("witemtopindex", value); }
    }
    public int witemfaceindex //여성아바타 얼굴선택
    {
        get { return SmartSecurity.GetInt("witemfaceindex", 0); }
        set { SmartSecurity.SetInt("witemfaceindex", value); }
    }

    public string stringttstest
    {
        get { return SmartSecurity.GetString("stringttstest", "0"); }
        set { SmartSecurity.SetString("stringttstest", value.ToString()); }
    }
    //남성 캐릭터선택.

    public int itememotion    //데이터 저장
    {
        get { return SmartSecurity.GetInt("itememotion", 0); }
        set { SmartSecurity.SetInt("itememotion", value); }
    }
    public int itemface   //데이터 저장
    {
        get { return SmartSecurity.GetInt("itemface", 0); }
        set { SmartSecurity.SetInt("itemface", value); }
    }
    public int itemtop   //데이터 저장
    {
        get { return SmartSecurity.GetInt("itemtop", 0); }
        set { SmartSecurity.SetInt("itemtop", value); }
    }


    public int getitememotion  //데이터 받아오기
    {
        get { return SmartSecurity.GetInt("getitememotion", 0); }
        set { SmartSecurity.SetInt("getitememotion", value); }
    }
    public int getitemface  //데이터 받아오기
    {
        get { return SmartSecurity.GetInt("getitemface", 0); }
        set { SmartSecurity.SetInt("getitemface", value); }
    }
    public int getitemtop   //데이터 받아오기
    {
        get { return SmartSecurity.GetInt("getitemtop", 0); }
        set { SmartSecurity.SetInt("getitemtop", value); }
    }



    public int itememotionindex    //아바타선택
    {
        get { return SmartSecurity.GetInt("itememotionindex", 0); }
        set { SmartSecurity.SetInt("itememotionindex", value); }
    }
    public int itemfaceindex   //아바타선택
    {
        get { return SmartSecurity.GetInt("itemfaceindex", 0); }
        set { SmartSecurity.SetInt("itemfaceindex", value); }
    }
    public int itemtopindex   //아바타선택
    {
        get { return SmartSecurity.GetInt("itemtopindex", 0); }
        set { SmartSecurity.SetInt("itemtopindex", value); }
    }

    public string iteminfoTt  //아이템텍스트.
    {
        get { return SmartSecurity.GetString("iteminfo", "0"); }
        set { SmartSecurity.SetString("iteminfo", value.ToString()); }
    }
    public string getiteminfoTt  //아이템텍스트 데이터 받아오기.
    {
        get { return SmartSecurity.GetString("getiteminfoTt", "0"); }
        set { SmartSecurity.SetString("getiteminfoTt", value.ToString()); }
    }
    

   
    public string mypublicitemdata //유저 아이템테이블데이터
    {
        get { return SmartSecurity.GetString("mypublicitemdata", "0"); }
        set { SmartSecurity.SetString("mypublicitemdata", value.ToString()); }
    }

    public string loginid
    {

        get { return SmartSecurity.GetString("loginid", "0"); }
        set { SmartSecurity.SetString("loginid", value.ToString()); }
    }
    public string loginpassword
    {

        get { return SmartSecurity.GetString("loginpassword", "0"); }
        set { SmartSecurity.SetString("loginpassword", value.ToString()); }
    }
    public string loginnickname
    {

        get { return SmartSecurity.GetString("loginnickname", "0"); }
        set { SmartSecurity.SetString("loginnickname", value.ToString()); }
    }
    public string mypubliclogindata //로그인테이블데이터
    {

        get { return SmartSecurity.GetString("mypubliclogindata", "0"); }
        set { SmartSecurity.SetString("mypubliclogindata", value.ToString()); }
    }


    //조이스틱관련데이터 및 플레이어데이터.
    public int mathfjoyveindex
    {
        get { return SmartSecurity.GetInt("mathfjoyveindex", 0); }
        set { SmartSecurity.SetInt("mathfjoyveindex", value); }
    }
    public int mathfjoyvecyindex
    {
        get { return SmartSecurity.GetInt("mathfjoyvecyindex", 0); }
        set { SmartSecurity.SetInt("mathfjoyvecyindex", value); }
    }
    public int mathfjoyvecxindex
    {
        get { return SmartSecurity.GetInt("mathfjoyvecxindex", 0); }
        set { SmartSecurity.SetInt("mathfjoyvecxindex", value); }
    }
    public int doubletapindex
    {
        get { return SmartSecurity.GetInt("doubletapindex", 0); }
        set { SmartSecurity.SetInt("doubletapindex", value); }
    }
    public int colnextindex
    {
        get { return SmartSecurity.GetInt("colnextindex", 0); }
        set { SmartSecurity.SetInt("colnextindex", value); }
    }
    public int checkcolindex
    {
        get { return SmartSecurity.GetInt("checkcolindex", 0); }
        set { SmartSecurity.SetInt("checkcolindex", value); }
    }
  
    public int touchjoy
    {
        get { return SmartSecurity.GetInt("touchjoy", 0); }
        set { SmartSecurity.SetInt("touchjoy", value); }
    }
    public int joyposition
    {
        get { return SmartSecurity.GetInt("joyposition", 0); }
        set { SmartSecurity.SetInt("joyposition", value); }
    }
    public int eventtouchindex  //오브젝트감지체크
    {
        get { return SmartSecurity.GetInt("eventtouchindex", 0); }
        set { SmartSecurity.SetInt("eventtouchindex", value); }
    }
    public int doubletouch
    {
        get { return SmartSecurity.GetInt("doubletouch", 0); }
        set { SmartSecurity.SetInt("doubletouch", value); }
    }
    public float playerdist
    {
        get { return SmartSecurity.GetFloat("playerdist", 0); }
        set { SmartSecurity.SetFloat("playerdist", value); }
    }
    public int checkplayerareaindex
    {
        get { return SmartSecurity.GetInt("checkplayerareaindex", 0); }
        set { SmartSecurity.SetInt("checkplayerareaindex", value); }
    }
    public int playerareaindex
    {
        get { return SmartSecurity.GetInt("playerareaindex", 2); }
        set { SmartSecurity.SetInt("playerareaindex", value); }
    }

    //로그인 및 회원가입 데이터.
    public int signupagreeindex
    {
        get { return SmartSecurity.GetInt("signupagreeindex", 0); }
        set { SmartSecurity.SetInt("signupagreeindex", value); }
    }
    public int signupindex
    {
        get { return SmartSecurity.GetInt("signupindex", 0); }
        set { SmartSecurity.SetInt("signupindex", value); }
    }
    public int signupidcheckindex
    {
        get { return SmartSecurity.GetInt("signupidcheckindex", 0); }
        set { SmartSecurity.SetInt("signupidcheckindex", value); }
    }
    public int signuppasswordcheckindex
    {
        get { return SmartSecurity.GetInt("signuppasswordcheckindex", 0); }
        set { SmartSecurity.SetInt("signuppasswordcheckindex", value); }
    }
    public int checknicknameindex
    {
        get { return SmartSecurity.GetInt("checknicknameindex", 0); }
        set { SmartSecurity.SetInt("checknicknameindex", value); }
    }
    public int signupidbadwordindex
    {
        get { return SmartSecurity.GetInt("signupidbadwordindex", 0); }
        set { SmartSecurity.SetInt("signupidbadwordindex", value); }
    }
  
    public int changenicknameindex
    {
        get { return SmartSecurity.GetInt("changenicknameindex", 0); }
        set { SmartSecurity.SetInt("changenicknameindex", value); }
    }
    public int changepasswordindex
    {
        get { return SmartSecurity.GetInt("changepasswordindex", 0); }
        set { SmartSecurity.SetInt("changepasswordindex", value); }
    }


    //오브젝트 UI패널 데이터.
    public int listobjindex //오브젝트순서
    {
        get { return SmartSecurity.GetInt("listobjindex", 0); }
        set { SmartSecurity.SetInt("listobjindex", value); }
    }
    public int listobjindex0
    {
        get { return SmartSecurity.GetInt("listobjindex0", 0); }
        set { SmartSecurity.SetInt("listobjindex0", value); }
    }
    public int listobjindex1
    {
        get { return SmartSecurity.GetInt("listobjindex1", 0); }
        set { SmartSecurity.SetInt("listobjindex1", value); }
    }
    public int listobjindex2
    {
        get { return SmartSecurity.GetInt("listobjindex2", 0); }
        set { SmartSecurity.SetInt("listobjindex2", value); }
    }
    public int listobjindex3
    {
        get { return SmartSecurity.GetInt("listobjindex3", 0); }
        set { SmartSecurity.SetInt("listobjindex3", value); }
    }
    public int listobjindex4
    {
        get { return SmartSecurity.GetInt("listobjindex4", 0); }
        set { SmartSecurity.SetInt("listobjindex4", value); }
    }
    public int listobjindex5
    {
        get { return SmartSecurity.GetInt("listobjindex5", 0); }
        set { SmartSecurity.SetInt("listobjindex5", value); }
    }
    public int listobjindex6
    {
        get { return SmartSecurity.GetInt("listobjindex6", 0); }
        set { SmartSecurity.SetInt("listobjindex6", value); }
    }
    public int listobjindex7
    {
        get { return SmartSecurity.GetInt("listobjindex7", 0); }
        set { SmartSecurity.SetInt("listobjindex7", value); }
    }
    public int listobjindex8
    {
        get { return SmartSecurity.GetInt("listobjindex8", 0); }
        set { SmartSecurity.SetInt("listobjindex8", value); }
    }
    public int listobjindex9
    {
        get { return SmartSecurity.GetInt("listobjindex9", 0); }
        set { SmartSecurity.SetInt("listobjindex9", value); }
    }
    public int listobjindex10
    {
        get { return SmartSecurity.GetInt("listobjindex10", 0); }
        set { SmartSecurity.SetInt("listobjindex10", value); }
    }
    public int listobjindex11
    {
        get { return SmartSecurity.GetInt("listobjindex11", 0); }
        set { SmartSecurity.SetInt("listobjindex11", value); }
    }

    //상호작용 캐릭터 이름저장
    public string listplayerindex
    {
        get { return SmartSecurity.GetString("listplayerindex", "0"); }
        set { SmartSecurity.SetString("listplayerindex", value.ToString()); }
    }
    public string listplayerindex0  
    {
        get { return SmartSecurity.GetString("listplayerindex0", "0"); }
        set { SmartSecurity.SetString("listplayerindex0", value.ToString()); }
    }
    public string listplayerindex1
    {
        get { return SmartSecurity.GetString("listplayerindex1", "0"); }
        set { SmartSecurity.SetString("listplayerindex1", value.ToString()); }
    }
    public string listplayerindex2
    {
        get { return SmartSecurity.GetString("listplayerindex2", "0"); }
        set { SmartSecurity.SetString("listplayerindex2", value.ToString()); }
    }
    public string listplayerindex3
    {
        get { return SmartSecurity.GetString("listplayerindex3", "0"); }
        set { SmartSecurity.SetString("listplayerindex3", value.ToString()); }
    }
    public string listplayerindex4
    {
        get { return SmartSecurity.GetString("listplayerindex4", "0"); }
        set { SmartSecurity.SetString("listplayerindex4", value.ToString()); }
    }
    public string listplayerindex5
    {
        get { return SmartSecurity.GetString("listplayerindex5", "0"); }
        set { SmartSecurity.SetString("listplayerindex5", value.ToString()); }
    }
    public string listplayerindex6
    {
        get { return SmartSecurity.GetString("listplayerindex6", "0"); }
        set { SmartSecurity.SetString("listplayerindex6", value.ToString()); }
    }
    public string listplayerindex7
    {
        get { return SmartSecurity.GetString("listplayerindex7", "0"); }
        set { SmartSecurity.SetString("listplayerindex7", value.ToString()); }
    }
    public string listplayerindex8
    {
        get { return SmartSecurity.GetString("listplayerindex8", "0"); }
        set { SmartSecurity.SetString("listplayerindex8", value.ToString()); }
    }
    public string listplayerindex9
    {
        get { return SmartSecurity.GetString("listplayerindex9", "0"); }
        set { SmartSecurity.SetString("listplayerindex9", value.ToString()); }
    }
    public string listplayerindex10
    {
        get { return SmartSecurity.GetString("listplayerindex10", "0"); }
        set { SmartSecurity.SetString("listplayerindex10", value.ToString()); }
    }
    public string listplayerindex11
    {
        get { return SmartSecurity.GetString("listplayerindex11", "0"); }
        set { SmartSecurity.SetString("listplayerindex11", value.ToString()); }
    }



    public int listcharindex //캐릭터남성여성 이미지.
    {
        get { return SmartSecurity.GetInt("listcharindex", 0); }
        set { SmartSecurity.SetInt("listcharindex", value); }
    }
    public int checklisten
    {
        get { return SmartSecurity.GetInt("checklisten", 0); }
        set { SmartSecurity.SetInt("checklisten", value); }
    }
    //오브젝트 , 캐릭터 구분인덱스
    public int checklisten0
    {
        get { return SmartSecurity.GetInt("checklisten0", 0); }
        set { SmartSecurity.SetInt("checklisten0", value); }
    }
    public int checklisten1
    {
        get { return SmartSecurity.GetInt("checklisten1", 0); }
        set { SmartSecurity.SetInt("checklisten1", value); }
    }
    public int checklisten2
    {
        get { return SmartSecurity.GetInt("checklisten2", 0); }
        set { SmartSecurity.SetInt("checklisten2", value); }
    }
    public int checklisten3
    {
        get { return SmartSecurity.GetInt("checklisten3", 0); }
        set { SmartSecurity.SetInt("checklisten3", value); }
    }
    public int checklisten4
    {
        get { return SmartSecurity.GetInt("checklisten4", 0); }
        set { SmartSecurity.SetInt("checklisten4", value); }
    }
    public int checklisten5
    {
        get { return SmartSecurity.GetInt("checklisten5", 0); }
        set { SmartSecurity.SetInt("checklisten5", value); }
    }
    public int checklisten6
    {

        get { return SmartSecurity.GetInt("checklisten6", 0); }
        set { SmartSecurity.SetInt("checklisten6", value); }
    }
    public int checklisten7
    {
        get { return SmartSecurity.GetInt("checklisten7", 0); }
        set { SmartSecurity.SetInt("checklisten7", value); }
    }
    public int checklisten8
    {
        get { return SmartSecurity.GetInt("checklisten8", 0); }
        set { SmartSecurity.SetInt("checklisten8", value); }
    }
    public int checklisten9
    {
        get { return SmartSecurity.GetInt("checklisten9", 0); }
        set { SmartSecurity.SetInt("checklisten9", value); }
    }
    public int checklisten10
    {
        get { return SmartSecurity.GetInt("checklisten10", 0); }
        set { SmartSecurity.SetInt("checklisten10", value); }
    }
    public int checklisten11
    {
        get { return SmartSecurity.GetInt("checklisten11", 0); }
        set { SmartSecurity.SetInt("checklisten11", value); }
    }

    //상호감지된 오브젝트 이름저장인덱스
    public string listname0
    {
        get { return SmartSecurity.GetString("listname0", "0"); }
        set { SmartSecurity.SetString("listname0", value.ToString()); }
    }
    public string listname1
    {
        get { return SmartSecurity.GetString("listname1", "0"); }
        set { SmartSecurity.SetString("listname1", value.ToString()); }
    }
    public string listname2
    {
        get { return SmartSecurity.GetString("listname2", "0"); }
        set { SmartSecurity.SetString("listname2", value.ToString()); }
    }
    public string listname3
    {
        get { return SmartSecurity.GetString("listname3", "0"); }
        set { SmartSecurity.SetString("listname3", value.ToString()); }
    }
    public string listname4
    {
        get { return SmartSecurity.GetString("listname4", "0"); }
        set { SmartSecurity.SetString("listname4", value.ToString()); }
    }
    public string listname5
    {
        get { return SmartSecurity.GetString("listname5", "0"); }
        set { SmartSecurity.SetString("listname5", value.ToString()); }
    }
    public string listname6
    {
        get { return SmartSecurity.GetString("listname6", "0"); }
        set { SmartSecurity.SetString("listname6", value.ToString()); }
    }
    public string listname7
    {
        get { return SmartSecurity.GetString("listname7", "0"); }
        set { SmartSecurity.SetString("listname7", value.ToString()); }
    }
    public string listname8
    {
        get { return SmartSecurity.GetString("listname8", "0"); }
        set { SmartSecurity.SetString("listname8", value.ToString()); }
    }
    public string listname9
    {
        get { return SmartSecurity.GetString("listname9", "0"); }
        set { SmartSecurity.SetString("listname9", value.ToString()); }
    }
    public string listname10
    {
        get { return SmartSecurity.GetString("listname10", "0"); }
        set { SmartSecurity.SetString("listname10", value.ToString()); }
    }
    public string listname11
    {
        get { return SmartSecurity.GetString("listname11", "0"); }
        set { SmartSecurity.SetString("listname11", value.ToString()); }
    }

    public int settingmenuindex //세팅패널 인덱스
    {
        get { return SmartSecurity.GetInt("settingmenuindex", 0); }
        set { SmartSecurity.SetInt("settingmenuindex", value); }
    }

    //더블탭 유저네임 가져오기 최대 12개.

    public string username
    {
        get { return SmartSecurity.GetString("username", "0"); }
        set { SmartSecurity.SetString("username", value.ToString()); }
    }

    public string username0
    {
        get { return SmartSecurity.GetString("username0", "0"); }
        set { SmartSecurity.SetString("username0", value.ToString()); }
    }
    public string username1
    {
        get { return SmartSecurity.GetString("username1", "0"); }
        set { SmartSecurity.SetString("username1", value.ToString()); }
    }
    public string username2
    {
        get { return SmartSecurity.GetString("username2", "0"); }
        set { SmartSecurity.SetString("username2", value.ToString()); }
    }
    public string username3
    {
        get { return SmartSecurity.GetString("username3", "0"); }
        set { SmartSecurity.SetString("username3", value.ToString()); }
    }
    public string username4
    {
        get { return SmartSecurity.GetString("username4", "0"); }
        set { SmartSecurity.SetString("username4", value.ToString()); }
    }
    public string username5
    {
        get { return SmartSecurity.GetString("username5", "0"); }
        set { SmartSecurity.SetString("username5", value.ToString()); }
    }
    public string username6
    {
        get { return SmartSecurity.GetString("username6", "0"); }
        set { SmartSecurity.SetString("username6", value.ToString()); }
    }
    public string username7
    {
        get { return SmartSecurity.GetString("username7", "0"); }
        set { SmartSecurity.SetString("username7", value.ToString()); }
    }
    public string username8
    {
        get { return SmartSecurity.GetString("username8", "0"); }
        set { SmartSecurity.SetString("username8", value.ToString()); }
    }
    public string username9
    {
        get { return SmartSecurity.GetString("username9", "0"); }
        set { SmartSecurity.SetString("username9", value.ToString()); }
    }
    public string username10
    {
        get { return SmartSecurity.GetString("username10", "0"); }
        set { SmartSecurity.SetString("username10", value.ToString()); }
    }
    public string username11
    {
        get { return SmartSecurity.GetString("username11", "0"); }
        set { SmartSecurity.SetString("username11", value.ToString()); }
    }



    //////////////

    public int portalindex
    {
        get { return SmartSecurity.GetInt("portalindex", 0); }
        set { SmartSecurity.SetInt("portalindex", value); }
    }

    public int changescene  //월드씬이동체크.
    {
        get { return SmartSecurity.GetInt("changescene", 0); }
        set { SmartSecurity.SetInt("changescene", value); }
    }
    public int avatapanelindex
    {
        get { return SmartSecurity.GetInt("avatapanelindex", 0); }
        set { SmartSecurity.SetInt("avatapanelindex", value); }
    }
    public int itemindex
    {
        get { return SmartSecurity.GetInt("itemindex", 0); }
        set { SmartSecurity.SetInt("itemindex", value); }
    }
    public int loginindex
    {
        get { return SmartSecurity.GetInt("loginindex", 0); }
        set { SmartSecurity.SetInt("loginindex", value); }
    }
 
    public int one //chatpanelindex
    {
        get { return SmartSecurity.GetInt("one", 0); }
        set { SmartSecurity.SetInt("one", value); }
    }
    public int btnindex
    {
        get { return SmartSecurity.GetInt("btnindex", 0); }
        set { SmartSecurity.SetInt("btnindex", value); }
    }
    public int positionindex
    {
        get { return SmartSecurity.GetInt("positionindex", 0); }
        set { SmartSecurity.SetInt("positionindex", value); }
    }
    public int nicknameindex
    {
        get { return SmartSecurity.GetInt("nicknameindex", 0); }
        set { SmartSecurity.SetInt("nicknameindex", value); }
    }
    public int playerindex
    {
        get { return SmartSecurity.GetInt("playerindex", 0); }
        set { SmartSecurity.SetInt("playerindex", value); }
    }

    public int addd //회원가입.
    {
        get { return SmartSecurity.GetInt("addd", 0); }
        set { SmartSecurity.SetInt("addd", value); }
    }
    public int playerpositionindex
    {
        get { return SmartSecurity.GetInt("playerpositionindex", 0); }
        set { SmartSecurity.SetInt("playerpositionindex", value); }
    }
    public int anidown
    {
        get { return SmartSecurity.GetInt("anidown", 0); }
        set { SmartSecurity.SetInt("anidown", value); }
    }


    public int genderindex
    {
        get { return SmartSecurity.GetInt("genderindex", 0); }
        set { SmartSecurity.SetInt("genderindex", value); }
    }

    public float stickdistance
    {
        get { return SmartSecurity.GetFloat("stickdistance", 0); }
        set { SmartSecurity.SetFloat("stickdistance", value); }
    }
    public float radious
    {
        get { return SmartSecurity.GetFloat("radious", 0); }
        set { SmartSecurity.SetFloat("radious", value); }
    }
    public float lastplaytime
    {
        get { return SmartSecurity.GetFloat("lastplaytime", 0); }
        set { SmartSecurity.SetFloat("lastplaytime", value); }
    }

    public string nowdate
    {

        get { return SmartSecurity.GetString("nowdate", "0"); }
        set { SmartSecurity.SetString("nowdate", value.ToString()); }
    }
    public string nowmonth
    {

        get { return SmartSecurity.GetString("nowmonth", "0"); }
        set { SmartSecurity.SetString("nowmonth", value.ToString()); }
    }


    public string getdatanickname
    {

        get { return SmartSecurity.GetString("getdatanickname", "0"); }
        set { SmartSecurity.SetString("getdatanickname", value.ToString()); }
    }
    public string getdataid
    {
        get { return SmartSecurity.GetString("getdataid", "0"); }
        set { SmartSecurity.SetString("getdataid", value.ToString()); }
    }
    public string getdatapassword
    {
        get { return SmartSecurity.GetString("getdatapassword", "0"); }
        set { SmartSecurity.SetString("getdatapassword", value.ToString()); }
    }

    public string usernickname
    {
        get { return SmartSecurity.GetString("usernickname", "0"); }
        set { SmartSecurity.SetString("usernickname", value.ToString()); }
    }
    public string usernicknameitem
    {
        get { return SmartSecurity.GetString("usernicknameitem", "0"); }
        set { SmartSecurity.SetString("usernicknameitem", value.ToString()); }
    }
  
    public string gamerindate
    {
        get { return SmartSecurity.GetString("gamerindate", "0"); }
        set { SmartSecurity.SetString("gamerindate", value.ToString()); }
    }
    //플레이어 감지 인덱스

    public int checkplayer0
    {
        get { return SmartSecurity.GetInt("checkplayer0", 0); }
        set { SmartSecurity.SetInt("checkplayer0", value); }
    }
    public int checkplayer1
    {
        get { return SmartSecurity.GetInt("checkplayer1", 0); }
        set { SmartSecurity.SetInt("checkplayer1", value); }
    }
    public int checkplayer2
    {
        get { return SmartSecurity.GetInt("checkplayer2", 0); }
        set { SmartSecurity.SetInt("checkplayer2", value); }
    }
    public int checkplayer3
    {
        get { return SmartSecurity.GetInt("checkplayer3", 0); }
        set { SmartSecurity.SetInt("checkplayer3", value); }
    }
    public int checkplayer4
    {
        get { return SmartSecurity.GetInt("checkplayer4", 0); }
        set { SmartSecurity.SetInt("checkplayer4", value); }
    }
    public int checkplayer5
    {
        get { return SmartSecurity.GetInt("checkplayer5", 0); }
        set { SmartSecurity.SetInt("checkplayer5", value); }
    }
    public int checkplayer6
    {
        get { return SmartSecurity.GetInt("checkplayer6", 0); }
        set { SmartSecurity.SetInt("checkplayer6", value); }
    }
    public int checkplayer7
    {
        get { return SmartSecurity.GetInt("checkplayer7", 0); }
        set { SmartSecurity.SetInt("checkplayer7", value); }
    }
    public int checkplayer8
    {
        get { return SmartSecurity.GetInt("checkplayer8", 0); }
        set { SmartSecurity.SetInt("checkplayer8", value); }
    }
    public int checkplayer9
    {
        get { return SmartSecurity.GetInt("checkplayer9", 0); }
        set { SmartSecurity.SetInt("checkplayer9", value); }
    }
    public int checkplayer10
    {
        get { return SmartSecurity.GetInt("checkplayer10", 0); }
        set { SmartSecurity.SetInt("checkplayer10", value); }
    }
    public int checkplayer11
    {
        get { return SmartSecurity.GetInt("checkplayer11", 0); }
        set { SmartSecurity.SetInt("checkplayer11", value); }
    }
    public int checkplayer12
    {
        get { return SmartSecurity.GetInt("checkplayer12", 0); }
        set { SmartSecurity.SetInt("checkplayer12", value); }
    }

    //tts 소리제어(이벤트 정보선택 음성정보).
    public int ttscontrolindex //1 세팅 (0 제한)
    {
        get { return SmartSecurity.GetInt("ttscontrolindex", 1); }
        set { SmartSecurity.SetInt("ttscontrolindex", value); }
    }

    //tts 스피드 제어인덱스
    public float ttsspeedfloat // 1f 기본값(Defalt)
    {
        get { return SmartSecurity.GetFloat("ttsspeedfloat", 1f); }
        set { SmartSecurity.SetFloat("ttsspeedfloat", value); }
    }

    public int ttsspeedindex // index 0 세팅 ( 보통 말하기)
    {
        get { return SmartSecurity.GetInt("ttsspeedindex", 0); }
        set { SmartSecurity.SetInt("ttsspeedindex", value); }
    }


    public int checkttsindex    //벽에 닿았을때 체크
    {
        get { return SmartSecurity.GetInt("checkttsindex", 0); }
        set { SmartSecurity.SetInt("checkttsindex", value); }
    }

    //조이스틱 시계방향 tts index (2시방향부터 시작)
    public int ttsindex
    {
        get { return SmartSecurity.GetInt("ttsindex", 0); }
        set { SmartSecurity.SetInt("ttsindex", value); }
    }
    public int ttsindex1
    {
        get { return SmartSecurity.GetInt("ttsindex1", 0); }
        set { SmartSecurity.SetInt("ttsindex1", value); }
    }
    public int ttsindex2
    {
        get { return SmartSecurity.GetInt("ttsindex2", 0); }
        set { SmartSecurity.SetInt("ttsindex2", value); }
    }
    public int ttsindex3
    {
        get { return SmartSecurity.GetInt("ttsindex3", 0); }
        set { SmartSecurity.SetInt("ttsindex3", value); }
    }
    public int ttsindex4
    {
        get { return SmartSecurity.GetInt("ttsindex4", 0); }
        set { SmartSecurity.SetInt("ttsindex4", value); }
    }
    public int ttsindex5
    {
        get { return SmartSecurity.GetInt("ttsindex5", 0); }
        set { SmartSecurity.SetInt("ttsindex5", value); }
    }
    public int ttsindex6
    {
        get { return SmartSecurity.GetInt("ttsindex6", 0); }
        set { SmartSecurity.SetInt("ttsindex6", value); }
    }
    public int ttsindex7
    {
        get { return SmartSecurity.GetInt("ttsindex7", 0); }
        set { SmartSecurity.SetInt("ttsindex7", value); }
    }
    public int ttsindex8
    {
        get { return SmartSecurity.GetInt("ttsindex8", 0); }
        set { SmartSecurity.SetInt("ttsindex8", value); }
    }



    public int intdata // int 형 데이터 저장 및 불러오기.
    {
        get { return SmartSecurity.GetInt("intdata", 0); }
        set { SmartSecurity.SetInt("intdata", value); }
    }
    public string stringdata // string 형 데이터 저장 및 불러오기.
    {
        get { return SmartSecurity.GetString("stringdata", "0"); }
        set { SmartSecurity.SetString("stringdata", value.ToString()); }
    }
    public float floatdata // float 형 데이터 저장 및 불러오기.
    {
        get { return SmartSecurity.GetFloat("floatdata", 0); }
        set { SmartSecurity.SetFloat("floatdata", value); }
    }


    //캐릭터 생성후 로딩패널 체크.
    public int loadingindex
    {
        get { return SmartSecurity.GetInt("loadingindex", 0); }
        set { SmartSecurity.SetInt("loadingindex", value); }
    }


    //상호작용된 오브젝트 설명 저장인덱스 (TTS)
    public string objtts
    {
        get { return SmartSecurity.GetString("objtts", "0"); }
        set { SmartSecurity.SetString("objtts", value.ToString()); }
    }
    public string objtts0
    {
        get { return SmartSecurity.GetString("objtts0", "0"); }
        set { SmartSecurity.SetString("objtts0", value.ToString()); }
    }
    public string objtts1
    {
        get { return SmartSecurity.GetString("objtts1", "0"); }
        set { SmartSecurity.SetString("objtts1", value.ToString()); }
    }
    public string objtts2
    {
        get { return SmartSecurity.GetString("objtts2", "0"); }
        set { SmartSecurity.SetString("objtts2", value.ToString()); }
    }
    public string objtts3
    {
        get { return SmartSecurity.GetString("objtts3", "0"); }
        set { SmartSecurity.SetString("objtts3", value.ToString()); }
    }
    public string objtts4
    {
        get { return SmartSecurity.GetString("objtts4", "0"); }
        set { SmartSecurity.SetString("objtts4", value.ToString()); }
    }
    public string objtts5
    {
        get { return SmartSecurity.GetString("objtts5", "0"); }
        set { SmartSecurity.SetString("objtts5", value.ToString()); }
    }
    public string objtts6
    {
        get { return SmartSecurity.GetString("objtts6", "0"); }
        set { SmartSecurity.SetString("objtts6", value.ToString()); }
    }
    public string objtts7
    {
        get { return SmartSecurity.GetString("objtts7", "0"); }
        set { SmartSecurity.SetString("objtts7", value.ToString()); }
    }
    public string objtts8
    {
        get { return SmartSecurity.GetString("objtts8", "0"); }
        set { SmartSecurity.SetString("objtts8", value.ToString()); }
    }
    public string objtts9
    {
        get { return SmartSecurity.GetString("objtts9", "0"); }
        set { SmartSecurity.SetString("objtts9", value.ToString()); }
    }
    public string objtts10
    {
        get { return SmartSecurity.GetString("objtts10", "0"); }
        set { SmartSecurity.SetString("objtts10", value.ToString()); }
    }
    public string objtts11
    {
        get { return SmartSecurity.GetString("objtts11", "0"); }
        set { SmartSecurity.SetString("objtts11", value.ToString()); }
    }

    public int autoindex  //자동로그인 체크 인덱스.
    {
        get { return SmartSecurity.GetInt("autoindex", 0); }
        set { SmartSecurity.SetInt("autoindex", value); }
    }
    public int photonroomindex  //포톤방생성인덱스.
    {
        get { return SmartSecurity.GetInt("photonroomindex", 0); }
        set { SmartSecurity.SetInt("photonroomindex", value); }
    }

    public int checkloading  //로딩체크인덱스
    {
        get { return SmartSecurity.GetInt("checkloading", 0); }
        set { SmartSecurity.SetInt("checkloading", value); }
    }

    public int checkuserevent // 유저감지 이벤트 호출시 조이스틱 제한체크
    {
        get { return SmartSecurity.GetInt("checkuserevent", 0); }
        set { SmartSecurity.SetInt("checkuserevent", value); }
    }

    public int checkgworldtts // 유저감지 이벤트 호출시 조이스틱 제한체크
    {
        get { return SmartSecurity.GetInt("checkgworldtts", 0); }
        set { SmartSecurity.SetInt("checkgworldtts", value); }
    }

}
