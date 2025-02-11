using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;
using Photon.Pun;
using BackEnd.Socketio;
using UnityEngine.SceneManagement;

public class LoginCheckController : MonoBehaviour
{
    private static LoginCheckController instance;

    public static LoginCheckController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<LoginCheckController>();
            if (instance != null) return instance;
            var container = new GameObject("LoginCheckController");
            return instance;
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {// 접속 시 반응하는 핸들러 설정
       // Backend.Notification.OnAuthorize = (bool Result, string Reason) => {
        //    Debug.Log("실시간 서버 성공 여부 : " + Result);
        //    Debug.Log("실패 시 이유 : " + Reason);
       // };

        // 실시간 알림 서버로 연결
        //Backend.Notification.Connect();
       // StartCoroutine(checkloginstatuscor());
        //StartCoroutine(testtest());
    }

   
    private void OnDisable()
    {
       // StopCoroutine(checkloginstatuscor());
        //StopCoroutine(testtest());
    }
    void Update()
    {
        if (Backend.IsInitialized)
        {
            //Backend.ErrorHandler.IwitializePoll(true); //profiler 1kb.
        }
    }
    IEnumerator testtest()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            Backend.Notification.OnServerStatusChanged = (ServerStatusType serverStatusType) => {
                Debug.Log(
                    $"[OnServerStatusChanged(서버 상태 변경)]\n" +
                    $"| ServerStatusType : {serverStatusType}\n"
                  //  [OnServerStatusChanged(서버 상태 변경)]
                 //   | ServerStatusType : Maintenance

                );
            };
        }
    }
    public GameObject checkerrorpanel;
    public GameObject loginerrorpanel;
    public float checkInterval = 5f; // 로그인 상태를 체크할 간격
    public void checkerror()
    {
        Backend.GameData.Get("", new Where(), callback => //공통에러처리.
          {

          });
        BackendReturnObject bro = Backend.BMember.IsAccessTokenAlive();
        if (!bro.IsSuccess())
        {
            var bro1 = Backend.BMember.RefreshTheBackendToken();
            var error = bro1.GetErrorCode();
            if (error == "BadUnauthorizedException")
            {
                loginerrorpanel.SetActive(true);
            }
        }
    }
    IEnumerator checkloginstatuscor()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            BackendReturnObject accessTokenCheckResult = Backend.BMember.IsAccessTokenAlive();
            if (!accessTokenCheckResult.IsSuccess())
            {
                BackendReturnObject refreshTokenResult = Backend.BMember.RefreshTheBackendToken();
                var error = refreshTokenResult.GetErrorCode();
                var checkerror = refreshTokenResult.GetMessage();
                if(!refreshTokenResult.IsSuccess())
                {
                    PhotonNetwork.Disconnect();
                    SmartSecurity.SetInt("autoindex", 0);
                    BackEndController.Instance.InsertPrivateData();
                    BackEndController.Instance.InsertPublicItemData();
                    BackEndController.Instance.InsertPublicLoginData();
                    Backend.BMember.Logout();
                    SceneManager.LoadScene(5);
                }

                /* if (error == "BadUnauthorizedException")
                 {
                if (checkerror == "bad refreshToken, 잘못된 refreshToken 입니다") //동시접속체크.
                {
                    PhotonNetwork.Disconnect();
                    SmartSecurity.SetInt("autoindex", 0);
                    BackEndController.Instance.InsertPrivateData();
                    BackEndController.Instance.InsertPublicItemData();
                    BackEndController.Instance.InsertPublicLoginData();
                    Backend.BMember.Logout();
                    SceneManager.LoadScene(5);
                    loginerrorpanel.SetActive(true);
                }
                else if(checkerror == "bad serverStatus: maintenance, 잘못된 serverStatus: maintenance 입니다") //서버점검체크.
                {
                    SceneManager.LoadScene(5);
                    checkerrorpanel.SetActive(true);
                }

                     SmartSecurity.SetInt("autoindex", 0);
                     BackEndController.Instance.InsertPrivateData();
                     BackEndController.Instance.InsertPublicData();
                     BackEndController.Instance.InsertPublicItemData();
                     BackEndController.Instance.InsertPublicLoginData();
                     Backend.BMember.Logout();
                     loginerrorpanel.SetActive(true);
                 }*/
            }

            //errorhandle check .. error
        }
    }

    public void loginerrorpanelbtn()
    {
        var bro = Backend.Utils.GetServerStatus();
        if (bro.IsSuccess())
        {
            var bro1 = (int)bro.GetReturnValuetoJSON()["serverStatus"];
            print(bro1);
            if (bro1 == 2)
            {
                print("점검중");
                //char 조회후 불러오기
                //패널띄우기.
                //BackendReturnObject chart = Backend.Chart.GetChartContents("114433");
            }
        }
        //데이터저장필요??
        //확인버튼 후 씬이동 로그인씬으로.
    }
    public void dqwwdqw()
    {
        if (Backend.IsInitialized)
        {
            
            Backend.ErrorHandler.InitializePoll(true);

            Backend.ErrorHandler.OnMaintenanceError = () => {
                Debug.Log("점검 에러 발생!!!");
                gameObject.GetComponentInChildren<Text>().text = "점검 중입니다.";
            };
            Backend.ErrorHandler.OnTooManyRequestError = () => {
                Debug.Log("403 에러 발생!!!");
                gameObject.GetComponentInChildren<Text>().text = "과도한 요청이 감지되었습니다.";
            };
            Backend.ErrorHandler.OnTooManyRequestByLocalError = () => {
                Debug.Log("403 로컬 에러 발생!!!");
                gameObject.GetComponentInChildren<Text>().text = "과도한 요청중입니다.";
            };
            Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
                Debug.Log("리프레시 불가!!!");
                gameObject.GetComponentInChildren<Text>().text = "다른 기기에서 로그인이 감지되었습니다.";
            };
        }
    }
}
