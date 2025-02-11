using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Apple.Accessibility;

public class DotPadManager : MonoBehaviour
{
    public void Start()
    {
        checkconnect = 0;
        checkbletoggle.isOn = false;
        checkstatebletoggle();
        if (SmartSecurity.GetInt("bleindex") == 0)
        {
            bleonimage.SetActive(false);
            bleoffimage.SetActive(true);
        }
        botsettingpaenl.SetActive(true);
        blepanel.SetActive(false);
    }

    [DllImport("__Internal")]
    private static extern void _connectToDotPad(int index);
    
    [DllImport("__Internal")]
    private static extern void _disconnectFromDotPad();
    
    [DllImport("__Internal")]
    private static extern void _displayGraphicData(string data);
    
    [DllImport("__Internal")]
    private static extern void _displayTextData(string text);
    
    [DllImport("__Internal")]
    private static extern void _scanForDotPad();

    [DllImport("__Internal")]
    private static extern void _stopScanForDotPad();

    [DllImport("__Internal")]
    private static extern void _dotpadinit();

    [DllImport("__Internal")]
    private static extern void _sendImageToSwift(string base64Image);

    [DllImport("__Internal")]
    private static extern void _checkBluetoothAndOpenSettingsIfDisabled();
    

    public Text[] deviceTexts; // 최대 3개 디바이스 이름 표시용 UI Text 배열

    public GameObject[] bleselectobj = new GameObject[3]; //ble 연결버튼 총 3개

 

