using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.IO;
using Photon.Pun;
using Apple.Accessibility; // apple plugin
using UnityEngine.EventSystems;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;

public class TestBackEndController : MonoBehaviour
{
    public void googleloginbtn()
    {
        //TheBackend.ToolKit.GoogleLogin.iOS.GoogleLogin(GoogleLoginCallback);
      /*  BackendFederation.iOS.OnGoogleLogin += (bool isSuccess, string errorMessage, string token) =>
        {
            if (isSuccess == false)
            {
                Debug.LogError(errorMessage);
                return;
            }
            else
            {
                Debug.Log(token);

            }
            //var loginBro = Backend.BMember.AuthorizeFederation(token, FederationType.Google);
           // Debug.Log("로그인 결과 : " + loginBro);
        };*/
    }

    void testscene()
    {
        SceneManager.LoadScene(1);
    }
    private static TestBackEndController instance;

    public static TestBackEndController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<TestBackEndController>();
            if (instance != null) return instance;
            var container = new GameObject("TestBackEndController");
            return instance;
        }
    }
  
    public void delectdatabtn()
    {
        PlayerPrefs.DeleteAll();
    }


    #region 자동로그인.
    public Toggle autoLoginToggle; // 자동 로그인 체크 박스
    public void checkautoindex() //로그아웃시 자동로그인체크해제.
    {
        if (SmartSecurity.GetInt("addd") == 1)
        {
            if (autoLoginToggle.isOn == true)
            {
                SmartSecurity.SetInt("autoindex", 1);
            }
            else
            {
                SmartSecurity.SetInt("autoindex", 0);
            }
        }
        else
        {
            SmartSecurity.SetInt("autoindex", 0);
        }
    }
    // 자동 로그인 정보 저장 함수
    private void SaveAutoLoginInfo()
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin(SmartSecurity.GetString("loginid"), SmartSecurity.GetString("loginpassword"));
        if (bro.IsSuccess())
        {
            BackendReturnObject BRO = Backend.BMember.GetUserInfo();
            string nickname = BRO.GetReturnValue().Split(':')[4].Split(',')[0];
            if (nickname == "null")
            {
                mainnicknamesettingpanel.SetActive(true);
            }
            else
            {
                Where where = new Where();
                where.IsNotEmpty("myinsideid");
                var bro2 = Backend.GameData.GetMyData("privatedata", where, 1);
                var myinsideid = bro2.GetReturnValuetoJSON()["rows"][0]["myinsideid"]["S"].ToString();
                if (DataController.Instance.insideid == myinsideid)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    SmartSecurity.SetInt("autoindex", 0);
                    autoLoginToggle.isOn = false;
                }
            }
        }
    }

    #endregion

    private IAppleAuthManager appleAuthManager;

    void Start()
    {
        Screen.SetResolution(1620, 2160, true);
        panelsettingfalse(); 
        print(SmartSecurity.GetInt("autoindex"));
        if (SmartSecurity.GetInt("autoindex") == 1)
        {
            SaveAutoLoginInfo();
        }
        else
        {
            autoLoginToggle.isOn = false;
        }
        SmartSecurity.SetInt("signupidcheckindex", 0);
        SmartSecurity.SetInt("signuppasswordcheckindex", 0);
        mainnicknamesettingpanel.SetActive(false);
        Data_Wifi();
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            appleAuthManager = new AppleAuthManager(deserializer);
        }
    }
    void Update()
    {
        if (appleAuthManager != null)
        {
            appleAuthManager.Update();
        }
    }


    public void OnAppleLoginButtonClick()
    {
        if (appleAuthManager == null)
        {
            Debug.LogError("AppleAuthManager가 초기화되지 않았습니다.");
            return;
        }

        appleAuthManager.LoginWithAppleId(
            new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName),
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    SetupAppleData(appleIdCredential.User, appleIdCredential);
                }
            },
            error =>
            {
                Debug.LogError("Apple Login Failed: " + error.ToString());
            });
    }

    public void SetupAppleData(string appleUserId, ICredential receivedCredential)
    {
        var appleIdCredential = receivedCredential as IAppleIDCredential;
        if (appleIdCredential.FullName != null)
        {
            Debug.Log($"Full Name: {appleIdCredential.FullName.GivenName} {appleIdCredential.FullName.FamilyName}");
        }

        if (appleIdCredential != null && appleIdCredential.IdentityToken != null)
        {
            var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken, 0, appleIdCredential.IdentityToken.Length);
            Debug.Log("Apple Identity Token: " + identityToken);

            //로그인 성공 회원가입 화면으로 이동 로직작성

            mainsignuppanelopen();
        }
        else
        {
            Debug.LogError("Invalid Apple Credential");
        }
    }

    public void panelsettingfalse() // start panel false
    {
        mainsignuppanel.SetActive(false);
        getpasswordpanel.SetActive(false);
    }
    public void Awake()
    {
        Initialized();
    }
    public void quityes() //메인패널 데이터저장 후 앱종료
    {
        InsertPrivateData();
        InsertPublicItemData();
        InsertPublicLoginData();
        Application.Quit();
    }
    public void loginquityes()
    {
        Application.Quit();
    }
    public void chckserver() //서버점검 체크.
    {
        var bro = Backend.Utils.GetServerStatus();
        if (bro.IsSuccess())
        {
            var bro1 = (int)bro.GetReturnValuetoJSON()["serverStatus"];
            print(bro1);
            if(bro1 == 2)
            {
                print("점검중");
                //char 조회후 불러오기
                //패널띄우기.
                //BackendReturnObject chart = Backend.Chart.GetChartContents("114433");
            }
        }
    }
    public void beforelogincheckserver()
    {
        string tempNotice = Backend.Notice.GetTempNotice();
        print(tempNotice);
        if (string.IsNullOrEmpty(tempNotice))
        {
            return;
        }

        JsonData data = JsonMapper.ToObject(tempNotice);
        if (bool.Parse(data["isUse"].ToString()))
        {
            //Debug.Log(data["contents"].ToString());
        }
    }
    public Text wifiTt;

    public void Data_Wifi()//update or checkcoru. 포톤스크립트로 이동.
    {
        //인터넷연결이 끊겼을떄 , 룸에서 나가졌으면 서버에 플레이어 포지션값 불러오기.
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //인터넷 연결이 안되었을때 현재위치 다시 불러오기.
            wifiTt.text = "Not";

        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            //데이터 연결이 되었을때.
            wifiTt.text = "DataNetwork";
        }
        else
        {
            //와이파이 연결이 되었을때.
            wifiTt.text = "Wifi";
        }
    }
    public void Initialized() //초기화 후 버천체크.
    {
        BackendReturnObject bro = Backend.Initialize(true);
        //BackendReturnObject versionbro = Backend.Utils.GetLatestVersion();
        //var version = versionbro.GetReturnValuetoJSON()["version"].ToString();
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공: " + bro.ToString());
        }
        else
        {
            Debug.LogError("초기화 실패: " + bro.ToString());
        }
    }



    #region 메인로그인
    public GameObject mainloginpanel; //메인로그인 패널
    
    public InputField maincustomloginidinput; //로그인아이디인풋필드.
    public InputField maincustomloginpasswordinput; //로그인패스워드인풋필드.
    public InputField mainnicknameinput; //닉네임인풋필드.
    public GameObject mainsignuppanel;   //회원가입패널.
    public GameObject blindsignuppanel; // 보이스오버가리기 회원가입 UI
    public GameObject blinderrorbotpanel; // 보이스오버가리기 회원가입 약관동의 UI 

    public GameObject mainnicknamesettingpanel; //닉네임가입패널. 
    public GameObject nicknameerrorpanel; //닉네임규칙에러패널.
    public GameObject nicknameerrorpanel1; //사용중인 닉네임에러패널.
    public GameObject nicknamesuccesspanel;
    public GameObject blindnickerrorbotpanel; //보이스오버가리기 닉네임설정 UI

    public InputField maincustomIdinputTt; //아이디인풋필드.
    public InputField maincustompasswordinputTt; //비밀번호인풋필드.
    public InputField maincustomverifypasswordinputTt; //비밀번호재확인인풋필드.
    public GameObject passworderrorpanel; //패스워드생성규칙에러패널.
    public GameObject iderrorpanel1; //아이디중복에러패널.
    public GameObject iderrorpanel;//아이디생성규칙에러패널.
    public GameObject signupsuccesspanel; //회원가입 완료패널.

    public GameObject privacyallagreepanel; // 개인정보 전체동의패널.
    public GameObject privacyalltruecheckimage; // 개인정보 전체동의 활성화 이미지.
    public GameObject privacyallfalsecheckimage; // 개인정보 전체동의 비활성화 이미지.

    public GameObject privacyagreepanel; //개인회원 약관동의패널.
    public GameObject privacytruecheckimage; // 개인회원 약관동의 활성화 이미지.
    public GameObject privacyfalsecheckimage; //개인정보 약관동의 비활성화 이미지.

    public GameObject privacyagreepanel1; // 개인정보 수집 및 이용 동의패널.
    public GameObject privacytruecheckimage1; //개인정보 수집 및 이용 활성화 이미지.
    public GameObject privacyfalsecheckimage1; //개인정보 수집 및 이용 비활성화 이미지.

    public InputField getpasswordidinput; //비번찾기 아이디 
    public InputField getpasswordnicknameinput; // 비번찾기 닉네임
    public GameObject getpasswordpanel; //비번찾기패널.
    public GameObject getpassworderrorpanel; //비밀번호 확인패널.
    public Text getpasswordTt; //비번확인텍스트.
    public GameObject getpasswordobj; // 애플플러그인접근성 라벨정보 가져오기
    public GameObject blindgetpasserrorpanel; // 보이스오버가리 비밀번호찾기 UI

    public GameObject getpasswordnullidpanel; //없는 ID 에러패널.
    public GameObject getpasswordnullnicknamepanel; // 잘못된 닉네임 에러패널.


    public void getpasswordpanelbtn()//비밀번호찾기패널
    {
        mainloginpanel.SetActive(false);
        getpassworderrorpanel.SetActive(false);
        getpasswordnullidpanel.SetActive(false);
        getpasswordnullnicknamepanel.SetActive(false);
        getpasswordidinput.text = "";
        getpasswordnicknameinput.text = "";
        getpasswordpanel.SetActive(true);
    }
    public void getpasswordpanelbackbtn()//비밀번호찾기패널 뒤로가기
    {
        blindgetpasserrorpanel.SetActive(true);
        maincustomloginidinput.text = "";
        maincustomloginpasswordinput.text = "";
        getpassworderrorpanel.SetActive(false);
        getpasswordnullidpanel.SetActive(false);
        getpasswordnullnicknamepanel.SetActive(false);
        getpasswordpanel.SetActive(false);
        mainloginpanel.SetActive(true);
    }
    public void getpasswordbtn() //비밀번호 찾기.
    {
        BackendReturnObject bro = Backend.BMember.CustomLogin("운영자", "1111");
        if (bro.IsSuccess())
        {
            var bro1 = Backend.Social.GetUserInfoByNickName(getpasswordnicknameinput.text);
            string error = bro1.GetStatusCode();
            if(error =="200") // 성공 
            {
                Where where = new Where();
                var username = bro1.GetReturnValuetoJSON()["row"]["nickname"].ToString();
                where.Equal("loginnickname", username);
                BackendReturnObject data = Backend.GameData.Get("publiclogindata", where, 1);
                string myname = data.GetReturnValuetoJSON()["rows"][0]["loginnickname"]["S"].ToString();
                string myid = data.GetReturnValuetoJSON()["rows"][0]["loginid"]["S"].ToString();
                string mypassword = data.GetReturnValuetoJSON()["rows"][0]["loginpassword"]["S"].ToString();
                if(getpasswordidinput.text == myid)
                {
                    SmartSecurity.SetString("getdatanickname", myname);
                    SmartSecurity.SetString("getdataid", myid);
                    SmartSecurity.SetString("getdatapassword", mypassword);
                    getpasswordlength(mypassword);
                }
                else
                {
                    blindgetpasserrorpanel.SetActive(false);
                    getpasswordnullidpanel.SetActive(true);
                }
            }
            else
            {
                if(error == "404")
                {
                    blindgetpasserrorpanel.SetActive(false);
                    getpasswordnullnicknamepanel.SetActive(true);
                }
                Backend.BMember.Logout();
            }
        }
    }
    public void getpasswordlength(string mypassword) //비밀번호가리기.
    {
        if (mypassword.Length == 7 || mypassword.Length == 9 || mypassword.Length == 11)
        {
            int mypasswordlength1 = ((mypassword.Length / 2));
            int mypasswordlength2 = ((mypassword.Length / 2));
            string mypassword1 = mypassword.Substring(0, mypasswordlength1 - 1);
            string mypassword2 = mypassword.Substring(mypasswordlength1 + 1, mypasswordlength2);
            string mypassword3 = mypassword1 + "**" + mypassword2;
            getpasswordTt.text = "당신의 비밀번호는 "+ mypassword3 + " 입니다." + "\n" + "중간의 마킹된 문자를 기억하셔" + "\n" + "비밀번호를 찾으시기 바랍니다.";
            getpasswordobj.GetComponent<AccessibilityNode>().AccessibilityLabel = getpasswordTt.text;
            blindgetpasserrorpanel.SetActive(false);
            getpassworderrorpanel.SetActive(true);
        }
        else
        {
            int mypasswordlength1 = ((mypassword.Length / 2) - 1);
            int mypasswordlength2 = ((mypassword.Length / 2) + 1);
            string mypassword1 = mypassword.Substring(0, mypasswordlength1);
            string mypassword2 = mypassword.Substring(mypasswordlength2, mypasswordlength1);
            string mypassword3 = mypassword1 + "**" + mypassword2;
            getpasswordTt.text = "당신의 비밀번호는 " + mypassword3 + " 입니다."+ "\n" + "중간의 마킹된 문자를 기억하셔" + "\n" + "비밀번호를 찾으시기 바랍니다.";
            getpasswordobj.GetComponent<AccessibilityNode>().AccessibilityLabel = getpasswordTt.text;
            blindgetpasserrorpanel.SetActive(false);
            getpassworderrorpanel.SetActive(true);
        }
        Backend.BMember.Logout();
    }
    public void getpasswordsurebtn() //비밀번호 찾기 후 내 정보가져와서 바로 로그인다음씬이동.
    { //로그인,패널끄기 ,씬이동
        blindgetpasserrorpanel.SetActive(true);
        Backend.BMember.Logout();
        BackendReturnObject bro = Backend.BMember.CustomLogin(SmartSecurity.GetString("getdataid"), SmartSecurity.GetString("getdatapassword"));
        if (bro.IsSuccess())
        {
            getpassworderrorpanel.SetActive(false);
            SceneManager.LoadScene(1);
        }
    }
    public void getpasswordnullidbtn()
    {
        blindgetpasserrorpanel.SetActive(true);
        getpasswordnullidpanel.SetActive(false);
    }
    public void getpasswordnullnicknamebtn()
    {
        blindgetpasserrorpanel.SetActive(true);
        getpasswordnullnicknamepanel.SetActive(false);
    }
   
    public void maincustomlogin() // 메인 커스텀 로그인버튼
    {
       // print("로그인버튼 호출");
        string id = maincustomloginidinput.text;
        string password = maincustomloginpasswordinput.text;
        BackendReturnObject bro = Backend.BMember.CustomLogin(id, password);
        if (bro.IsSuccess())
        {
            BackendReturnObject BRO = Backend.BMember.GetUserInfo();
            string nickname = BRO.GetReturnValue().Split(':')[4].Split(',')[0];
            if (nickname == "null")
            {
                mainloginpanel.SetActive(false);
                mainnicknamesettingpanel.SetActive(true);
            }
            else
            {
                Where where = new Where();
                where.IsNotEmpty("myinsideid");
                var bro2 = Backend.GameData.GetMyData("privatedata", where, 1);
                var myinsideid = bro2.GetReturnValuetoJSON()["rows"][0]["myinsideid"]["S"].ToString();
                if (DataController.Instance.addd == 0) //기기삭제후 체크는 여기로됨. 인사이드아이디 다름.
                {
                    if (myinsideid != DataController.Instance.insideid)  //기기 재설치.
                    {
                        //기기변경 하지 않음 게임 시작
                        PlayerPrefs.DeleteAll();
                        SmartSecurity.SetInt("addd", 1);
                        pasingautologin();
                        OnClickGetPrivateContents();
                        SetLocalId();
                        InsertPrivateData();
                        InsertPublicItemData();
                        InsertPublicLoginData();
                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                    else
                    {
                        SmartSecurity.SetInt("addd", 1);
                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                }
                else
                {
                    if (myinsideid == DataController.Instance.insideid)
                    {
                        //기기변경 하지 않음 게임 시작
                        SmartSecurity.SetInt("addd", 1);
                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                    else
                    {
                        //기기변경으로 간주하고 서버의 데이터를 로컬로 덮어씌움 , 그후 인사이드아이디 변경 하고난뒤에 서버에저장
                        PlayerPrefs.DeleteAll();
                        SmartSecurity.SetInt("addd", 1);
                        OnClickGetPrivateContents();
                        pasingautologin();
                        SetLocalId();
                        InsertPrivateData();
                        InsertPublicItemData();
                        InsertPublicLoginData();
                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                }
            
            }
        }
        else
        {
            string geterror = bro.GetMessage();
            if(geterror == "bad customId, 잘못된 customId 입니다")
            {
                //존재하지 않는 아이디의 경우
                print("idbad");
            }
            else if(geterror == "bad customPassword, 잘못된 customPassword 입니다")
            {
                print("passwordbad");
            }
            else if(geterror == "bad serverStatus: maintenance, 잘못된 serverStatus: maintenance 입니다")
            {
                print("serverbad");
            }
            else if(geterror == "Gone user, 사라진 user 입니다")
            {
                print("withdrawbad");
            }
           
        }
    }
    public void pasingautologin()
    {
        if(autoLoginToggle.isOn == true)
        {
            SmartSecurity.SetInt("autoindex", 1);
        }
        else
        {
            SmartSecurity.SetInt("autoindex", 0);
        }
    }
    public bool IsInputValid(string input)
    { //영어 대소문자 1개이상 숫자 1이개이상 포함한 6자 이상 12자이내.
        return Regex.IsMatch(input, @"^(?=.*[a-zA-Z])(?=.*[0-9]).{6,12}$"); 
    }
    private bool mainchecknick()
    {
        return Regex.IsMatch(mainnicknameinput.text, "^[0-9a-zA-Z가-힣].{1,12}$");
    }
    public void checkmainnickname()
    {
        int c = 0;
        SmartSecurity.SetInt("checknicknameindex", 0);
        if (c==0)
        {
            TextAsset data = Resources.Load("BadWord") as TextAsset;
            lines = Regex.Split(data.text, LINE_SPLIT_RE);
            //   SmartSecurity.SetInt("signuppasswordcheckindex", 0);
            for (int i = 0; i < lines.Length; i++)
            {
                if (mainnicknameinput.text.Contains(lines[i]))
                {
                    nicknameerrorpanel.SetActive(true);
                    c = 1;
                    return;
                }
            }
            if (mainchecknick() == false)
            {
                // Debug.Log("패스워드는 영어숫자 혼용 1자이상 12자 이내입력");
                //패스워치생성규칙에러패널true.
                blindnickerrorbotpanel.SetActive(false);
                nicknameerrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (mainnicknameinput.text.Replace(" ", "").Equals(""))
            { //공백이있을경우
              //패스워치생성규칙에러패널true.
                blindnickerrorbotpanel.SetActive(false);
                nicknameerrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (c == 0)
            {
                SmartSecurity.SetInt("checknicknameindex", 1);
                //약관체크확인.
            }
        }
    }
    public void mainustomnickname()
    {
        checkmainnickname();
        if(SmartSecurity.GetInt("checknicknameindex")==1)
        {
            BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(mainnicknameinput.text);
            if(bro.IsSuccess())
            {
                BackendReturnObject nick=Backend.BMember.CreateNickname(mainnicknameinput.text);
                if(nick.IsSuccess())
                {
                    //데이터저장.패널종료.
                    // mainnicknamesettingpanel.SetActive(false);
                    blindnickerrorbotpanel.SetActive(false);
                    SmartSecurity.SetString("nickname", mainnicknameinput.text);
                    SmartSecurity.SetString("loginnickname", mainnicknameinput.text);
                    PhotonNetwork.LocalPlayer.NickName = mainnicknameinput.text;
                    SetLocalId();
                    SmartSecurity.SetInt("addd", 1);
                    SmartSecurity.SetInt("ttscontrolindex", 1);
                    SmartSecurity.SetFloat("ttsspeedfloat", 1f);
                    SmartSecurity.SetInt("playerareaindex", 2);
                    SmartSecurity.SetString("iteminfoTt", "미소 짖는 눈으로 살짝 웃는 얼굴에 갈색 긴 생머리와 가르마 앞머리에 짙은 갈색 눈동자가 어울리는 얼굴에 스쿨룩 스타일의 연한 갈색 재킷과 하얀 브라우스 그리고 짙은 갈색 짧은 치마로 매칭한 복장");
                    DataController.Instance.nicknameindex = 1;
                    InsertPrivateData();
                    InsertPublicItemData();
                    InsertPublicLoginData(); 
                    nicknamesuccesspanel.SetActive(true);
                }
            }
            else
            {
                string error = bro.GetStatusCode();
                if(error == "409")
                { //이미 중복된 닉네임이 있는 경우
                    blindnickerrorbotpanel.SetActive(false);
                    nicknameerrorpanel1.SetActive(true);
                }
            }
        }
    }

    public void nicknamesuccessbtn()
    {
        blindnickerrorbotpanel.SetActive(true);
        nicknamesuccesspanel.SetActive(false);
        mainnicknamesettingpanel.SetActive(false);
        mainloginpanel.SetActive(true);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void checkpassword()
    {
        int c = 0;
        SmartSecurity.SetInt("signuppasswordcheckindex", 0);
        if (c == 0)
        {
            TextAsset data = Resources.Load("BadWord") as TextAsset;
            lines = Regex.Split(data.text, LINE_SPLIT_RE);
            //   SmartSecurity.SetInt("signuppasswordcheckindex", 0);
            for (int i = 0; i < lines.Length; i++)
            {
                if (maincustompasswordinputTt.text.Contains(lines[i]))
                {
                    passworderrorpanel.SetActive(true);
                    c = 1;
                    return;
                }
            }
            if (IsInputValid(maincustompasswordinputTt.text) == false)
            {
                //패스워치생성규칙에러패널true.
                passworderrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (maincustompasswordinputTt.text.Replace(" ", "").Equals(""))
            { //공백이있을경우
              //패스워치생성규칙에러패널true.
                passworderrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (c == 0)
            {
                SmartSecurity.SetInt("signuppasswordcheckindex", 1);
                //약관체크확인.
            }
        }
    }
    public void checkidword()
    {
        int c = 0;
        SmartSecurity.SetInt("signupidcheckindex", 0);
        if (c==0)
        {
            TextAsset data = Resources.Load("BadWord") as TextAsset;
            lines = Regex.Split(data.text, LINE_SPLIT_RE);
            for (int i = 0; i < lines.Length; i++)
            {
                if (maincustomIdinputTt.text.Contains(lines[i]))
                {
                    iderrorpanel.SetActive(true);
                    c = 1;
                    return;
                }
            }
            if (IsInputValid(maincustomIdinputTt.text) == false)
            {
              //  Debug.Log("아이디는 영어숫자 혼용 6자이상 12자 이내입력");
                //아이디생성규칙에러패널true.
                iderrorpanel.SetActive(true);
                //SmartSecurity.SetInt("signuppasswordcheckindex", 0);
                c = 1;
                return;
            }
            else if(maincustomIdinputTt.text.Replace(" ", "").Equals(""))
            { //공백이있을경우
              //아이디생성규칙에러패널true.
                iderrorpanel.SetActive(true);
                c = 1;
                return;
            }
            else if (c == 0)
            { //패시워드 에러체크.
                SmartSecurity.SetInt("signupidcheckindex", 1);
            }
        }
    }

    // 회원가입 버튼.
    //회원가입시 전에있던 로컬 데이터 유지됨
    public void maincustomsign()
    {
        PlayerPrefs.DeleteAll();
        string id = maincustomIdinputTt.text;
        string password = maincustompasswordinputTt.text;
        string verifypassword = maincustomverifypasswordinputTt.text;
        if (password == verifypassword) //패스워드체크 
        {
            checkidword();
            if (SmartSecurity.GetInt("signupidcheckindex") == 1)
            {
                checkpassword();
                if (SmartSecurity.GetInt("signuppasswordcheckindex") == 1)
                {
                    if(privacyagreeint>=1 && privacyagreeint1>=1 )
                    {
                        BackendReturnObject bro = Backend.BMember.CustomSignUp(id, password);
                        if (bro.IsSuccess())
                        {
                            //성공시 회원가입패널false , 닉네임 패널오픈 토글체크초기화 아이디비번인풋 "" 처리
                            SmartSecurity.SetString("loginid", id);
                            SmartSecurity.SetString("loginpassword", password);
                            signupsuccesspanel.SetActive(true);
                            blinderrorbotpanel.SetActive(false);
                        }
                        else
                        {
                            string error = bro.GetStatusCode();
                            if (error == "409")
                            {
                                //아이디 중복 에러패널 true.
                                iderrorpanel1.SetActive(true);
                                blinderrorbotpanel.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
        else
        { //비밀번호 불일치.
            passworderrorpanel.SetActive(true);
            blinderrorbotpanel.SetActive(false);
        }

    }
   
    public void mainsignuppanelopen() //회원가입패널 열기.
    {
        maincustomIdinputTt.text = "";
        maincustompasswordinputTt.text = "";
        maincustomverifypasswordinputTt.text="";
        mainloginpanel.SetActive(false);
        privacyagreeint = 0;
        privacyagreeint1 = 0;
        privacyalltruecheckimage.SetActive(false);
        privacytruecheckimage.SetActive(false);
        privacytruecheckimage1.SetActive(false);
        privacyallfalsecheckimage.SetActive(true);
        privacyfalsecheckimage.SetActive(true);
        privacyfalsecheckimage1.SetActive(true);
        mainsignuppanel.SetActive(true);
    }
    public void mainsignuppanelbackbtn() //회원가입패널 닫기.
    {
        maincustomloginidinput.text = "";
        maincustomloginpasswordinput.text = "";
        iderrorpanel.SetActive(false);
        iderrorpanel1.SetActive(false);
        passworderrorpanel.SetActive(false);
        signupsuccesspanel.SetActive(false);
        mainsignuppanel.SetActive(false);
        mainloginpanel.SetActive(true);
    }
    public void iderrorbackbtn() //회원가입 아이디에러 확인버튼 
    {
        blinderrorbotpanel.SetActive(true);
        iderrorpanel.SetActive(false);
    }
    public void iderrorbackbtn1() //회원가입 아이디에러 중복 확인버튼 
    {
        blinderrorbotpanel.SetActive(true);
        iderrorpanel1.SetActive(false);
    }
    public void signupsuccessbtn()  //로그인화면으로가기.
    {
        maincustomloginidinput.text = "";
        maincustomloginpasswordinput.text = "";
        signupsuccesspanel.SetActive(false);
        mainsignuppanel.SetActive(false);
        mainloginpanel.SetActive(true);
    }
    public void passworderrorbackbtn() //회원가입 패스워드에러 확인버튼 
    {
        SmartSecurity.SetInt("signuppasswordcheckindex", 0);
        blinderrorbotpanel.SetActive(true);
        passworderrorpanel.SetActive(false);
    }
    public void nicknamepanelbackbtn()
    {
        blindnickerrorbotpanel.SetActive(true);
        mainnicknameinput.text = "";
        nicknameerrorpanel.SetActive(false);
        nicknameerrorpanel1.SetActive(false);
        nicknamesuccesspanel.SetActive(false);
        mainnicknamesettingpanel.SetActive(false);
        mainloginpanel.SetActive(true);
    }

    public void nicknameerrorbackbtn() // 닉네임 규칙에러패널 확인버튼 
    {
        nicknameerrorpanel.SetActive(false);
        blindnickerrorbotpanel.SetActive(true);
    }
    public void nicknameerrorbackbtn1()  // 닉네임 중복에러패널 확인버튼 
    {
        nicknameerrorpanel1.SetActive(false);
        blindnickerrorbotpanel.SetActive(true);
    }


    int privacyagreeint; //정보 동의 체크인덱스
    int privacyagreeint1;
  
    public void privacyagreeallviewbtn() // 개인정보 전체 약관동의 상세보기.
    {
        blindsignuppanel.SetActive(false);
        privacyallagreepanel.SetActive(true);
    }
    public void privacyallagreebackbtn() //개인정보 전체 약관동의 취소버튼.
    {
        privacyagreeint = 0;
        privacyagreeint1 = 0;
        privacyalltruecheckimage.SetActive(false);
        privacytruecheckimage.SetActive(false);
        privacytruecheckimage1.SetActive(false);
        privacyallfalsecheckimage.SetActive(true);
        privacyfalsecheckimage.SetActive(true);
        privacyfalsecheckimage1.SetActive(true);
        privacyallagreepanel.SetActive(false);
        blindsignuppanel.SetActive(true);
    }
    public void privacyallagreebtn() //개인정보 전체 약관동의버튼.
    {
        privacyagreeint = 1;
        privacyagreeint1 = 1;
        privacyalltruecheckimage.SetActive(true);
        privacytruecheckimage.SetActive(true);
        privacytruecheckimage1.SetActive(true);
        privacyallfalsecheckimage.SetActive(false);
        privacyfalsecheckimage.SetActive(false);
        privacyfalsecheckimage1.SetActive(false);
        privacyallagreepanel.SetActive(false);
        blindsignuppanel.SetActive(true);
    }

    public void privacyagreeviewbtn() //개인회원 약관 동의 상세보기.
    {
        blindsignuppanel.SetActive(false);
        privacyagreepanel.SetActive(true);
    }
    public void privacyagreebackbtn() //개인회원약관 취소버튼.
    {
        privacyagreeint = 0;
        privacytruecheckimage.SetActive(false);
        privacyfalsecheckimage.SetActive(true);
        checkprivacyagree();
        privacyagreepanel.SetActive(false);
        blindsignuppanel.SetActive(true);
    }
    public void privacyagreebtn() //개인회원약간 동의버튼.
    {
        privacyagreeint = 1;
        privacytruecheckimage.SetActive(true);
        privacyfalsecheckimage.SetActive(false);
        checkprivacyagree();
        privacyagreepanel.SetActive(false);
        blindsignuppanel.SetActive(true);
    }


    public void privacyagreeviewbtn1() //개인정보 수집 및 이용 상세보기.
    {
        blindsignuppanel.SetActive(false);
        privacyagreepanel1.SetActive(true);
    }
    public void privacyagreebackbtn1() //개인정보 수집 및 이용 취소버튼.
    {
        privacyagreeint1 = 0;
        privacytruecheckimage1.SetActive(false);
        privacyfalsecheckimage1.SetActive(true);
        checkprivacyagree();
        privacyagreepanel1.SetActive(false);
        blindsignuppanel.SetActive(true);
    }
    public void privacyagreebtn1() //개인정보 수집 및 이용 동의버튼.
    {
        privacyagreeint1 = 1;
        privacytruecheckimage1.SetActive(true);
        privacyfalsecheckimage1.SetActive(false);
        checkprivacyagree();
        privacyagreepanel1.SetActive(false);
        blindsignuppanel.SetActive(true);
    }



    //delete
    public void privacviewagreebtn() //개인회원약관동의 패널동의.
    {
        privacyagreepanel.SetActive(false);
        blindsignuppanel.SetActive(true);
    }
    public void privacviewagreebtn1() //개인정보 수집 및 이용 패널동의.
    {
        privacyagreepanel1.SetActive(false);
        blindsignuppanel.SetActive(true);
    }
    //--delete


    public void checkprivacyagree()
    { // 전시물 설명패널 전시물이미지 기본
        if (privacyagreeint >= 1 && privacyagreeint1 >= 1)
        {
            privacyalltruecheckimage.SetActive(true);
            privacyallfalsecheckimage.SetActive(false);
        }
        else
        {
            privacyalltruecheckimage.SetActive(false);
            privacyallfalsecheckimage.SetActive(true);


        }
    }

    #region 애플로그인

    public void appleloginbtn()
    {
        BackendReturnObject bro = Backend.BMember.AuthorizeFederation("idToken", FederationType.Apple, "siwa");
        print(bro);
        if (bro.IsSuccess())
        {
            Debug.Log("APPLE 로그인 성공");
            //성공 처리
        }
        else
        {
            Debug.LogError("Apple 로그인 실패");
            //실패 처리
        }
    }


    
    #endregion

    public void mainpasswordinputblind() //패스워드 가리기
    {
        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            int checkpassword = maincustompasswordinputTt.text.Length;
            var password = new string('*', checkpassword);
            maincustompasswordinputTt.GetComponent<AccessibilityNode>().AccessibilityLabel = password;
        }
    }

#endregion

#if UNITY_ANDROID
    public void testgpgsloginbtn()
     {
         if(Debug.isDebugBuild == false)
         {
             PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
               .RequestServerAuthCode(false)
               .RequestEmail()
               .RequestIdToken()
               .Build();
             PlayGamesPlatform.InitializeInstance(config);
             PlayGamesPlatform.DebugLogEnabled = false;
             PlayGamesPlatform.Activate();
             GoogleAuth();
         }
     }
     public void GoogleAuth()
     {
         if (PlayGamesPlatform.Instance.localUser.authenticated == true) //이미 로그인된 경우 
         {
             BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
         }
         else
         {
             //로딩바 넣기
             Social.localUser.Authenticate((bool Success) =>
             {
                 if (Success)
                 {
                     BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                     SceneManager.LoadScene(1, LoadSceneMode.Single);
                 }
                 else
                 {
                     Debug.Log("구글 로그인 실패");
                     return;
                 }
             });
         }
     }
     private string GetTokens()
     {
         if (PlayGamesPlatform.Instance.localUser.authenticated)
         {
             //유저 토큰 받기 첫번째 방법
             string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
             //두번째 방법
             // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
             return _IDtoken;
         }
         else
         {
             //Debug.Log("접속되어있지 않습니다. 잠시 후 다시 시도하세요.");
             return null;
         }
     }

#endif
    public string[] lines;
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

#region 데이터 저장 및 데이터 파싱.

    private string[] _strings =
    {
        "a", "T", "Q", "4", "e", "n", "d", "K", "k", "W", "p", "o", "L", "i", "G",
        "y", "z", "X", "J", "u", "E", "5", "7", "0", "V", "b", "2", "1", "P", "B"
    };
    public void SetLocalId()
    {
        SmartSecurity.SetString("insideid", _strings[Random.Range(0, 30)] + _strings[Random.Range(0, 30)] +
                                            _strings[Random.Range(0, 30)] + _strings[Random.Range(0, 30)] +
                                            _strings[Random.Range(0, 30)] + _strings[Random.Range(0, 30)] +
                                            _strings[Random.Range(0, 30)] + _strings[Random.Range(0, 30)] +
                                            _strings[Random.Range(0, 30)] + _strings[Random.Range(0, 30)] +
                                            _strings[Random.Range(0, 30)] + _strings[Random.Range(0, 30)]);
    }
   
    public void OnClickGetPrivateContents()
    {
        Where where = new Where();
        where.IsNotEmpty("intData");
        var BRO = Backend.GameData.GetMyData("privatedata", where, 1);
        if (BRO.IsSuccess())
        {
            print(BRO.GetReturnValue());
            print("데이터 로드");
            GetGameInfoprivate(BRO.GetReturnValuetoJSON());
        }
        else
        {
            print("오류");
            //오류
        }
    }
    void GetGameInfoprivate(JsonData returnData)
    {
        // ReturnValue가 존재하고, 데이터가 있는지 확인
        if (returnData != null)
        {
            //Debug.Log("데이터가 존재합니다.");
            // rows 로 전달받은 경우 
            if (returnData.Keys.Contains("rows"))
            {
                JsonData rows = returnData["rows"];
                GetDataprivate(rows[0]);
            }

            // row 로 전달받은 경우
            else if (returnData.Keys.Contains("row"))
            {
                JsonData row = returnData["row"];
                GetDataprivate(row[0]);
            }
        }
        else
        {
            //Debug.Log("데이터가 없습니다.");
        }
    }
    void GetDataprivate(JsonData data)
    {
        // 아래는 해당 키가 존재하는지 확인하고 데이터를 파싱하는 방법입니다. 
        if (data.Keys.Contains("intData"))
        {
            var int_Data = data["intData"][0].ToString().Split('◙');
            print(int_Data);
            for (var i = 0; i < int_Data.Length - 1; i++)
            {
                var data2 = int_Data[i].ToString().Split('║');
                int resultdata;
                if(int.TryParse(data2[1],out resultdata))
                {
                    SmartSecurity.SetInt(data2[0], resultdata);
                }
            }
        }
        if (data.Keys.Contains("floatData"))
        {
            var float_Data = data["floatData"][0].ToString().Split('◙');
            print(float_Data);
            for (var i = 0; i < float_Data.Length - 1; i++)
            {
                var data2 = float_Data[i].ToString().Split('║');
                SmartSecurity.SetFloat(data2[0], float.Parse(data2[1]));
            }
        }
        if (data.Keys.Contains("stringData"))
        {
            var string_Data = data["stringData"][0].ToString().Split('◙');
            print(string_Data);
            for (var i = 0; i < string_Data.Length - 1; i++)
            {
                var data2 = string_Data[i].ToString().Split('║');
                SmartSecurity.SetString(data2[0], data2[1]);
            }
        }
        print("파싱완료");
    }
    public void InsertPrivateData()
    {

        Param param = new Param();

        var intData = "";

        if (SmartSecurity.GetString("int_smart_name") != "")
        {
            var int_Datas = (SmartSecurity.GetString("int_smart_name") + "ꂊ").Replace("》ꂊ", "").Replace("《", "")
                .Split('》');

            for (var i = 0; i < int_Datas.Length; i++)
            {
                intData += int_Datas[i] + "║" + SmartSecurity.GetInt(int_Datas[i]) + "◙";
            }
        }
        param.Add("intData", intData);

        var floatData = "";

        if (SmartSecurity.GetString("float_smart_name") != "")
        {
            var float_Datas = (SmartSecurity.GetString("float_smart_name") + "ꂊ").Replace("》ꂊ", "")
                .Replace("《", "")
                .Split('》');

            for (var i = 0; i < float_Datas.Length; i++)
            {
                floatData += float_Datas[i] + "║" + SmartSecurity.GetFloat(float_Datas[i]) + "◙";
            }
        }
        param.Add("floatData", floatData);

        var stringData = "";

        if (SmartSecurity.GetString("string_smart_name") != "")
        {
            var string_Datas = (SmartSecurity.GetString("string_smart_name") + "ꂊ").Replace("《inside_id》", "")
                .Replace("》ꂊ", "").Replace("《", "")
                .Split('》');

            for (var i = 0; i < string_Datas.Length; i++)
            {
                stringData += string_Datas[i] + "║" + SmartSecurity.GetString(string_Datas[i]) + "◙";
            }
        }

        param.Add("stringData", stringData);
        param.Add("myinsideid", SmartSecurity.GetString("insideid"));
        BackendReturnObject BRO = Backend.GameData.Insert("privatedata", param);
        SmartSecurity.SetString("myprivateindata", BRO.GetInDate());
    }
   

    public void InsertPublicItemData() //유저 아이템 정보 감정 머리 상의 하의 (컬럼 4개) , 닉네임조회 (컬럼1개) ,여성아바타 추가.
    {
        Param param = new Param();
        param.Add("wmsureindex", SmartSecurity.GetInt("wmsureindex"));  //성별체크.
        param.Add("itememotion", SmartSecurity.GetInt("itememotion"));
        param.Add("itemface", SmartSecurity.GetInt("itemface"));
        param.Add("itemtop", SmartSecurity.GetInt("itemtop"));
        param.Add("witememotion", SmartSecurity.GetInt("witememotion"));
        param.Add("witemtop", SmartSecurity.GetInt("witemtop"));
        param.Add("witemface", SmartSecurity.GetInt("witemface"));
        param.Add("iteminfoTt", SmartSecurity.GetString("iteminfoTt"));
        param.Add("mynickname", SmartSecurity.GetString("nickname"));
        param.Add("dotmcharimageindex", SmartSecurity.GetInt("dotmcharimageindex"));
        param.Add("dotwcharimageindex", SmartSecurity.GetInt("dotwcharimageindex"));
        BackendReturnObject bro = Backend.GameData.Insert("publicitemdata", param);
        SmartSecurity.SetString("mypublicitemdata", bro.GetInDate());
    }
    public void InsertPublicLoginData() //유저 아이디 ,닉네임 중복체크.
    {
        Param param = new Param();
        param.Add("loginid", SmartSecurity.GetString("loginid"));
        param.Add("loginpassword", SmartSecurity.GetString("loginpassword"));
        param.Add("loginnickname", SmartSecurity.GetString("loginnickname"));
        BackendReturnObject bro = Backend.GameData.Insert("publiclogindata", param);
        SmartSecurity.SetString("mypubliclogindata", bro.GetInDate());
    }
#endregion
  
}

