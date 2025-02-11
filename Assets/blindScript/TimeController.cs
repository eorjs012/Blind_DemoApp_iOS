using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using Photon.Pun;

public class TimeController : MonoBehaviour
{
    private static TimeController instance;

    public static TimeController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<TimeController>();
            if (instance != null) return instance;
            var container = new GameObject("TimeController");
            instance = container.AddComponent<TimeController>();
            return instance;
        }
    }
    private string _url = "https://pyougzoo.ourplan.kr/";
    private string _timeData;
    private int serverTime;
    public Text timeTt;

    //배터리 상태확인
    public Slider batslider;
    public Text batTt;
    public void Awake()
    {
        //obj 으로 동작해야함
        var obj = FindObjectsOfType<DataController>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine("OfflineCompensation");
        }
        else
        {
            StopCoroutine("OfflineCompensation");
            Destroy(gameObject);

        }
    }
    private void Update()
    {
        // 로컬시간
        //  var y = DateTime.Now.Year;
        // var m = DateTime.Now.Month;
        // var d = DateTime.Now.Day;
        // var h = DateTime.Now.Hour;
        // var min = DateTime.Now.Minute;
        //  timeTt.text = h + " : " + min + "  " + y + " - " + m + " - " + d;
    }



    [Obsolete] //더 이상 사용하지 않는 코드라는 경고만 출력.
    public IEnumerator OfflineCompensation()
    {
        while(true)
        {
            WWW www = new WWW(_url);
            yield return www;
            _timeData = www.text;
            string[] words = _timeData.Replace("<body>", "@").Split('@');
            var a = words[1].Replace("<br />", "");
            string todaydate = a.Split('/')[0].Split('-')[1];
            string todaymonth = a.Split('/')[0].Split('-')[0];
            string mid = a.Split('/')[1].Split('<')[0];

            string y = a.Split('/')[0].Split('-')[2];
            string m = a.Split('/')[0].Split('-')[0].Trim();
            string d = a.Split('/')[0].Split('-')[1];
            string h = a.Split('/')[1].Split(':')[0];
            string min = a.Split('/')[1].Split(':')[1];
            timeTt.text = h + " 시 " + min +" 분" + " / " + y + " 년 " + m + " 월 " + d + " 일";


            float number = SystemInfo.batteryLevel; // 베터리 충전량 가져오기 (0 ~ 1);
            batslider.value = number;
            batTt.text = (number * 100) + " %";
            Debug.Log(SystemInfo.batteryLevel);


            switch (SystemInfo.batteryStatus) //베터리 상태 가져오기
            {
                case BatteryStatus.Unknown:
                    Debug.Log("충전 상태를 알 수 없음");
                    break;

                case BatteryStatus.Discharging:
                    Debug.Log("충전 케이블이 연결되지 않았고 충전도 되지 않는 상태");
                    break;

                case BatteryStatus.NotCharging:
                    Debug.Log("충전 케이블이 연결되었지만 충전이 되지 않는 상태");
                    break;

                case BatteryStatus.Charging:
                    Debug.Log("충전 케이블이 연결되어 있고 충전되고 있는 상태");
                    break;

                case BatteryStatus.Full:
                    Debug.Log("충전 케이블이 연결되어 있고 베터리가 가득찬 상태");
                    break;
            }
            //  var nowTime = (float)TimeSpan.Parse(mid).TotalSeconds;
            //var compensationTime = 0f;
            //  int num = int.Parse(DataController.Instance.resttime);
            DataController.Instance.nowdate = todaydate;
            DataController.Instance.nowmonth = todaymonth;
            //DataController.Instance.lastplaytime = nowTime;
            yield return new WaitForSeconds(1f);
        }
    }
}
