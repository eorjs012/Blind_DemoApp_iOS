using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using BackEnd;
using LitJson;
using UnityEngine.SceneManagement;
using System.Net;
using Random = UnityEngine.Random;
//using System.IO.Ports;

public class PhotonController : MonoBehaviourPunCallbacks , IOnPhotonViewPreNetDestroy//, IChatClientListener ,IPunObservable
{
    private static PhotonController instance;

    public static PhotonController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<PhotonController>();
            if (instance != null) return instance;
            var container = new GameObject("PhotonController");
            return instance;
        }
    }

    //public PlayerLeaderboardEntry myplayfabinfo;
    // public GameObject loginpanel;
    PhotonView photonview;
    public InputField chatinput;
    public GameObject chatTtPrf;
    public RectTransform chatcontent;
    public RectTransform chatview;
    public ScrollRect chatscroll;
    public Scrollbar chatscrollbar;
    public GameObject quitpanel;
    TouchScreenKeyboard keyboard;
    public GameObject boxprf; //추후 지우기
    ChatClient chatclient;
    void Start()
    {
        Screen.SetResolution(1620, 2160, true);
        Application.targetFrameRate = 30;
        Camera.main.aspect = 4f / 4f;
        PhotonNetwork.ConnectUsingSettings(); //포톤서버접속.
        PhotonNetwork.SendRate = 30; //60
        PhotonNetwork.SerializationRate = 15;//30
        //PhotonNetwork.AutomaticallySyncScene = true; //
        Application.runInBackground = true; //실수로 앱을 나갔다와도 실행이 유지됨(끊김현상없음에디터한정).
        photonview = GetComponent<PhotonView>();
        chatscroll = GetComponent<ScrollRect>();
        chatscrollbar = GetComponent<Scrollbar>();
        SmartSecurity.SetInt("loadingindex", 0);
        rodingpanel.SetActive(true);
       
    }
    public void Update()
    {
        if (photonview.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Return) || keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                if (chatinput.text.Replace(" ", "").Equals(""))
                {
                    chatinput.text = "";
                    return;
                }
                else
                {
                    string chatTt = PhotonNetwork.NickName + " : " + chatinput.text;
                    photonview.RPC("chatposition", RpcTarget.All, chatTt);
                    TextController.Instance.chatbtn();
                    // chatinput.ActivateInputField();
                    chatinput.text = "";
                }
            }
        }
    }

    //네트워크 삭제되기 직적의콜백
    void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
    {
        Debug.Log("네트워크 삭제되기 직적의콜백");
    }

    public void twofloorselecttruebtn()
    {
        //SmartSecurity.SetInt("checkgworldtts", 0);
        TextController.Instance.floorpanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        SmartSecurity.SetInt("photonroomindex", 2);
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }
    public void onefloorselecttruebtn()
    {
        //SmartSecurity.SetInt("checkgworldtts", 0);
        TextController.Instance.floorpanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        SmartSecurity.SetInt("photonroomindex", 1);
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }


    public void openkeyborad()
    {
        keyboard = TouchScreenKeyboard.Open(chatinput.text, TouchScreenKeyboardType.Default); //모바일키패드 
    }
    public override void OnConnectedToMaster() 
    {
        if (SmartSecurity.GetInt("photonroomindex") == 1) 
        {
            PhotonNetwork.JoinOrCreateRoom("floor1", new RoomOptions { MaxPlayers = 20 },null);
        }
        else if (SmartSecurity.GetInt("photonroomindex") == 2)
        {
            PhotonNetwork.JoinOrCreateRoom("floor2", new RoomOptions { MaxPlayers = 20 }, null);
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message) //방입장 실패
    {
        this.createroom(); 
    }
    public void createroom()
    {
        //3개의 층 방생성 , index로 체크.
        // RoomOptions rommioptions = new RoomOptions();
        // rommioptions.IsOpen = true; //방이 열려있는지 닫혀있는지 설정.
        // rommioptions.IsVisible = true; //비공개 방 여부
       // PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
    }
    public GameObject disconnectpanel;
    public override void OnDisconnected(DisconnectCause cause) //서버연결끊김.
    {
        Debug.LogWarningFormat("연결 끊김 : {0}", cause);
        // PhotonNetwork.RejoinRoom("방이름"); //방에 재접속
        // disconnectpanel.SetActive(true);
        // SceneManager.LoadScene(1, LoadSceneMode.Single); //로드씬으로 이동
        Redisconnect();
    }
    public void Redisconnect()
    { //재접속 시도 방떠나기 메인씬으로 이동
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void disconnectpanelsurebtn()
    {   //서버에 접속중인 캐릭터 destroy
        PhotonNetwork.Disconnect();
        disconnectpanel.SetActive(false);
    }
    public void testondisconnected()
    {
        disconnectpanel.SetActive(true);
    }
    public void sendchat() //pv.ismine 일때 닉네임 색상 구분.
    {
        if(photonview.IsMine)
        {
            if (chatinput.text.Replace(" ", "").Equals(""))
            {
                chatinput.text = "";
                return;
            }
            else
            {
                //nickcolor = new Color(0, 0, 0, 0);
                //string colorchatTt = "<color =#" + PhotonNetwork.NickName + ">" + chatinput.text;
                string chatTt = PhotonNetwork.NickName + " : " + chatinput.text;
                photonview.RPC("chatposition", RpcTarget.All, chatTt);
                TextController.Instance.chatbtn();
            }
        }
    }
    [PunRPC]
    public void chatposition(string chatTt)
    {
        GameObject chatprf = Instantiate(chatTtPrf);
        chatprf.GetComponent<Text>().text = chatTt;
        StartCoroutine(addchat(chatprf));
        chatinput.text = "";

    }

    IEnumerator addchat(GameObject chatprf)
    {
        yield return new WaitForSeconds(0.1f);
        chatprf.transform.SetParent(chatcontent);

        yield return new WaitForSeconds(0.1f);
        if (chatcontent.sizeDelta.y > chatview.sizeDelta.y) //scroll view size up 
        {
            chatcontent.anchoredPosition = new Vector2(0, chatcontent.sizeDelta.y - chatview.sizeDelta.y);
        }
    }
  
    public GameObject rodingpanel;

    public override void OnJoinedRoom() //캐릭터 애니메이터 프레임(지속시간 딜레이) 0.1로 설정.
    { //point vector3(430,762,-11)
       //서버접속만하면 바로되서 오류 캐릭터 생성됨
       if (SmartSecurity.GetInt("wmsureindex") == 0) //여성캐릭터.
       {
            if (SmartSecurity.GetInt("witemtop") ==0)
            {
                if(SmartSecurity.GetInt("witemface") ==0)
                {
                    PhotonNetwork.Instantiate("W0201", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0401", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0701", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1001", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1101", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1201", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2101", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2401", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2801", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3001", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 1)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0202", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0402", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0702", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1002", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1102", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1202", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2102", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2402", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2802", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3002", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 2)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0203", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0403", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0703", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1003", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1103", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1203", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2103", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2403", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2803", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3003", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 3)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0204", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0404", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0704", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1004", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1104", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1204", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2104", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2404", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2804", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3004", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 4)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0205", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0405", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0705", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1005", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1105", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1205", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2105", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2405", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2805", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3005", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 5)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0206", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0406", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0706", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1006", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1106", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1206", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2106", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2406", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2806", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3006", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 6)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0207", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0407", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0707", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1007", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1107", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1207", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2107", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2407", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2807", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3007", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 7)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0208", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0408", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0708", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1008", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1108", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1208", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2108", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2408", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2808", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3008", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 8)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0209", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0409", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0709", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1009", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1109", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1209", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2109", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2409", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2809", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3009", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
            else if (SmartSecurity.GetInt("witemtop") == 9)
            {
                if (SmartSecurity.GetInt("witemface") == 0)
                {
                    PhotonNetwork.Instantiate("W0210", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 1)
                {
                    PhotonNetwork.Instantiate("W0410", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 2)
                {
                    PhotonNetwork.Instantiate("W0710", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 3)
                {
                    PhotonNetwork.Instantiate("W1010", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 4)
                {
                    PhotonNetwork.Instantiate("W1110", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 5)
                {
                    PhotonNetwork.Instantiate("W1210", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 6)
                {
                    PhotonNetwork.Instantiate("W2110", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 7)
                {
                    PhotonNetwork.Instantiate("W2410", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 8)
                {
                    PhotonNetwork.Instantiate("W2810", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
                else if (SmartSecurity.GetInt("witemface") == 9)
                {
                    PhotonNetwork.Instantiate("W3010", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                }
            }
        }
       else if(SmartSecurity.GetInt("wmsureindex") == 1) //남성캐릭터.
       {
            if (SmartSecurity.GetInt("itemtop") == 0)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0301", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 1);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0601", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 2);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1101", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 3);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1801", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 4);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1901", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 5);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2201", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 6);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2401", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 7);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2501", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 8);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2801", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 9);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3001", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 10);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 1)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0302", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 11);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0602", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 12);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1102", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 13);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1802", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 14);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1902", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 15);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2202", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 16);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2402", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 17);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2502", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 18);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2802", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 19);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3002", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 20);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 2)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0303", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 21);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0603", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 22);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1103", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 23);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1803", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 24);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1903", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 25);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2203", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 26);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2403", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 27);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2503", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 28);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2803", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 29);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3003", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 30);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 3)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0304", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 31);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0604", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 32);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1104", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 33);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1804", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 34);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1904", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 35);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2204", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 36);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2404", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 37);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2504", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 38);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2804", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 39);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3004", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 40);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 4)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0305", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 41);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0605", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 42);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1105", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 43);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1805", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 44);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1905", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 45);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2205", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 46);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2405", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 47);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2505", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 48);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2805", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 49);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3005", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 50);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 5)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0306", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 51);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0606", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 52);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1106", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 53);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1806", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 54);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1906", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 55);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2206", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 56);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2406", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 57);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2506", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 58);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2806", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 59);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3006", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 60);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 6)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0307", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 61);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0607", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 62);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1107", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 63);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1807", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 64);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1907", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 65);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2207", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 66);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2407", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 67);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2507", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 68);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2807", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 69);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3007", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 70);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 7)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0308", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 71);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0608", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 72);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1108", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 73);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1808", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 74);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1908", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 75);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2208", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 76);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2408", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 77);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2508", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 78);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2808", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 79);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3008", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 80);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 8)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0309", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 81);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0609", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 82);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1109", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 83);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1809", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 84);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1909", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 85);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2209", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 86);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2409", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 87);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2509", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 88);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2809", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 89);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3009", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 90);
                }
            }
            else if (SmartSecurity.GetInt("itemtop") == 9)
            {
                if (SmartSecurity.GetInt("itemface") == 0)
                {
                    PhotonNetwork.Instantiate("M0310", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 91);
                }
                else if (SmartSecurity.GetInt("itemface") == 1)
                {
                    PhotonNetwork.Instantiate("M0610", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 92);
                }
                else if (SmartSecurity.GetInt("itemface") == 2)
                {
                    PhotonNetwork.Instantiate("M1110", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 93);
                }
                else if (SmartSecurity.GetInt("itemface") == 3)
                {
                    PhotonNetwork.Instantiate("M1810", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 94);
                }
                else if (SmartSecurity.GetInt("itemface") == 4)
                {
                    PhotonNetwork.Instantiate("M1910", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 95);
                }
                else if (SmartSecurity.GetInt("itemface") == 5)
                {
                    PhotonNetwork.Instantiate("M2210", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 96);
                }
                else if (SmartSecurity.GetInt("itemface") == 6)
                {
                    PhotonNetwork.Instantiate("M2410", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 97);
                }
                else if (SmartSecurity.GetInt("itemface") == 7)
                {
                    PhotonNetwork.Instantiate("M2510", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 98);
                }
                else if (SmartSecurity.GetInt("itemface") == 8)
                {
                    PhotonNetwork.Instantiate("M2810", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 99);
                }
                else if (SmartSecurity.GetInt("itemface") == 9)
                {
                    PhotonNetwork.Instantiate("M3010", new Vector3(Random.Range(425.26f, 435f), Random.Range(763f, 765.42f), -11), Quaternion.identity);
                    SmartSecurity.SetInt("dotmcharimageindex", 100);
                }
            }
        }

        SmartSecurity.SetInt("loadingindex", 1);

    }

    public void colliderprf()
    {
        DataController.Instance.btnindex = 1;
        //boxprf = Resources.Load<GameObject>("PhotonUnityNetworking/Resources/boxprf");
        // Instantiate(boxprf, new Vector3(450, 800, 1), Quaternion.identity);
    }
   
    public void quityes()
    {
        //서버접속종료
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
    public void quitno()
    {
        quitpanel.SetActive(false);
        //종료패널끄기
    }
}