    public void hometts() //home 설명 tts 
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                RoadTTS.Instance.audioSource.Stop();
                //RoadTTS.Instance.alltts("메인화면 입니다. 화면 하단에 좌에서 우로 홈버튼, 아바타꾸미기, 메타월드, 앱세팅 등 4개의 메뉴버튼이 있습니다.각 메뉴 버튼을 터치하여 해당 메뉴로 이동할 수 있습니다");
              
            }
        }
    }

    //연결된 디바이스 정보확인 ( 300 , 320 )
    public void ConnectDotInfo(string message)
    {
        print(message);

        SmartSecurity.SetInt("dotimageindex", int.Parse(message));
    }

    public void image1()
    {
        string st = "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000884462aa262244880000000000004822222222224208000000000000801642882411420848126108000000f0aaaaaaaaaaaaaafa000000000080478824114208481242084872080000f000000000000000f00000000000f024114288ae1991ea481242080f0000f000000000000000f000000000c832111111f9998998999f111111630c00f000000000000000f00000000043442622110111222211103162444407007044444444444444740000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        _displayGraphicData(st);
    }
    public void image2()
    {
        string st = "000000000000000000004cc4c4cccccccc4c4cc400000000000000000000000000000000000000000ff0f0ffffffff0f0ff000000000000000000000000000000000000000000ff0f0ffffffff0f0ff0000000000000000000000000000000000000000021f4f0ffffffff0f4f120000000000000000000000000000000000000000000031ff7777ff130000000000000000000000000000000000000000000000004835aaaa53840000000000000000000000000000000000000000000080162cf244442fc261080000000000000000000000000000000000000000f0002d72888827d2000f00000000000000000000000000000000000000000043297288882792340000000000000000000000000000000000000000000000102122221201000000000000000000000000";
        _displayGraphicData(st);
    }
    public void image3()
    {
        string st = "0000000000000000000000888888888888888800000000000000000000000000000000c04444444444f5545d5555d5455f44444444440c0000000000000000000000f1b3b3bbb3f31111f11f11113f3bbb3b3b1f000000000000000000000000f0f0f0f0f0f0551d5dd5d1550f0f0f0f0f0f000000000000000000000000f0e0f0f0e0f0000f0ff0f0000f0e0f0f0e0f000000000000000000000000f0b8b8bab8f8aa8babbab8aa8f8bab8b8b0f000000000000000000000000f04c74c44447c4447447444c74444c47c40f000000000000000000000000e0232232222232222222222322222322320e0000000000000000f3dddddddddddddddddddd13111131dddddddddddddddddddd7d0000000033222222222222222222222322223222222222222222222222330000";
        _displayGraphicData(st);
    }

    public void ReceiveDeviceList(string message)
    {
        Debug.Log(message);  // Unity 콘솔에 메시지 출력
        
        string[] parts = message.Split(',');
        //모든 버튼 비활성화
        foreach (var button in bleselectobj)
        {
            button.SetActive(false);
        }

        if (string.IsNullOrEmpty(message))
        {
            //Debug.Log("Device list is empty. All buttons are disabled.");
            return;
        }
        Dictionary<int, string> devicelist = new Dictionary<int, string>();

        int activecount = 0;

        for(int i = 0; i<parts.Length - 1 && activecount < 3; i+= 2)
        {
            if(int.TryParse(parts[i],out int index))
            {
                string localname = parts[i + 1];
                devicelist[index] = localname;

                if(index<bleselectobj.Length)
                {
                    nonconnectTtobj[index].SetActive(true);
                    connectTtobj[index].SetActive(false);
                    bleselectobj[index].SetActive(true);
                    deviceTexts[index].text = localname;
                    activecount++;
                }
            }
        }

      
    }


    // Unity에서 Swift로 메시지 보내기

    public GameObject[] nonconnectTtobj = new GameObject[3];
    public GameObject[] connectTtobj = new GameObject[3];

    //닷패드 연결
    public void ConnectToDotPad(int index) {
        if (SmartSecurity.GetInt("bleconnectindex") == 0)
        {
            _connectToDotPad(index);
            nonconnectTtobj[index].SetActive(false);
            connectTtobj[index].SetActive(true);
            SmartSecurity.SetInt("bleconnectindex", 1);
            SmartSecurity.SetInt("checkbleobjindex" + index, 1);
            SmartSecurity.SetString("checkbleobjname" + index, deviceTexts[index].text);
            SmartSecurity.SetString("checkbleobjname", deviceTexts[index].text);
        }
    }

    string initdotimage = "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

    //닷패드 연결끊기
    public void DisconnectFromDotPad() {
        if (SmartSecurity.GetInt("bleconnectindex") == 1)
        {
            DisplayGraphicData(initdotimage);
            Invoke("dotdisconnecton", 1.5f);
        }
    }

    public void dotdisconnecton()
    {
        _disconnectFromDotPad();
        SmartSecurity.SetInt("dotimageindex", 0);
        SmartSecurity.SetInt("bleconnectindex", 0);
        SmartSecurity.SetInt("checkbleobjindex0", 0);
        SmartSecurity.SetInt("checkbleobjindex1", 0);
        SmartSecurity.SetInt("checkbleobjindex2", 0);
        SmartSecurity.SetString("checkbleobjname", "");
        SmartSecurity.SetString("checkbleobjname0", "");
        SmartSecurity.SetString("checkbleobjname1", "");
        SmartSecurity.SetString("checkbleobjname2", "");
    }
     
    //닷패드 이미지 드로잉 
    public void DisplayGraphicData(string data) {
        _displayGraphicData(data);
    }

    //닷패드 텍스트 드로잉
    public void DisplayTextData(string text) {
        _displayTextData(text);
    }


    //닷패드 블루투스 스캔
    public void ScanForDotPad() {
        _scanForDotPad();
    }

    public void StopScanForDotPad()
    {
        _stopScanForDotPad();
    }
 
    //닷패드 재설정
    public void DotPadInit()
    {
        _dotpadinit();
    }

    public void LoadImageAndSendToSwift()
    {
        string str = "000000000000000000000000000000000000000000000000000000000000000000000000008888888888882446888888888888000000000000000000000000000000f0a622222244440ce84644222222e10f0000000000000000000000000000f0f088888888f70fa10488888888780f000000000000000000000000000010212222e2fff0100100e1a622221200000000000000000000000000000000000080e7310c0f000030fc000000000000000000000000000000000000000000f0f000f10f000000690f0000000000000000000000000000000000000000102133233333333312010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        _displayGraphicData(str);
      
    }
    public void testload()
    {
        string str1 = "0000000000000000000000000000000000000000000000000000000000000000000000CCEE0E00000000000000000000000000000000000000000000000000000010FF0F00000000000000000000000000000000880800808800000000888888FF0F0000000000000000C0CC0000000000003303003033000080FE7F33F7FF0F00C0FE7FF7EF0C00F7FF77770000000088080080880000F0FF000000FF0F00FF1F0000F1FF00F0FF00000000000033030030330000F0FF8C00C8FF0F00F7CF0880FC7F00F0FF080000000000880800808800000073777777777707103377773301003077777700000000330300303300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        _displayGraphicData(str1);
    }

    public Toggle checkbletoggle;
    public GameObject blepanel;
    public GameObject blesettingpanel;
    public GameObject bleconnectpanel;

    public GameObject bleonimage;
    public GameObject bleoffimage;
    public GameObject botsettingpaenl;

    public void blepanelopenbtn()
    {
        bleselectobj[0].SetActive(false);
        bleselectobj[1].SetActive(false);
        bleselectobj[2].SetActive(false);
        nonconnectTtobj[0].SetActive(true);
        connectTtobj[0].SetActive(false);
        nonconnectTtobj[1].SetActive(true);
        connectTtobj[1].SetActive(false);
        nonconnectTtobj[2].SetActive(true);
        connectTtobj[2].SetActive(false);
        botsettingpaenl.SetActive(false);
        blepanel.SetActive(true);
        blesettingpanel.SetActive(true);
        bleconnectpanel.SetActive(false);
    }

    public void bleclosebtn()
    {
        botsettingpaenl.SetActive(true);
    }

    public void CheckBluetoothAndOpenSettingsIfDisabled()
    {
        if (checkbletoggle.isOn == true)
        {
            if (SmartSecurity.GetInt("bleconnectindex") == 0)
            {
                _checkBluetoothAndOpenSettingsIfDisabled();
            }
            SmartSecurity.SetInt("bleindex", 1);
        }
        else
        {
            SmartSecurity.SetInt("bleindex", 0);
        }
    }
    public void checkstatebletoggle()
    {
        if(SmartSecurity.GetInt("bleindex")== 1)
        {
            checkbletoggle.isOn = true;
        }
        else
        {
            checkbletoggle.isOn = false;
        }
    }

    int checkconnect; // 블루투스 연결버튼 닷패드 스캔 체크 인덱스

    public void bleconnectbtn()   //블루투스패널 연결버튼
    {
        
        if (SmartSecurity.GetInt("bleconnectindex") == 0)
        {
            if (SmartSecurity.GetInt("bleindex") == 1)
            {
               
                if (checkconnect == 0)
                {
                    _dotpadinit();
                    //_scanForDotPad();
                    checkconnect++;
                    //ReceiveDeviceList(Tt.text);
                }
                else if (checkconnect > 0)
                {
                    _scanForDotPad();
                    //ReceiveDeviceList(Tt.text);
                }
            }
        }
        else
        {
            checkselectobj();
        }
       
        blesettingpanel.SetActive(false);
        bleconnectpanel.SetActive(true);
    }
    public void checkobjstate()
    {

    }
    public void checkselectobj()
    {
        bleselectobj[0].SetActive(true);
        deviceTexts[0].text = SmartSecurity.GetString("checkbleobjname");
        nonconnectTtobj[0].SetActive(false);
        connectTtobj[0].SetActive(true);
    }
    string[] bleinfostring = new string[3];
    public void checkbleinfo()
    {
        for(int i = 0; i<0; i++)
        {
            if(i > 3)
            {
                break;
            }
            else
            {
                ReceiveDeviceList(bleinfostring[i]);
                bleselectobj[i].SetActive(true);
            }
        }

    }

    public void bledisconnectbtn()
    {
        if (SmartSecurity.GetInt("bleindex") == 1)
        {
            checkbletoggle.isOn = false;
            SmartSecurity.SetInt("bleindex", 0);
            SmartSecurity.SetInt("checkbleobjindex0", 0);
            SmartSecurity.SetInt("checkbleobjindex1", 0);
            SmartSecurity.SetInt("checkbleobjindex2", 0);

        }
    }
    
}

