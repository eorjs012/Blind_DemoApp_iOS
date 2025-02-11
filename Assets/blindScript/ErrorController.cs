using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ErrorController : MonoBehaviour
{

    private static ErrorController instance;

    public static ErrorController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<ErrorController>();
            if (instance != null) return instance;
            var container = new GameObject("ErrorController");
            return instance;
        }
    }
   
    void Start()
    {
        Screen.SetResolution(1620, 2160, true);
       
    }

    
    void Update()
    {
        
    }

    public void gologinscene()
    {
        SceneManager.LoadScene(0);
    }
    /*
     *  public Text errorTt;
    Backend.ErrorHandler.Poll();
        if (Backend.IsInitialized)
        {
            Backend.ErrorHandler.InitializePoll(true);
            Backend.ErrorHandler.OnMaintenanceError = () =>
            {
                errorTt.text = "점검 중입니다. /n 로그인화면으로 돌아갑니다.";
            };
Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () =>
{
    errorTt.text = "다른 기기에서 로그인이 감지되었습니다. /n 로그인화면으로 돌아갑니다. ";
};
Backend.ErrorHandler.OnTooManyRequestError = () =>
{
    errorTt.text = "과도한 요청이 감지되었습니다.";
};
Backend.ErrorHandler.OnTooManyRequestByLocalError = () =>
{
    errorTt.text = "과도한 요청중입니다.";
};
        }
     * */
}
