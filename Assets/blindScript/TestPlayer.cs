using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;
using BackEnd;
using System.Linq;
using Apple.Accessibility;

public class TestPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    private static TestPlayer instance;

    public static TestPlayer Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<TestPlayer>();
            if (instance != null) return instance;
            var container = new GameObject("TestPlayer");
            return instance;
        }
    }
    
    public PhotonView PV;
    private Animator animator;
    public float speed;
    private Rigidbody2D rb;
    public Transform ts;
    public GameObject playerbox;
    public TextMeshPro tt;
    public BoxCollider2D mycol;
    public int alli; //cols alli.
    public int oi;
    

    public Collider2D[] col, cols, allcol; //obj,player ,obj player.
    public LayerMask layer; //오브젝트 전시물 감지 레이어
    public LayerMask layers; //캐릭터 유저 감지 레이어
    public LayerMask alllayer; //유저 전시물 감지레이어
    public Vector2 size;  // 오브젝트 감지 영역 사이즈


    //public float sizex;
    //public float sizey;


    public SpriteRenderer tssprite;
    public SpriteRenderer changeavataemotionsprite;
    public Transform emotionps;
    public CinemachineVirtualCamera cvc;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
        mycol = GetComponent<BoxCollider2D>();
        speed = 3f; //캐릭터 이동스피드
        changeavata();
        if (PV.IsMine)
        {
            cvc = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            changeplayername();
            //PV.Owner.NickName = DataController.Instance.nickname;
            //tt.text = PV.Owner.NickName;
            tt.text = SmartSecurity.GetString("nickname");
            this.gameObject.layer = 7;
            //tssprite.sortingLayerName = "mylayer";
            changeavataemotionsprite.sortingLayerName = "mylayer";
            checkarea();
            Checkismine.Instance.testplayer = GetComponent<TestPlayer>();
//            TestJoyStick.Instance.testplayer = GetComponent<TestPlayer>();
        }
        else
        {
            this.gameObject.layer = 6;
        }
    }
    private bool isColliding = false; // 기본 충돌 상태
    private bool isColliding1 = false; // 왼쪽아래 충돌 상태
    private bool isColliding2 = false; // 오른쪽 아래 충돌 상태
    private bool isColliding3 = false; // 맨 위 충돌 상태
    private bool isColliding4 = false; // 포탈 위 충돌 상태
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bound"))
        {
            if (PV.IsMine) //벽에있으면 TTS 종료.
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding = true;
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("벽이 있습니다");
                    JoyStickTTS.Instance.audioSource.Stop();
                    //animator.SetBool("Walking", false);
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Boundbotleft")) 
        {
            if (PV.IsMine) //벽에있으면 TTS 종료.
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding1 = true;
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("벽이 있습니다");
                    JoyStickTTS.Instance.audioSource.Stop();
                    //animator.SetBool("Walking", false);
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Boundbotright")) 
        {
            if (PV.IsMine) //벽에있으면 TTS 종료.
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding2 = true;
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("벽이 있습니다");
                    JoyStickTTS.Instance.audioSource.Stop();
                    //animator.SetBool("Walking", false);
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Boundtop")) 
        {
            if (PV.IsMine) //벽에있으면 TTS 종료.
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding3 = true;
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("벽이 있습니다");
                    JoyStickTTS.Instance.audioSource.Stop();
                    //animator.SetBool("Walking", false);
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Boundpotal")) 
        {
            if (PV.IsMine) //벽에있으면 TTS 종료.
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding4 = true;
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("벽이 있습니다");
                    JoyStickTTS.Instance.audioSource.Stop();
                    //animator.SetBool("Walking", false);
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Potal")) //포탈씬이동.
        {
            if (PV.IsMine)
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    SmartSecurity.SetInt("checkuserevent", 1);
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    JoyStickTTS.Instance.audioSource.Stop();
                    TextController.Instance.floorpanel.SetActive(true);
                    if (SmartSecurity.GetInt("photonroomindex") == 1)
                    {
                        testtts.Instance.audioSource.Stop();
                        testtts.Instance.alltts("엘리베이터 입니다.화면 중앙 중심으로 화면 아래 왼쪽 터치시 취소, 오른쪽 터치시 확인 입니다.확인 터치시 2층으로 이동합니다.");
                        TestJoyStick.Instance.joypanel.GetComponent<AccessibilityNode>().IsAccessibilityElement = false;
                    }
                    else if (SmartSecurity.GetInt("photonroomindex") == 2)
                    {
                        testtts.Instance.audioSource.Stop();
                        testtts.Instance.alltts("엘리베이터 입니다.화면 중앙 중심으로 화면 아래 왼쪽 터치시 취소, 오른쪽 터치시 확인 입니다.확인 터치시 1층으로 이동합니다.");
                        TestJoyStick.Instance.joypanel.GetComponent<AccessibilityNode>().IsAccessibilityElement = false;
                    }
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
        else if (other.CompareTag("Object"))  //전시물충돌
        {
            if (PV.IsMine)
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding = true;
                    SmartSecurity.SetInt("checkttsindex", 1);
                    testtts.Instance.audioSource.Stop();
                    testtts.Instance.alltts("전시물이있습니다");
                    JoyStickTTS.Instance.audioSource.Stop();
                    //  Handheld.Vibrate();
                    Invoke("ttscheckon", 0.5f);
                }
            }
        }
    }

    public void ttscheckon()
    {
        SmartSecurity.SetInt("checkttsindex", 0);
        CancelInvoke("ttscheckon");
    }
    private void OnTriggerExit2D(Collider2D collision) //충돌에서 벗어났을때 호출.
    {
        if (collision.CompareTag("Bound"))
        {
            if (PV.IsMine)
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding = false;
                    testtts.Instance.audioSource.Stop();
                }
                //JoyStickTTS.Instance.audioSource.Stop();
            }
            //testtts.Instance.alltts("벽에서 멀어졌습니다.");
        }
        else if (collision.CompareTag("Boundbotleft")) 
        {
            if (PV.IsMine) 
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding1 = false;
                    testtts.Instance.audioSource.Stop();
                }
            }
        }
        else if (collision.CompareTag("Boundbotright")) 
        {
            if (PV.IsMine) 
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding2 = false;
                    testtts.Instance.audioSource.Stop();
                }
            }
        }
        else if (collision.CompareTag("Boundtop")) 
        {
            if (PV.IsMine) 
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding3 = false;
                    testtts.Instance.audioSource.Stop();
                }
            }
        }
        else if (collision.CompareTag("Boundpotal")) 
        {
            if (PV.IsMine) 
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding4 = false;
                    testtts.Instance.audioSource.Stop();
                }
            }
        }
        else if (collision.CompareTag("Object"))  //전시물충돌
        {
            if (PV.IsMine)
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    isColliding = false;
                    testtts.Instance.audioSource.Stop();
                }
            }
        }
    }
    private void Update()
    {
        if (PV.IsMine)
        {
            //playercheck();
            //objcheck();
            checktts();
            //stopobjinfo();
        }
    }


    private float charlastTiltX = 0f, charlastTiltY = 0f;
    private const float charsuddenTiltThreshold = 2.0f; // 크게 흔들렸을 때 감지 임계값
    private bool charsuddenTiltDetected = false;
    private float charsuddenTiltCooldown = 1.5f; // 제한 시간
    private float charlastShakeTime = 0f;
    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            if (SmartSecurity.GetInt("checkloading") == 1)
            {
                if (SmartSecurity.GetInt("loadingindex") == 1)
                {
                    followcamera();
                    if (SmartSecurity.GetInt("checkuserevent") != 0 || charsuddenTiltDetected)
                    {
                        rb.velocity = Vector2.zero;
                        animator.SetBool("Walking", false);

                        // 제한 시간 이후 이동 제한 해제
                        if (Time.time - charlastShakeTime > charsuddenTiltCooldown)
                        {
                            charsuddenTiltDetected = false;
                        }
                    }
                    else
                    {
                        Vector2 tilt = new Vector2(Input.acceleration.x, Input.acceleration.y);
                        if (Mathf.Abs(tilt.x - charlastTiltX) > charsuddenTiltThreshold || Mathf.Abs(tilt.y - charlastTiltY) > charsuddenTiltThreshold)
                        {
                            charsuddenTiltDetected = true;
                            charlastShakeTime = Time.time;
                            charlastTiltX = tilt.x;
                            charlastTiltY = tilt.y;
                            return;
                        }

                        if (tilt.magnitude > 0.5f && !charsuddenTiltDetected && tilt.magnitude < 1f) // 작은 움직임 무시
                        {
                            animator.SetFloat("DirX", tilt.x);
                            animator.SetFloat("DirY", tilt.y);
                            animator.SetBool("Walking", true);

                            // 기울기에 따른 속도 설정
                            rb.velocity = tilt * speed;
                            checkwalkingindex = 0;
                            charclockposition(tilt);

                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                            animator.SetBool("Walking", false);
                        }

                    }

                    if (animator.GetBool("Walking") == true)
                    {
                        if (isColliding || isColliding1 || isColliding2 || isColliding3 || isColliding4)
                        {
                            FootStep.Instance.foot.Stop();
                        }
                        else
                        {
                            if (!FootStep.Instance.foot.isPlaying)
                            {
                                FootStep.Instance.foot.Play();
                            }
                        }
                    }
                    else
                    {
                        FootStep.Instance.foot.Stop();
                    }
                }
                // 자이로센서 기울기 감지

            }
            else
            {

            }
        }
    }
    private int lastDirection = -1; // 최근 호출된 방향 (-1은 초기값)
    private float lastTiltX = 0f, lastTiltY = 0f; // 이전 프레임의 기울기 값
    private const float suddenTiltThreshold = 1.0f; // 갑작스러운 기울기 변화를 감지하는 임계값
    private bool suddenTiltDetected = false;

    public void charclockposition(Vector2 pos)
    {
        if (Mathf.Abs(pos.x - lastTiltX) > suddenTiltThreshold || Mathf.Abs(pos.y - lastTiltY) > suddenTiltThreshold)
        {
            // 큰 변화가 감지되면 호출하지 않고 종료
            suddenTiltDetected = true;
            lastTiltX = pos.x;
            lastTiltY = pos.y;
            return;
        }

        if(SmartSecurity.GetInt("checkgworldtts")==0)
        {
            SmartSecurity.SetInt("checkgworldtts", 1);
            testtts.Instance.audioSource.Stop();
            AccessibilityNotification.PostAnnouncementNotification("");

            //AccessibilityNotification.PostLayoutChangedNotification();
        }

        suddenTiltDetected = false;
        lastTiltX = pos.x;
        lastTiltY = pos.y;

        joyvecpositionx(pos);
        joyvecpositiony(pos);

        if (SmartSecurity.GetInt("checkuserevent") == 0 && !suddenTiltDetected)
        {
            if (SmartSecurity.GetInt("mathfjoyveindex") == 0)
            {
                if (mathfjoyvecx == 1 && mathfjoyvecy == 0) // 3시 방향
                {
                    SmartSecurity.SetInt("mathfjoyveindex", 1);
                    JoyStickTTS.Instance.testttsTt1("3시 방향");
                   // SmartSecurity.SetInt("ttsindex1", 0);
                    SmartSecurity.SetInt("ttsindex3", 0);
                    SmartSecurity.SetInt("ttsindex5", 0);
                    SmartSecurity.SetInt("ttsindex7", 0);

                }
                else if (mathfjoyvecx == 0 && mathfjoyvecy == -1) // 6시 방향 
                {
                    SmartSecurity.SetInt("mathfjoyveindex", 2);
                    JoyStickTTS.Instance.testttsTt3("6시 방향");
                    SmartSecurity.SetInt("ttsindex1", 0);
                    //SmartSecurity.SetInt("ttsindex3", 0);
                    SmartSecurity.SetInt("ttsindex5", 0);
                    SmartSecurity.SetInt("ttsindex7", 0);
                }
                else if (mathfjoyvecx == -1 && mathfjoyvecy == 0) // 9시 방향
                {
                    SmartSecurity.SetInt("mathfjoyveindex", 3);
                    JoyStickTTS.Instance.testttsTt5("9시 방향");
                    SmartSecurity.SetInt("ttsindex1", 0);
                    SmartSecurity.SetInt("ttsindex3", 0);
                   // SmartSecurity.SetInt("ttsindex5", 0);
                    SmartSecurity.SetInt("ttsindex7", 0);
                }
                else if (mathfjoyvecx == 0 && mathfjoyvecy == 1) // 12시 방향
                {
                    SmartSecurity.SetInt("mathfjoyveindex", 4);
                    JoyStickTTS.Instance.testttsTt7("12시 방향");
                    SmartSecurity.SetInt("ttsindex1", 0);
                    SmartSecurity.SetInt("ttsindex3", 0);
                    SmartSecurity.SetInt("ttsindex5", 0);
                    //SmartSecurity.SetInt("ttsindex7", 0);
                }
                else if (mathfjoyvecx == 0 && mathfjoyvecy == 0)
                {
                    // 기본 위치에서 아무것도 하지 않음
                }

                SmartSecurity.SetInt("mathfjoyvecxindex", mathfjoyvecx);
                SmartSecurity.SetInt("mathfjoyvecyindex", mathfjoyvecy);
            }
            else
            {
                if (SmartSecurity.GetInt("mathfjoyvecxindex") != mathfjoyvecx || SmartSecurity.GetInt("mathfjoyvecyindex") != mathfjoyvecy)
                {
                    SmartSecurity.SetInt("mathfjoyveindex", 0);
                }
            }
        }
    }

  
    public int mathfjoyvecx;
    public int mathfjoyvecy;

    public void joyvecpositionx(Vector2 pos)
    {
        mathfjoyvecx = pos.x > -0.5 ? Mathf.RoundToInt(pos.x) : Mathf.FloorToInt(pos.x);
    }

    public void joyvecpositiony(Vector2 pos)
    {
         mathfjoyvecy = pos.y > -0.5 ? Mathf.RoundToInt(pos.y) : Mathf.FloorToInt(pos.y);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ts.position); //포지션 지연보상
        }
        else
        {
            ts.position = (Vector3)stream.ReceiveNext(); 
        }
    }


    public void followcamera() //시네머신카메라 find 1회 호출.
    {
        // var cm = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
        //cm.Follow = transform;
        //cm.LookAt = transform;
        cvc.Follow = transform;
        cvc.LookAt = transform;
    }

    void OnDrawGizmos() //overlapbox gizmos
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
    public void checkarea()
    {
        if (SmartSecurity.GetInt("playerareaindex") == 2)
        {
            size = new Vector2(12f, 12f);
        }
    }


    [PunRPC]
    public void changeplayernamerpc(string nickname) //nicknameRPC.
    {
        tt.text = nickname;
    }
    public void changeplayername() //nicknameRPC 호출.
    {
        string nickname = SmartSecurity.GetString("nickname");
        PV.RPC("changeplayernamerpc", RpcTarget.Others,nickname);
    }
    [PunRPC] 
    public void changeplayerimagerpc(int index) //감정 RPC
    {
        changeavataemotionsprite.sprite = TextController.Instance.avataemotionsprite[index];
    }
    public void changewsprite() //여성.
    {
        int index = SmartSecurity.GetInt("witememotion");
        PV.RPC("changeplayerimagerpc", RpcTarget.Others, index);
    }
    public void changesprite() //남성.
    {
        int index = SmartSecurity.GetInt("itememotion");
        PV.RPC("changeplayerimagerpc", RpcTarget.Others, index);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) //플레이어 방입장시 동기화
    {
        if(PV.IsMine)
        {
            changeplayername();
            changeavata();
        }
    }
    public void changeavata() //감정.
    {
        if (PV.IsMine)
        {
            if (SmartSecurity.GetInt("wmsureindex") == 0) //여성
            {
                changeavataemotionsprite.sprite = TextController.Instance.avataemotionsprite[SmartSecurity.GetInt("witememotion")];
                changewsprite();
            }
            else if ((SmartSecurity.GetInt("wmsureindex") == 1)) //남성
            {
                changeavataemotionsprite.sprite = TextController.Instance.avataemotionsprite[SmartSecurity.GetInt("itememotion")];
                changesprite();
            }
        }
    }
    public Vector3 b;
    int audiocheck; 
    public void playercheck()
    {
        cols = Physics2D.OverlapBoxAll(ts.transform.position, size, 0, layers);
        if (cols.Length == 0 && col.Length == 0)
        {
            //TextController.Instance.tapbtn.SetActive(false);
            audiocheck = 0;
        }
        else
        {
            if (audiocheck == 0)
            {
                testtts.Instance.audioSource.PlayOneShot(testtts.Instance.eventclip);
                audiocheck = 1;
            }
           // TextController.Instance.tapbtn.SetActive(true);
        }
        for (alli = 0; alli < cols.Length; alli++)
        {
            if (alli >= 0)
            {
                DataController.Instance.eventtouchindex = 1;
                SmartSecurity.SetInt("eventtouchindex", 1);
                //캐릭터가 다수일때. 
                if (cols[alli].CompareTag("Player"))
                {
                    //b = cols[alli].GetComponent<objplayer>().ts.transform.position;
                    b = cols[alli].GetComponent<TestPlayer>().ts.transform.position;//다른 플레이어 캐릭터.
                }
            }
        }
    }
   
    List<Collider2D> detectedObjects = new List<Collider2D>();
    public void checktts() 
    {
        allcol = Physics2D.OverlapBoxAll(ts.transform.position, size, 0, alllayer);
        
        // 이전 프레임에 감지된 오브젝트와 비교하여 새로 감지된 오브젝트를 체크
        foreach (Collider2D col in allcol)
        {
            if (SmartSecurity.GetInt("checkloading") == 1)
            {
                if (!detectedObjects.Contains(col))
                {
                    // 새로운 오브젝트를 감지함
                    detectedObjects.Add(col);
                    //Debug.Log("새로운 오브젝트를 감지했습니다.");
                    if (detectedObjects.Count > 0)
                    {
                        SmartSecurity.SetInt("eventtouchindex", 1);
                        TextController.Instance.tapbtn.SetActive(true);
                        if (SmartSecurity.GetInt("startblindindex")==1)
                        {
                            testtts.Instance.audioSource.PlayOneShot(testtts.Instance.eventclip);

                        }
                    }
                }
            }
        }

        // 이전 프레임에 있었지만 현재는 감지되지 않은 오브젝트를 체크하여 리스트에서 제거
        foreach (Collider2D col in detectedObjects.ToList())
        {
            if (!allcol.Contains(col))
            {
                // 이전에 감지된 오브젝트가 현재는 감지되지 않음
                detectedObjects.Remove(col);
                if (detectedObjects.Count == 0)
                {
                    SmartSecurity.SetInt("eventtouchindex", 0);
                    TextController.Instance.tapbtn.SetActive(false);
                }
                //Debug.Log("오브젝트가 사라졌습니다.");
            }
        }

        //캐릭터 이동이 멈추면 감지 EventClip STOP 후 EventClip Play
        if (animator.GetBool("Walking")== false)
        {
            if(detectedObjects.Count >0)
            {
                if(checkwalkingindex == 0)
                {
                    if (SmartSecurity.GetInt("startblindindex") == 1)
                    {
                        //testtts.Instance.audioSource.Stop();
                        //testtts.Instance.audioSource.PlayOneShot(testtts.Instance.eventclip);
                    }
                    checkwalkingindex = 1;
                }
            }
        }
    }
    int checkwalkingindex;
    public void stopeventclip()
    {
        testtts.Instance.audioSource.PlayOneShot(testtts.Instance.eventclip);
    }

}


