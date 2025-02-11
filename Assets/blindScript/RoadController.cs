using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using BackEnd;
using Photon.Pun;
using Apple.Accessibility;


public class RoadController : MonoBehaviour
{
    private static RoadController instance;

    public static RoadController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<RoadController>();
            if (instance != null) return instance;
            var container = new GameObject("RoadController");
            return instance;
        }
    }

    public GameObject settingmenu;
    public GameObject privacymenu;
    public GameObject playerareasizemenu;
    public GameObject eventinfomenu;
    public GameObject[] selectTt = new GameObject[3];
    public GameObject homepanel;
    public GameObject settingpanel;


    public void OnEnable()
    {
        if(SmartSecurity.GetInt("checkhomeindex") == 1)
        {
            SmartSecurity.SetInt("checkhomeindex", 0);
        }
    }

    void Start()
    {
        Screen.SetResolution(1620, 2160, true);
        Application.targetFrameRate = 30;
        DataController.Instance.settingmenuindex = 0;
        DataController.Instance.changescene = 0;
        if (SmartSecurity.GetInt("checkroadindex") == 0)
        {
            SmartSecurity.SetInt("checkroadindex", 1);
            startroad();
        }
    }
    public void settingmenupanel(int index)
    {
        if (index == 0)
        {
            settingmenu.SetActive(true);
            privacymenu.SetActive(false);
            playerareasizemenu.SetActive(false);
            eventinfomenu.SetActive(false);
        }
        else if (index == 1) //개인정보.
        {
            settingmenu.SetActive(false);
            privacymenu.SetActive(true);
            playerareasizemenu.SetActive(false);
            eventinfomenu.SetActive(false);
        }
        else if (index == 2) //정보수집영역.
        {
            settingmenu.SetActive(false);
            privacymenu.SetActive(false);
            playerareasizemenu.SetActive(true);
            eventinfomenu.SetActive(false);
            playerareaselectindex(DataController.Instance.playerareaindex);
        }
        else if (index == 3) //이벤트 정보선택.
        {
            settingmenu.SetActive(false);
            privacymenu.SetActive(false);
            playerareasizemenu.SetActive(false);
            eventinfomenu.SetActive(true);
        }
        else if (index == 4) //로그아웃.
        {
            BackEndController.Instance.logout();
            settingmenu.SetActive(false);
            privacymenu.SetActive(false);
            playerareasizemenu.SetActive(false);
            eventinfomenu.SetActive(false);
        }
        DataController.Instance.settingmenuindex = index;
    }

    public void settingpanelexit()
    {
        DataController.Instance.settingmenuindex = 0;
        settingmenupanel(DataController.Instance.settingmenuindex);
    }

    public void settingpanelstart() // 세팅버튼 누를시 패널세팅
    {
        settingmenu.SetActive(true);
        privacymenu.SetActive(false);
        reprivacymenu.SetActive(false);
        playerareasizemenu.SetActive(false);
        eventinfomenu.SetActive(false);
        ttsspeedmenu.SetActive(false);
        logoutpanel.SetActive(false);
        blindwithdrawpanel.SetActive(true);

    }

    public void logoutpanelbtn()  //설정패널 로그아웃 버튼
    {
        logoutpanel.SetActive(true);
    }


    public void settingmenubackbtn()   // 설정패널 뒤로가기(홈화면) 버튼
    {
        SmartSecurity.SetInt("checksettingindex", 0);
        settingpanel.SetActive(false);
        logoutpanel.SetActive(false);
        homepanel.SetActive(true);
    }


    #region setting menu 1.개인정보.
    public Text nicknameinfoTt; //설정메뉴 닉네임텍스트.
    public Text idinfoTt;  //설정메뉴 아이디텍스트.
    public Text passwordinfoTt; //설정메뉴 패스워드텍스트 ***형태로 나타내야함.

    public GameObject privacychangemenu;

    public Text idinfochangeTt; //개인정보 수정 아이디텍스트.
    public Text nicknameinfochangeTt; //개인정보수정 닉네임텍스트.
    public Text passwordinfochangeTt; //개인정보수정 패스워드텍스트.

    public InputField nicknamechangeinput; //개인정보수정 닉네임인풋
    public InputField passwordchangeinput; //개인정보수정 패스워드인풋.
    public InputField verifypasswordchangeinput; //개인정보수정 패스워드재확인인풋.

    public GameObject nickchangeinputobj; // 보이스오버 라벨링 회원정보 변경 
    public GameObject idchangeinputobj;   // 보이스오버 라벨링 회원정보 변경
    public GameObject passwordchangeinputobj; // 보이스오버 라벨링 회원정보 변경
    public GameObject verifypasswordchangeinputobj; //보이스오버 라벨링 회원정보 변경

    public GameObject reprivacymenu; // 회원탈퇴패널.
    public GameObject logoutpanel; //로그아웃패널.
    public GameObject nicknameerrorpanel; //닉네임에러패널.
    public GameObject nicknameerrorpanel1;//닉네임중복패널.
    public GameObject passworderrorpanel; //패스워드에러패널.
    public GameObject privacychangesuccesspanel; // 회원정보변경완료패널.

    public GameObject passwordtrueimage; //비밀번호같음.
    public GameObject passwordfalseimage; //비밀번호같지않음.

    public GameObject blindwithdrawpanel; //보이스오버가리기 회원탈퇴 UI

    public GameObject nicknameinfoTtobj;  // 보이스오버 라벨검색 회원정보변 오브젝트 
    public GameObject idinfoTtobj;        // 보이스오버 라벨검색 회원정보변경 오브젝트 
    public GameObject passwordinfoTtobj;  // 보이스오버 라벨검색 회원정보변경 오브젝트 

    public GameObject blindinfochangepanel; // 보이스오버가리기 회원정보변경 UI
    public GameObject privacychangesavebtn; // 애플플러그인 라벨정보 가져오기 

    public string[] lines;
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

    public bool IsInputValid(string input)
    { //영어 대소문자 1개이상 숫자 1이개이상 포함한 6자 이상 12자이내.
        return Regex.IsMatch(input, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{6,12}$");
    }
    private bool mainchecknick()
    {
        return Regex.IsMatch(nicknamechangeinput.text, "^[0-9a-zA-Z가-힣].{1,12}$");
    }
    public void checkpassword() //체크 패스워드 수정
    {
        int c = 0;
        SmartSecurity.SetInt("changepasswordindex", 0);
        if (c == 0)
        {
            TextAsset data = Resources.Load("BadWord") as TextAsset;
            lines = Regex.Split(data.text, LINE_SPLIT_RE);
            //   SmartSecurity.SetInt("signuppasswordcheckindex", 0);
            for (int i = 0; i < lines.Length; i++)
            {
                if (verifypasswordchangeinput.text.Contains(lines[i]))
                {
                    blindinfochangepanel.SetActive(false);
                    passworderrorpanel.SetActive(true);
                    c = 1;
                    return;
                }
            }
            if (IsInputValid(verifypasswordchangeinput.text) == false)
            {   //패스워치생성규칙에러패널true.
                blindinfochangepanel.SetActive(false);
                passworderrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (verifypasswordchangeinput.text.Replace(" ", "").Equals(""))
            { //공백이있을경우
                blindinfochangepanel.SetActive(false);
                passworderrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (c == 0)
            {
                SmartSecurity.SetInt("changepasswordindex", 1);
                //약관체크확인.
            }
        }
    }
    public void checkmainnickname() //체크 닉네임 수정.
    {
        int c = 0;
        SmartSecurity.SetInt("changenicknameindex", 0);
        if (c == 0)
        {
            TextAsset data = Resources.Load("BadWord") as TextAsset;
            lines = Regex.Split(data.text, LINE_SPLIT_RE);
            //   SmartSecurity.SetInt("signuppasswordcheckindex", 0);
            for (int i = 0; i < lines.Length; i++)
            {
                if (nicknamechangeinput.text.Contains(lines[i]))
                {
                    blindinfochangepanel.SetActive(false);
                    nicknameerrorpanel.SetActive(true);
                    c = 1;
                    return;
                }
            }
            if (mainchecknick() == false)
            {
                //패스워치생성규칙에러패널true.
                blindinfochangepanel.SetActive(false);
                nicknameerrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (nicknamechangeinput.text.Replace(" ", "").Equals(""))
            { //공백이있을경우
              //패스워치생성규칙에러패널true.
                blindinfochangepanel.SetActive(false);
                nicknameerrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (c == 0)
            {
                SmartSecurity.SetInt("changenicknameindex", 1);
                //약관체크확인.
            }
        }
    }
    public void privacyinfo() //개인정보.
    {
        nicknameinfoTt.text = SmartSecurity.GetString("loginnickname");//DataController.Instance.loginnickname;
        idinfoTt.text = SmartSecurity.GetString("loginid");//DataController.Instance.loginid;
        int password = SmartSecurity.GetString("loginpassword").Length;//DataController.Instance.loginpassword.Length;
        passwordinfoTt.text = new string('*', password);
        updateblindtext();
    }
    public void privacychangeinfo() //개인정보수정.
    {
        nicknamechangeinput.text = SmartSecurity.GetString("loginnickname");
        idinfochangeTt.text = SmartSecurity.GetString("loginid");
        //passwordchangeinput.text = SmartSecurity.GetString("loginpassword");
        int password = SmartSecurity.GetString("loginpassword").Length;
        passwordchangeinput.text = new string('*', password);
        updateblindinfochangetext();
    }
    public void privacychangebtn() //개인정보수정패널이동 
    {
        privacychangeinfo();
        verifypasswordchangeinput.text = "";
        passwordfalseimage.SetActive(false);
        passwordtrueimage.SetActive(false);
        logoutpanel.SetActive(false);
        blindinfochangepanel.SetActive(true);
        nicknameerrorpanel.SetActive(false);
        nicknameerrorpanel1.SetActive(false);
        passworderrorpanel.SetActive(false);
        privacychangesuccesspanel.SetActive(false);
        privacymenu.SetActive(false);
        privacychangemenu.SetActive(true);
    }
    public void privacychangebackbtn() // 회원정보변경 이전으로돌아가기버튼 개인정보패널이동
    {
        //privacyinfo();
        nicknameinfoTt.text = SmartSecurity.GetString("loginnickname");//DataController.Instance.loginnickname;
        idinfoTt.text = SmartSecurity.GetString("loginid");//DataController.Instance.loginid;
        int password = SmartSecurity.GetString("loginpassword").Length;//DataController.Instance.loginpassword.Length;
        passwordinfoTt.text = new string('*', password);
        privacychangemenu.SetActive(false);
        updateblindtext();
        privacymenu.SetActive(true);
    }
    public void privacychangebackbtn1() //뒤로가기버튼 개인보안화면으로이동
    {
        nicknameinfoTt.text = SmartSecurity.GetString("loginnickname");
        idinfoTt.text = SmartSecurity.GetString("loginid");
        int password = SmartSecurity.GetString("loginpassword").Length;
        passwordinfoTt.text = new string('*', password);
        privacychangemenu.SetActive(false);
        updateblindtext();
        privacymenu.SetActive(true);
    }
    public void ttsprivacychangebackbtn() // 회원정보변경 이전으로돌아가기버튼 개인정보패널이동 tts 호출
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이전으로 돌아가기 버튼 개인보안화면으로 이동합니다");
            }
        }
    }
    public void setprivacychangebtn() //개인정보수정 저장. 닉네임 중복 규칙 ,패스워드 규칙. 
    {
        if (verifypasswordchangeinput.text == DataController.Instance.loginpassword) //비밀번호 유지.
        {
            passwordfalseimage.SetActive(false);
            passwordtrueimage.SetActive(true);
            if (nicknamechangeinput.text.Length > 1)
            {
                if (nicknamechangeinput.text != DataController.Instance.loginnickname) //닉네임 변경.
                {
                    checkmainnickname();
                    if (SmartSecurity.GetInt("changenicknameindex") == 1)
                    {
                        BackendReturnObject changenickname = Backend.BMember.UpdateNickname(nicknamechangeinput.text);
                        if (changenickname.IsSuccess())
                        {
                            blindinfochangepanel.SetActive(false);
                            privacychangesuccesspanel.SetActive(true);
                            SmartSecurity.SetString("nickname", nicknamechangeinput.text);
                            SmartSecurity.SetString("loginnickname", nicknamechangeinput.text);
                            BackEndController.Instance.InsertPrivateData();
                            BackEndController.Instance.InsertPublicItemData();
                            BackEndController.Instance.InsertPublicLoginData();
                            checksavesuccess();
                            ttssavebtn();
                        }
                        else
                        {
                            string error = changenickname.GetStatusCode();
                            if (error == "409") //닉네임 중복.
                            {
                                blindinfochangepanel.SetActive(false);
                                nicknameerrorpanel1.SetActive(true);
                                if (AccessibilitySettings.IsVoiceOverRunning == true)
                                {
                                    privacychangesavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장하기버튼 닉네임중복 팝업창 활성화";
                                    Invoke("savebtnlabelreset", 1f);
                                }
                            }
                        }
                    }
                }
            }
        }
        else //비밀번호 변경.
        {
            passwordfalseimage.SetActive(true);
            passwordtrueimage.SetActive(false);
            if (verifypasswordchangeinput.text.Length > 1)
            {
                if (passwordchangeinput.text == verifypasswordchangeinput.text)
                {
                    passwordfalseimage.SetActive(false);
                    passwordtrueimage.SetActive(true);
                    checkpassword();
                    if (SmartSecurity.GetInt("changepasswordindex") == 1)
                    {
                        if (nicknamechangeinput.text.Length > 1)
                        {
                            if (nicknamechangeinput.text != DataController.Instance.loginnickname) //닉네임 변경 및 패스워드변경.
                            {
                                checkmainnickname();
                                if (SmartSecurity.GetInt("changenicknameindex") == 1)
                                {
                                    string newpassword = passwordchangeinput.text;
                                    BackendReturnObject changepassword = Backend.BMember.UpdatePassword(SmartSecurity.GetString("loginpassword"), newpassword);
                                    if (changepassword.IsSuccess())
                                    {
                                        BackendReturnObject changenickname = Backend.BMember.UpdateNickname(nicknamechangeinput.text);
                                        if (changenickname.IsSuccess())
                                        { // TTS 호출
                                            blindinfochangepanel.SetActive(false);
                                            privacychangesuccesspanel.SetActive(true);
                                            SmartSecurity.SetString("loginpassword", newpassword);
                                            SmartSecurity.SetString("nickname", nicknamechangeinput.text);
                                            SmartSecurity.SetString("loginnickname", nicknamechangeinput.text);
                                            PhotonNetwork.LocalPlayer.NickName = nicknamechangeinput.text;
                                            BackEndController.Instance.InsertPrivateData();
                                            BackEndController.Instance.InsertPublicItemData();
                                            BackEndController.Instance.InsertPublicLoginData();
                                            checksavesuccess();
                                            ttssavebtn();
                                        }
                                        else
                                        {
                                            string error = changenickname.GetStatusCode();
                                            if (error == "409") //닉네임 중복.
                                            {
                                                blindinfochangepanel.SetActive(false);
                                                nicknameerrorpanel1.SetActive(true);
                                                if (AccessibilitySettings.IsVoiceOverRunning == true)
                                                {
                                                    privacychangesavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장하기버튼 닉네임중복 팝업창 활성화";
                                                    Invoke("savebtnlabelreset", 1f);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else //패스워드변경
                            {
                                string newpassword = passwordchangeinput.text;
                                BackendReturnObject changepassword = Backend.BMember.UpdatePassword(SmartSecurity.GetString("loginpassword"), newpassword);
                                if (changepassword.IsSuccess())
                                {
                                    blindinfochangepanel.SetActive(false);
                                    privacychangesuccesspanel.SetActive(true);
                                    SmartSecurity.SetString("loginpassword", newpassword);
                                    BackEndController.Instance.InsertPrivateData();
                                    BackEndController.Instance.InsertPublicItemData();
                                    BackEndController.Instance.InsertPublicLoginData();
                                    checksavesuccess();
                                    ttssavebtn();
                                }
                            }
                        }
                    }
                }
                else
                {
                    passwordfalseimage.SetActive(true);
                    passwordtrueimage.SetActive(false);
                    if (AccessibilitySettings.IsVoiceOverRunning == true)
                    {
                        privacychangesavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장하기버튼 password생성규칙 팝업창 활성화";
                        Invoke("savebtnlabelreset", 1f);
                    }
                }
            }
        }
    }

    public void ttssavebtn() // 회원정보변경 저장하기 버튼 (TTS 호출)
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            privacychangesavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장하기버튼 회원정보변경 성공 팝업창 활성화";
            Invoke("savebtnlabelreset", 1f);
        }
    }

    public void savebtnlabelreset()
    {
        privacychangesavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장하기버튼";
        CancelInvoke("savebtnlabelreset");
    }

    public void checksavesuccess() //정보변경 저장 성공시 초기화 
    {
        passwordfalseimage.SetActive(false);
        passwordtrueimage.SetActive(false);
        verifypasswordchangeinput.text = "";
        nicknameinfoTt.text = SmartSecurity.GetString("loginnickname");
        idinfoTt.text = SmartSecurity.GetString("loginid");
        int password = SmartSecurity.GetString("loginpassword").Length;
        passwordinfoTt.text = new string('*', password);
        updateblindinfochangetext();
    }
    public void privateinputevent()  //인풋필드 입력시 체크이미지 꺼짐
    {
        passwordfalseimage.SetActive(false);
        passwordtrueimage.SetActive(false);
        if (passwordchangeinput.text.Length > 0)
        {
            int password = passwordchangeinput.text.Length;
            var changepassword = new string('*', password);
            passwordchangeinputobj.GetComponent<AccessibilityNode>().AccessibilityLabel = changepassword;
        }

        if (verifypasswordchangeinput.text.Length > 0)
        {
            int verifypassword = verifypasswordchangeinput.text.Length;
            var changeveifypassword = new string('*', verifypassword);
            verifypasswordchangeinputobj.GetComponent<AccessibilityNode>().AccessibilityLabel = changeveifypassword;
        }
    }


    public void privateinputeventend()//인풋필드 패스워드변경값 같은지 체크
    {
        if(verifypasswordchangeinput.text.Length > 1)
        {
            if (verifypasswordchangeinput.text == passwordchangeinput.text)
            {
                passwordtrueimage.SetActive(true);
            }
            else if (verifypasswordchangeinput.text == "")
            {
                passwordtrueimage.SetActive(false);
                passwordfalseimage.SetActive(false);
            }
            else
            {
                passwordfalseimage.SetActive(true);
            }
        }
    }

    public void nicknameerrorbackbtn() //회원정보변경 에러패널 비속어체크 완료버튼 
    {
        blindinfochangepanel.SetActive(true);
        nicknameerrorpanel.SetActive(false);
    }
    public void nicknameerrorbackbtn1() //회원정보변경 에러패널 닉네임중복 완료버튼 
    {
        blindinfochangepanel.SetActive(true);
        nicknameerrorpanel1.SetActive(false);
    }
    public void passworderrorbackbtn() //회원정보변경 에러패널 패스워드 완료버튼 
    {
        blindinfochangepanel.SetActive(true);
        passworderrorpanel.SetActive(false);
    }

    public void privacychangesuccessbackbtn() //회원정보변경 에러패널 회원정보변경 완료버튼 
    {
        blindinfochangepanel.SetActive(true);
    }

    public void privacybtn() // 개인/보안 버튼 -- 개인정보 회원탈퇴,개인정보(아이디,패스워드,닉네임)패널이동.
    { // 애플로그인 일때 아이디 패스워드 빈칸으로
        // privacyinfo();
        nicknameinfoTt.text = SmartSecurity.GetString("loginnickname");
        idinfoTt.text = SmartSecurity.GetString("loginid");
        int password = SmartSecurity.GetString("loginpassword").Length;
        passwordinfoTt.text = new string('*', password);
        updateblindtext();
        privacymenu.SetActive(true);
        blindwithdrawpanel.SetActive(true);
        settingmenu.SetActive(false);
    }
    public void privacywithdrawbtn() //회원탈퇴확인 패널이동.
    {
        blindwithdrawpanel.SetActive(false);
        reprivacymenu.SetActive(true);
    }
    public void privacybeforebtn() //설정메뉴패널이동.
    {
        blindwithdrawpanel.SetActive(true);
        privacymenu.SetActive(false);
        reprivacymenu.SetActive(false);
        settingmenu.SetActive(true);
    }
    public void privacybackbtn() // 개인보안 뒤로가기 버튼
    {
        blindwithdrawpanel.SetActive(true);
        privacymenu.SetActive(false);
        reprivacymenu.SetActive(false);
        settingmenu.SetActive(true);
    }
    public void privacywithdraw() //회원탈퇴버튼.
    {
        reprivacymenu.SetActive(false);
        blindwithdrawpanel.SetActive(true);
        privacymenu.SetActive(false);
        BackEndController.Instance.Withdraw();
    }
    public void privacylogoutbtn() // 설정메뉴 로그아웃패널 확인버튼
    {
        logoutpanel.SetActive(false);
        if (SmartSecurity.GetInt("ttscontrolindex") == 1)
        {
            Invoke("privacylogouton", 3.5f);
            SmartSecurity.SetInt("checkroadindex", 0);
        }
        else
        {
            BackEndController.Instance.logout();
            SmartSecurity.SetInt("checkroadindex", 0);
        }
    }

    

    public void privacylogouton() // 앱종료 n초뒤 실행 
    {
        BackEndController.Instance.logout();
    }
    public void privacywithdrawbackbtn()  // 회원탈퇴 알림패널 취소버튼 
    {
        blindwithdrawpanel.SetActive(true);
        reprivacymenu.SetActive(false);
    }

    public void updateblindtext() // 보이스오버 개인/보안  - 닉네임 아이디 패스워드 보이스오버라벨 업데이트   
    {
        nicknameinfoTtobj.GetComponent<AccessibilityNode>().AccessibilityLabel = nicknameinfoTt.text;
        idinfoTtobj.GetComponent<AccessibilityNode>().AccessibilityLabel = idinfoTt.text;
        passwordinfoTtobj.GetComponent<AccessibilityNode>().AccessibilityLabel = passwordinfoTt.text;
    }

    public void updateblindinfochangetext() // 보이스오버 회원정보변경 - 닉네임 아이디 패스워드 패스워드확인 보이스오버라벨 업데이트
    {
        nickchangeinputobj.GetComponent<AccessibilityNode>().AccessibilityLabel = nicknamechangeinput.text;
        idchangeinputobj.GetComponent<AccessibilityNode>().AccessibilityLabel = idinfochangeTt.text;
        passwordchangeinputobj.GetComponent<AccessibilityNode>().AccessibilityLabel = passwordchangeinput.text;
        verifypasswordchangeinputobj.GetComponent<AccessibilityNode>().AccessibilityLabel = "";
    }


    public void reprivacybeforebtn() //개인정보 애플회원가입,회원탈퇴패널 되돌아오기.
    {
        privacymenu.SetActive(true);
        reprivacymenu.SetActive(false);
    }
    #endregion
    #region settingmenu 2. 이벤트 수집영역.

    //애플 플러그인 라벨정보 가져오기 
    public GameObject smallareabtn;
    public GameObject bigareabtn;
    public GameObject allareabtn;

    public void playerareasizebtn() //설정메뉴 이벤트 수집영역버튼 넓은면적,좁은면적 패널이동. 
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("설정메뉴 이벤트수집영역버튼 이벤트 수집영역 설정화면으로 이동합니다");
            }
        }
        playerareasizemenu.SetActive(true);
        settingmenu.SetActive(false);
        playerareaselectindex(SmartSecurity.GetInt("playerareaindex"));
    }
    public void playerareasizebeforebtn() //이벤트 수집영역 이전이로 돌아가기 버튼
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트수집영역 이전으로 돌아가기버튼 설정화면으로 이동합니다");
            }
        }
        settingmenu.SetActive(true);
        playerareasizemenu.SetActive(false);
    }

    public void playerareasizebackbtn()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("뒤로가기버튼 설정화면으로 이동합니다");
            }
        }
        settingmenu.SetActive(true);
        playerareasizemenu.SetActive(false);
    }

    public void playerareaselectindex(int index) //오브젝트,플레이어 감지영역 선택.
    {
        for(int i = 0; i<3; i++)
        {
            selectTt[i].SetActive(false);
        }
        DataController.Instance.playerareaindex = index;
        SmartSecurity.SetInt("playerareaindex", index);
        selectTt[index].SetActive(true);
        BackEndController.Instance.InsertPrivateData();
    }
  
    public void selecttts1() //이벤트수집영역 아바타 밀접 버튼 
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            smallareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "1단계 (아바타 밀접) 활성화";
            Invoke("checkselect1", 1f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트수집영역 1단계버튼 아바타밀접을 설정하였습니다");
            }
        }
    }
    public void selecttts2() //이벤트수집영역 아바타주변버튼
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            bigareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "2단계 (아바타 주변) 좁은면적 활성화";
            Invoke("checkselect2", 1f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트수집영역 2단계버튼 아바타주변을 설정하였습니다");
            }
        }
    }
    public void selecttts3() //이벤트수집영역 보이는영역전체 버튼
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            allareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "3단계 (보이는 영역 전체) 활성화";
            Invoke("checkselect3", 1f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트수집영역 3단계버튼 보이는영역전체를 설정하였습니다");
            }
        }
    }


    public void checkselect1() //활성화일때 애플로그인 라벨 변경
    {
        smallareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "1단계 (아바타 밀접) 활성화 상태입니다";
        bigareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "2단계 (아바타 주변) 좁은면적 비활성화 상태입니다";
        allareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "3단계 (보이는 영역 전체) 비활성화 상태입니다";
        CancelInvoke("checkselect1");
    }
    public void checkselect2() //활성화일때 애플로그인 라벨 변경
    {
        smallareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "1단계 (아바타 밀접) 비활성화 상태입니다";
        bigareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "2단계 (아바타 주변) 좁은면적 활성화 상태입니다";
        allareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "3단계 (보이는 영역 전체) 비활성화 상태입니다";
        CancelInvoke("checkselect2");
    }
    public void checkselect3() //활성화일때 애플로그인 라벨 변경
    {
        smallareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "1단계 (아바타 밀접) 비활성화 상태입니다";
        bigareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "2단계 (아바타 주변) 좁은면적 비활성화 상태입니다";
        allareabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "3단계 (보이는 영역 전체) 활성화 상태입니다";
        CancelInvoke("checkselect3");
    }
    #endregion


    #region settingmenu 3. 이벤트 정보선택.
    public Toggle eventinfottstoggle;
    public Toggle dotimagetoggle;


    public void eventinfobtn() // 설정메뉴 이벤트 정보선택 패널이동.
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("설정메뉴 이벤트정보선택버튼 이벤트 정보선택설정 화면으로 이동합니다");
            }
        }  
        checkeventinfottsbtn();
        checkeventdotimagebtn();
        eventinfomenu.SetActive(true);
        settingmenu.SetActive(false);
    }
    public void eventinfobeforebtn() //설정 이벤트정보선택 이전으로 돌아가기버튼  설정화면으로 이동
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트정보선택 이전으로돌아가기 버튼 설정메뉴 화면으로 돌아갑니다");
            }
        }
        settingmenu.SetActive(true);
        eventinfomenu.SetActive(false);
    }

    public void eventinfobackbtn() //설정 이벤트정보선택 뒤로가기버튼 설정화면으로 이동
    {
        if(AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트정보선택 뒤로가기버튼 설정화면으로 돌아갑니다");
            }
        }
        settingmenu.SetActive(true);
        eventinfomenu.SetActive(false);
    }

    public void eventinfottsbtn() //음성정보체크버튼.
    {
        if(AccessibilitySettings.IsVoiceOverRunning == true)
        {
            if (eventinfottstoggle.isOn == true)
            {
                SmartSecurity.SetInt("ttscontrolindex", 1);
                eventinfottstoggle.GetComponent<AccessibilityNode>().AccessibilityLabel = "음성정보 활성화";
                Invoke("ttsinfolabelchange", 1f);
            }
            else
            {
                SmartSecurity.SetInt("ttscontrolindex", 0);
                eventinfottstoggle.GetComponent<AccessibilityNode>().AccessibilityLabel = "음성정보 비활성화";
                Invoke("ttsinfolabelchange", 1f);
            }
        }
        else
        {
            if (eventinfottstoggle.isOn == true)
            {
                SmartSecurity.SetInt("ttscontrolindex", 1);
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트 정보선택 음성정보버튼 음성정보가 활성화됩니다");
            }
            else
            {
                SmartSecurity.SetInt("ttscontrolindex", 0);
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("이벤트 정보선택 음성정보버튼 음성정보가 비활성화됩니다");
            }
        }
       
    }

    public void ttsinfolabelchange()  //음성정보 애플플러그인 라벨변경
    {
        if (eventinfottstoggle.isOn == true)
        {
            eventinfottstoggle.GetComponent<AccessibilityNode>().AccessibilityLabel = "음성정보 활성화 상태입니다";
        }
        else
        {
            eventinfottstoggle.GetComponent<AccessibilityNode>().AccessibilityLabel = "음성정보 비활성화 상태입니다";
        }
        CancelInvoke("ttsinfolabelchange");
    }

    public void checkeventinfottsbtn() //체크음성정보.
    {
        if(SmartSecurity.GetInt("ttscontrolindex")==0)
        {
            eventinfottstoggle.isOn = false;
        }
        else
        {
            eventinfottstoggle.isOn = true;
        }
    }

    public void checkeventdotimagebtn() //체크촉각이미지토클
    {
        if(SmartSecurity.GetInt("dotimageindex") == 0)
        {
            dotimagetoggle.isOn = false;
        }
        else
        {
            dotimagetoggle.isOn = true;
        }
    }


    public void dotimagetogglebtn() // 설정이벤트정보 촉각이미지 토글  
    {

    }

    #endregion

    #region settingmenu 4. 읽기속도

    //애플 플러그인 라벨 가져오기
    public GameObject ttsspeedbtn1;  //보통 
    public GameObject ttsspeedbtn2;  //더빠르게읽기 
    public GameObject ttsspeedbtn3;  //빠르게읽기 
    public GameObject ttsspeedbtn4;  //느리게읽기 
    public GameObject ttsspeedbtn5;  //더느리게읽기 

    string[] stringttsspeed = { "더빠르게 읽기" , "빠르게 읽기" , "보통" , "느리게 읽기" , "더느리게 읽기" };

    public GameObject ttsspeedmenu;
    public GameObject[] ttsspeedonimageobj = new GameObject[5];

    public void ttsspeedpanelopenbtn() //설정메뉴 읽기속도 패널이동
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("설정메뉴 읽기속도버튼 읽기속도설정 화면으로 이동합니다");
            }
        }
        settingmenu.SetActive(false);
        ttsspeedmenu.SetActive(true);
        ttsspeedseletindex(SmartSecurity.GetInt("ttsspeedindex"));
    }
    public void ttsspeedpanelbackbtn()
    {
        RoadTTS.Instance.audioSource.Stop();
        ttsspeedmenu.SetActive(false);
        settingmenu.SetActive(true);
    }
    public void ttsspeedseletindex(int index)
    {
        for(int i = 0; i < 5; i++)
        {
            ttsspeedonimageobj[i].SetActive(false);
        }
        ttsspeedonimageobj[index].SetActive(true);
        DataController.Instance.ttsspeedindex = index;
        SmartSecurity.SetInt("ttsspeedindex", index);
        BackEndController.Instance.InsertPrivateData();
        print(DataController.Instance.ttsspeedindex);
        print(DataController.Instance.ttsspeedfloat);
    }
    public void ttsspeedseletbtn0() //1f 보통
    {
        SmartSecurity.SetFloat("ttsspeedfloat", 1f);
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            ttsspeedbtn1.GetComponent<AccessibilityNode>().AccessibilityLabel = "보통 활성화";
            Invoke("checkapplelabel1", 0.5f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts(stringttsspeed[2]);
            }
        }
    }
    public void ttsspeedseletbtn1() //1.8f 더빠르게
    {
        SmartSecurity.SetFloat("ttsspeedfloat", 1.8f);
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            ttsspeedbtn2.GetComponent<AccessibilityNode>().AccessibilityLabel = "더빠르게읽기 활성화";
            Invoke("checkapplelabel2", 0.5f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts(stringttsspeed[0]);
            }
        }
    }
    public void ttsspeedseletbtn2() //1.6f 빠르게 
    {
        SmartSecurity.SetFloat("ttsspeedfloat", 1.6f);
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            ttsspeedbtn3.GetComponent<AccessibilityNode>().AccessibilityLabel = "빠르게 읽기 활성화";
            Invoke("checkapplelabel3", 0.5f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts(stringttsspeed[1]);
            }
        }
    }
    public void ttsspeedseletbtn3() // 0.7f 느리게
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            ttsspeedbtn4.GetComponent<AccessibilityNode>().AccessibilityLabel = "느리게읽기 활성화";
            Invoke("checkapplelabel4", 0.5f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts(stringttsspeed[3]);
            }
        }
        SmartSecurity.SetFloat("ttsspeedfloat", 0.7f);
    }
    public void ttsspeedseletbtn4() // 0.5f 더 느리게
    {
        SmartSecurity.SetFloat("ttsspeedfloat", 0.5f);
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            ttsspeedbtn5.GetComponent<AccessibilityNode>().AccessibilityLabel = "더느리게 읽기 활성화";
            Invoke("checkapplelabel5", 0.5f);
        }
        else
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts(stringttsspeed[4]);
            }
        }
    }

    public void ttsspeedbackbtn() //읽기속도선택화면 뒤로가기 버튼 TTS
    {
        if(AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("읽기속도선택 뒤로가기버튼 설정화면으로 이동합니다");
            }
        }
    }
    public void ttsspeedbeforebtn() //읽기속도선택화면 이전으로 돌아가기 버튼 TTS
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                RoadTTS.Instance.alltts("읽기속도선택 이전으로돌아가기버튼 설정화면으로 이동합니다");
            }
        }
    }

    public void checkapplelabel1() //말하기 속도 보통 애플플러그인 라벨 변경  
    {
        ttsspeedbtn1.GetComponent<AccessibilityNode>().AccessibilityLabel = "보통 활성화 상태입니다";
        ttsspeedbtn2.GetComponent<AccessibilityNode>().AccessibilityLabel = "더빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn3.GetComponent<AccessibilityNode>().AccessibilityLabel = "빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn4.GetComponent<AccessibilityNode>().AccessibilityLabel = "느리게읽기 비활성화 상태입니다";
        ttsspeedbtn5.GetComponent<AccessibilityNode>().AccessibilityLabel = "더느리게읽기 비활성화 상태입니다";
        CancelInvoke("checkapplelabel1");
    }
    public void checkapplelabel2() //말하기 속도 더빠르게읽기 애플플러그인 라벨 변경  
    {
        ttsspeedbtn1.GetComponent<AccessibilityNode>().AccessibilityLabel = "보통 비활성화 상태입니다";
        ttsspeedbtn2.GetComponent<AccessibilityNode>().AccessibilityLabel = "더빠르게읽기 활성화 상태입니다";
        ttsspeedbtn3.GetComponent<AccessibilityNode>().AccessibilityLabel = "빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn4.GetComponent<AccessibilityNode>().AccessibilityLabel = "느리게읽기 비활성화 상태입니다";
        ttsspeedbtn5.GetComponent<AccessibilityNode>().AccessibilityLabel = "더느리게읽기 비활성화 상태입니다";
        CancelInvoke("checkapplelabel2");
    }
    public void checkapplelabel3() //말하기 속도 빠르게읽기 애플플러그인 라벨 변경  
    {
        ttsspeedbtn1.GetComponent<AccessibilityNode>().AccessibilityLabel = "보통 비활성화 상태입니다";
        ttsspeedbtn2.GetComponent<AccessibilityNode>().AccessibilityLabel = "더빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn3.GetComponent<AccessibilityNode>().AccessibilityLabel = "빠르게읽기 활성화 상태입니다";
        ttsspeedbtn4.GetComponent<AccessibilityNode>().AccessibilityLabel = "느리게읽기 비활성화 상태입니다";
        ttsspeedbtn5.GetComponent<AccessibilityNode>().AccessibilityLabel = "더느리게읽기 비활성화 상태입니다";
        CancelInvoke("checkapplelabel3");
    }
    public void checkapplelabel4() //말하기 속도 느리게읽기 애플플러그인 라벨 변경  
    {
        ttsspeedbtn1.GetComponent<AccessibilityNode>().AccessibilityLabel = "보통 비활성화 상태입니다";
        ttsspeedbtn2.GetComponent<AccessibilityNode>().AccessibilityLabel = "더빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn3.GetComponent<AccessibilityNode>().AccessibilityLabel = "빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn4.GetComponent<AccessibilityNode>().AccessibilityLabel = "느리게읽기 활성화 상태입니다";
        ttsspeedbtn5.GetComponent<AccessibilityNode>().AccessibilityLabel = "더느리게읽기 비활성화 상태입니다";
        CancelInvoke("checkapplelabel4");
    }
    public void checkapplelabel5() //말하기 속도 더느리게읽기 애플플러그인 라벨 변경  
    {
        ttsspeedbtn1.GetComponent<AccessibilityNode>().AccessibilityLabel = "보통 비활성화 상태입니다";
        ttsspeedbtn2.GetComponent<AccessibilityNode>().AccessibilityLabel = "더빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn3.GetComponent<AccessibilityNode>().AccessibilityLabel = "빠르게읽기 비활성화 상태입니다";
        ttsspeedbtn4.GetComponent<AccessibilityNode>().AccessibilityLabel = "느리게읽기 비활성화 상태입니다";
        ttsspeedbtn5.GetComponent<AccessibilityNode>().AccessibilityLabel = "더느리게읽기 활성화 상태입니다";
        CancelInvoke("checkapplelabel5");
    }
    #endregion

    public void checkplayerareabtn(int index)
    {
        DataController.Instance.checkplayerareaindex = index;
    }
    public void playerareaybtn()
    {
        DataController.Instance.playerareaindex = DataController.Instance.checkplayerareaindex;
        checkplayerareabtn(DataController.Instance.checkplayerareaindex);
        playerareasizemenu.SetActive(false);
        settingmenu.SetActive(true);
    }
    #region 웓드패널

    public GameObject goworldbtn; // 애플플러그인 보이스오버 라벨 변경 
    
    public void sceneroad() //월드이동 버튼
    {
        if(AccessibilitySettings.IsVoiceOverRunning == true)
        {
            goworldbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "입장 버튼 전시월드로 이동합니다";
        }
        Invoke("sceneon", 3f);
    }
    public void sceneon()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            goworldbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "입장 버튼";
        }
        SmartSecurity.SetInt("photonroomindex", 1);
        SceneManager.LoadScene(3, LoadSceneMode.Single);
        CancelInvoke("sceneon");
    }
    #endregion


    public GameObject blindhome; //앱 설명 텍스트
    public GameObject blindnameTt; // 앱 이름 텍스트
    public GameObject blindblebtn; // 블루투스 버튼 
    public GameObject blindsettingbtn; //설정 버튼
    public GameObject blindavatabtn; // 아바타 버튼
    public GameObject blindworldbtn; // 월드 버튼

    public void startroad()
    {
        string st = " 앱 메뉴 버튼이 화면을 4분할 하여 크게 4개버튼으로 구성되어 있습니다.왼쪽 상단 영역은 앱 소개 버튼,왼쪽 하단 영역은 아바타 꾸미기 버튼,오른쪽 상단 영역은 세팅 버튼,오른쪽 하단 영역은 박물관 월드 버튼, 이렇게 4개 영역의 버튼으로 구성되어 있습니다.";
        //아래 텍스트 2~3 초뒤 실행으로 변경
        blindhome.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindblebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindsettingbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindavatabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindworldbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        Invoke("startroadon", 2f);
    }
    public void startroadon()
    {
        blindhome.GetComponent<AccessibilityNode>().AccessibilityLabel = "'Blind Metaverse'는 시각장애인과 비시각 장애인을 모두를 위한 서비스로, 작품과 유물에 대한 음성 안내를 통해 사용자들이 전시를 더 깊이 이해하고 체험할 수 있도록 돕습니다. 본 앱의 화면터치는 태블릿을 양손으로 잡은 상태에서 엄지 손가락으로 좌우, 위아래 엄지 손가락의 위치를 이동하면서 버튼을 터치할 수 있도록 버튼 UI가 배치되어 있으며, 태블릿 4축 센서 기술을 활용하여 월드 전시 공간 내 이동 및 전시물 정보를 직 , 간접적으로 경험 할 수 있습니다.";
        blindblebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "Bluetooth 버튼";
        blindsettingbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "세팅 버튼.개인정보 영역으로 회원탈퇴, 로그아웃 기능이 있습니다.";
        blindavatabtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "아바타 버튼.아바타 성별, 얼굴, 의상 등 꾸미기 기능이 있습니다.";
        blindworldbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "월드 버튼.전시관 월드로 진입하는 기능이 있습니다.";
        blindnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "Blind Metaverse";

        CancelInvoke("startroadon");
    }

    public void startroadTton()
    {
        
        CancelInvoke("startroadTton");
    }

    //아바타 보이스오버 초기 세팅 오브젝트 작업
    public GameObject blindavatanameTt; // 아바타화면 상단바 메인 텍스트
    public GameObject blindavatabackbtn; // 아바타화면 상단바 뒤로가기 버튼
    public GameObject blindwbtn; //아바타화면 여성선택버튼
    public GameObject blindmbtn; //아바타화면 남성선택버튼

    public GameObject blindmemotionpreviousbtn;
    public GameObject blindmemotionnextbtn;
    public GameObject blindmfacepreviousbtn;
    public GameObject blindmfacenextbtn;
    public GameObject blindmtoppreviousbtn;
    public GameObject blindmtopnextbtn;

    public GameObject blindwemotionpreviousbtn;
    public GameObject blindwemotionnextbtn;
    public GameObject blindwfacepreviousbtn;
    public GameObject blindwfacenextbtn;
    public GameObject blindwtoppreviousbtn;
    public GameObject blindwtopnextbtn;

    public GameObject blindavataclosebtn;
    public GameObject blindavatasavebtn;

    //월드 보이스오버 초기 세팅 오브젝트 작업
    public GameObject blindworldnameTt; // 월드화면 상단바 메인 텍스트
    public GameObject blindworldbackbtn; // 월드화면 상단바 뒤로가기 버튼
    public GameObject blindworldinfonameTt; // 월드화면 선택월드 이름 텍스트
    public GameObject blindworldinfoTt; // 월드화면 선택월드 설명 텍스트
    public GameObject blindworldclosebtn; // 월드화면 화면닫기 버튼
    public GameObject blindworldgobtn; // 월드화면 월드진입 버튼

    public void startavata()
    {
        string st = "아바타 꾸미기 화면 입니다.자신의 아바타를 꾸미는 화면으로 화면 위쪽에서 아래로 4개 선택 메뉴 영역이 있습니다. 1. 여성 , 남성 성별 선택 , 2. 감정 선택 , 3. 얼굴 선택 , 4. 의상 선택 , 입니다. 각 영역에는 왼쪽 , 오른쪽 버튼이 있어 좌우 버튼을 터치해서 선택을 할 수 있습니다.화면 하단에는 오른쪽에 아바타 꾸미기 저장 버튼이 있고 왼쪽에는 아바타 설정 취소 기능 겸 이전 홈화면으로 되돌아가는 닫기 버튼이 있습니다.";
        blindavatabackbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindavatanameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmemotionpreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmemotionnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmfacepreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmfacenextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmtoppreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindmtopnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwemotionpreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwemotionnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwfacepreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwfacenextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwtoppreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindwtopnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindavataclosebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindavatasavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        AccessibilityNotification.PostLayoutChangedNotification();
        Invoke("startavataon", 2.5f);
       // Invoke("startavataTton", 6f);
    }
    public void startavataon()
    {
        blindavatanameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "아바타 설정";
        blindavatabackbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "뒤로가기 버튼";
        if (DataController.Instance.wmindex == 0)
        {
            blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성성별선택버튼 활성화상태입니다";
            blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성성별선택버튼 비활성화상태입니다";
        }
        else
        {
            blindmbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성성별선택버튼 활성화상태입니다";
            blindwbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성성별선택버튼 비활성화상태입니다";
        }
        blindmemotionpreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성감정선택 버튼";
        blindmemotionnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성감정선택 버튼";
        blindmfacepreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성얼굴선택 버튼";
        blindmfacenextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성얼굴선택 버튼";
        blindmtoppreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성의상선택 버튼";
        blindmtopnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "남성의상선택 버튼";

        blindwemotionpreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성감정선택 버튼";
        blindwemotionnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성감정선택 버튼";
        blindwfacepreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성얼굴선택 버튼";
        blindwfacenextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성얼굴선택 버튼";
        blindwtoppreviousbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성의상선택 버튼";
        blindwtopnextbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "여성의상선택 버튼";

        blindavataclosebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "닫기 버튼";
        blindavatasavebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "저장 버튼";

        CancelInvoke("startavataon");
    }
    public void startavataTton()
    {
        blindavatanameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "아바타 설정";
        CancelInvoke("startavataTton");
    }

    public void startworld()
    {
        string st = "메타 월드 국립고궁 박물관 소개 화면 입니다.국립고궁박물관은 조선시대와 대한제국기의 고귀한 왕실유물을 자랑스럽게 전시하며, 조선왕조에 대한 올바른 역사 이해와 관람객들에게 품격 높은 문화경험을 선사합니다.메타버스 상에서의 월드를 구현하여, 국립고궁박물관이라는 고유한 문화 공간을 형성하고자 합니다.메타버스 전시 월드를 통해 전통과 현대의 만남을 표현하며, 이를 통해 관람자들에게 조선왕실의 풍요로운 문화유산을 현대적이고 다양한 방식으로 전달합니다.화면 하단 오른쪽에 국립고궁 박물관 메타월드 진입버튼이 있습니다.화면 하단 왼쪽에는 홈화면으로 되돌아가는 닫기 버튼이 있습니다.";
        blindworldnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindworldbackbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindworldinfonameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindworldinfoTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindworldclosebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindworldgobtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        Invoke("startworldon", 2.8f);
    }
    public void startworldon()
    {
        blindworldnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "월드 선택";
        blindworldbackbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "뒤로가기 버튼";
        blindworldinfonameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "국립고궁박물관";
        blindworldinfoTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "국립고궁박물관은 조선시대와 대한제국기의 고귀한 왕실유물을 자랑스럽게 전시하며, 조선 왕조에 대한 올바른 역사 이해와 관람객들에게 품격 높은 문화 경험을 선사합니다.메타버스 상에서의 월드를 구현하여, 국립고궁박물관이라는 고유한 문화 공간을 형성하고자 합니다. 메타버스 전시 월드를 통해 전통과 현대의 만남을 표현하며, 이를 통해 관람자들에게 조선왕실의 풍요로운 문화유산을 현대적이고 다양한 방식으로 전달합니다.";
        blindworldclosebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "닫기 버튼";
        blindworldgobtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "입장 버튼";
        CancelInvoke("startworldon");
    }


    //세팅 보이스오버 초기 세팅 오브젝트 작업
    public GameObject blindsettingnameTt; // 설정화면 상단바 메인 텍스트
    public GameObject blindsettingbackbtn; // 설정화면 상단바 뒤로가기 버튼
    public GameObject blindsettingprivacybtn; // 설정화면 개인보안 버튼
    public GameObject blindsettinglogoutbtn; // 설정화면 로그아웃 버튼
    public GameObject blindsettingclosebtn; // 설정화면 닫기 버튼

    public void startsetting()
    {
        string st = "앱 세팅 화면 입니다. 화면 중앙에 2개의 가로버튼이 위에서 아래로 순차적으로 있습니다. 1. 개인정보 설정 , 2. 로그아웃 입니다.화면 하단 아래에 홈화면으로 되돌아가는 이전으로 돌아가기 버튼이 있습니다.";
        blindsettingnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindsettingbackbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindsettingprivacybtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindsettinglogoutbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        blindsettingclosebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = st;
        AccessibilityNotification.PostLayoutChangedNotification();
        Invoke("startsettingon", 2.5f);
    }
    public void startsettingon()
    {
        blindsettingnameTt.GetComponent<AccessibilityNode>().AccessibilityLabel = "설정";
        blindsettingbackbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "뒤로가기 버튼";
        blindsettingprivacybtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "개인 / 보안 버튼";
        blindsettinglogoutbtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "로그아웃 버튼";
        blindsettingclosebtn.GetComponent<AccessibilityNode>().AccessibilityLabel = "이전으로 돌아가기 버튼";

        CancelInvoke("startsettingon");
    }

   
}
