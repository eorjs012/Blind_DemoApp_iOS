using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadQuit : MonoBehaviour
{
    public GameObject quit;
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("앱을 종료하시겠습니까 앱종료안내 화면이 켜졌습니다");
                }
                quit.SetActive(true);
                //종료패널켜기
            }
        }
    }
}
