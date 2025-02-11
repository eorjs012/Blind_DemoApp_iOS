using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Batterystatus : MonoBehaviour
{
    public Slider batslider;
    public Text batTt;
    private void Start()
    {
        /* float number = SystemInfo.batteryLevel; // 베터리 충전량 가져오기 (0 ~ 1);
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
     }
        */
    }
}
