using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Apple.Accessibility;

public class TestJoyStick : MonoBehaviour,IPointerEnterHandler //, IDragHandler //, IBeginDragHandler, IDragHandler, IDropHandler   //, IEndDragHandler, IPointerDownHandler//,IPointerUpHandler//,IPointerExitHandler//,IPointerUpHandler//, IPointerEnterHandler//, IPointerClickHandler , IDragHandler//,IBeginDragHandler, IEndDragHandler
{
    private static TestJoyStick instance;

    public static TestJoyStick Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<TestJoyStick>();
            if (instance != null) return instance;
            var container = new GameObject("TestJoyStick");
            return instance;
        }
    }

    public GameObject joypanel;
     public GameObject smallstick;
    public GameObject bgstick;
    //public TestPlayer testplayer; //PV.Ismine
    public Vector3 stickfirstposition;
    public Vector3 joyvec;
    public Vector3 joystickfirstposition;
    public float stickradius;

  

    void Start()
    {
        DataController.Instance.touchjoy = 0;
        stickradius = rectbgstick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joystickfirstposition = rectbgstick.transform.position;
        stickradius = bgstick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joystickfirstposition = bgstick.transform.position;
        checkInterval = 0.2f;
        checkjoystick = 0;
        var Element = joypanel.GetComponent<AccessibilityNode>();
        Element.IsAccessibilityElement = true;
    }

    public void floorselectclosebtn() //엘리베이터 선택팝업창 취소버튼
    {
        joypanel.GetComponent<AccessibilityNode>().IsAccessibilityElement = true;
    }


    public int ad = 0;
    public float interval = 0.5f;  //0.5f 이상으로 
    public float doubleclickedtime = 0f;
    public bool isdoubleclicked = false;
    public void OnPointerEnter(PointerEventData eData) // 체크 조이스틱 더블탭 감지 
    {
        if (Input.touchCount == 1)
        {
            ad = 1;
        }

        if (AccessibilitySettings.IsVoiceOverRunning == true)
        {
            var magictap = joypanel.GetComponent<AccessibilityNode>();
            magictap.onAccessibilityPerformMagicTap = eventmagictap;
        }
        else
        {
            if (Input.touchCount == 2) //더블탭 체크
            {
                drop();
                ad = 2;
                float currentTime = Time.time;
                if (ad == 2)
                {
                    interval = 0.5f;
                    if ((currentTime - doubleclickedtime) < interval)
                    {
                        DataController.Instance.doubletouch++;
                        print(DataController.Instance.doubletouch);
                        if (DataController.Instance.doubletouch >= 2)
                        {
                            drop();
                            isdoubleclicked = true;
                            doubleclickedtime = -1.0f;
                            testtts.Instance.audioSource.Stop();
                            //testtts.Instance.alltts("유저 주변 이벤트");
                            Checkismine.Instance.testindex();
                            DataController.Instance.doubletouch = 0;
                        }
                    }
                    else
                    {
                        isdoubleclicked = false;
                        doubleclickedtime = currentTime;
                        DataController.Instance.doubletouch = 0;
                        ad = 0;
                        print("doubletouchstop");
                    }
                }
            }
        }
    }

    public bool eventmagictap()  //보이스오버 두손가락 이중탭 
    {
        Checkismine.Instance.testindex();
        var Element = joypanel.GetComponent<AccessibilityNode>();
        Element.IsAccessibilityElement = false;
        return true;
    }

    public void listuitts()
    {
        testtts.Instance.audioSource.Stop();
        //testtts.Instance.alltts("유저 주변 이벤트 안내 입니다");
        testtts.Instance.alltts("아바타 주변 정보 안내 화면입니다. 본인 아바타 중심으로 주변에 있는 전시물 정보를 화면 중앙에 위에서 아래로 순차적 버튼으로 나열 하였습니다. 손가락으로 버튼 영역을 터치하면 안내 멘트가 나옵니다.주변 정보 안내 화면을 닫으시려면 중앙 하단에 있는“설명 닫기” 버튼을 터치 해주세요.");
    }

    int checkjoystick;
    //811.00 , 1018.13, 0.00
    public void pointdown() // 조이스틱 터치감지
    { // inp
        if (ad == 1)
        {// touchPosition
            if (SmartSecurity.GetInt("checkuserevent") != 1)
            {
                //joypanel.GetComponent<AccessibilityNode>().IsAccessibilityElement = false;
                Invoke("checkjoystickon",0.3f);
                // 주기적으로 joyvec의 안정성을 체크하는 함수 호출
                InvokeRepeating("CheckJoyVecStability", 0, checkInterval);
            }
        }
        else
        {
             drop();
        }
    }

    public void checkjoystickon()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            DataController.Instance.touchjoy = 1;
            bgstick.transform.position = Input.mousePosition;
            smallstick.transform.position = Input.mousePosition;
            stickfirstposition = Input.mousePosition;
            checkjoystick = 1;
            if (SmartSecurity.GetInt("ttscontrolindex") == 1) 
            {
                testtts.Instance.audioSource.Stop();
                testtts.Instance.alltts("조이스틱활성화");
            }
            else
            {
                testtts.Instance.audioSource.PlayOneShot(testtts.Instance.joyclickclip); //dingdong sound
            }
            //SetTrigger
            SmartSecurity.SetInt("mathfjoyveindex", 0);
            // 처음에는 안정적으로 변하지 않은 것으로 설정
            isJoyVecStable = false;
            // 최초의 joyvec 값 설정
            //  previousJoyVec = joyvec;
            previousmathfjoyvecx = mathfjoyvecx;
            previousmathfjoyvecy = mathfjoyvecy;
            vectorpreviousmathfjoyvec = new Vector3(mathfjoyvecx, mathfjoyvecy);
            CancelInvoke("checkjoystickon");
        }
        else
        {
            Invoke("checkjoyttson", 2f);
        }
    }

    public void checkjoyttson()
    {
        DataController.Instance.touchjoy = 1;
        bgstick.transform.position = Input.mousePosition;
        smallstick.transform.position = Input.mousePosition;
        stickfirstposition = Input.mousePosition;
        checkjoystick = 1;
        if (SmartSecurity.GetInt("ttscontrolindex") == 1)
        {
            testtts.Instance.audioSource.Stop();
            testtts.Instance.alltts("조이스틱활성화");
        }
        CancelInvoke("checkjoyttson");
        //SetTrigger
        SmartSecurity.SetInt("mathfjoyveindex", 0);
        // 처음에는 안정적으로 변하지 않은 것으로 설정
        isJoyVecStable = false;
        // 최초의 joyvec 값 설정
        //  previousJoyVec = joyvec;
        previousmathfjoyvecx = mathfjoyvecx;
        previousmathfjoyvecy = mathfjoyvecy;
        vectorpreviousmathfjoyvec = new Vector3(mathfjoyvecx, mathfjoyvecy);
        CancelInvoke("checkjoystickon");
    }
 
    public int previousmathfjoyvecy;
    public int previousmathfjoyvecx;
    public Vector3 vectormathfjoyvec;
    public Vector3 vectorpreviousmathfjoyvec;

    private Vector3 previousJoyVec; // 이전 joyvec의 값
    private bool isJoyVecStable = false; // joyvec가 안정적으로 변하지 않았는지 여부

    // 체크할 시간 간격1
    public float checkInterval = 0.5f;  //유지 시간체크.

    public void drag(BaseEventData baseEventData) //조이스틱 드래그
    { 
        if (ad == 1)
        {
            if(checkjoystick == 1)
            {
                DataController.Instance.doubletouch = 0;
                if (SmartSecurity.GetInt("checkuserevent") != 1)
                {
                    joyvecposition();
                    //checkjoyvecposition();
                    PointerEventData pointerEventData = baseEventData as PointerEventData;
                    Vector3 dragposition = pointerEventData.position;
                    float stickdistance = Vector3.Distance(dragposition, stickfirstposition);
                    joyvec = (dragposition - stickfirstposition).normalized;

                    if (stickdistance < stickradius)
                    {
                        smallstick.transform.position = stickfirstposition + joyvec * stickdistance;
                    }
                    else
                    {
                        smallstick.transform.position = stickfirstposition + joyvec * stickradius;
                    }
                }
            }
        }
    }

   
    private void CheckJoyVecStability() // 조이스틱 TTS 호출 제한 
    {
        if (ad == 1)
        {
            // 현재의 joyvec 값과 이전의 joyvec 값을 비교하여 변화 여부 확인
            vectormathfjoyvec = new Vector3(mathfjoyvecx, mathfjoyvecy);
            if(vectormathfjoyvec == vectorpreviousmathfjoyvec)
            {
                isJoyVecStable = true;
                checkjoytts();
                previousmathfjoyvecx = mathfjoyvecx;
                previousmathfjoyvecy = mathfjoyvecy;
                vectorpreviousmathfjoyvec = new Vector3(mathfjoyvecx, mathfjoyvecy);
            }
            else
            {
                isJoyVecStable = false;
                previousmathfjoyvecx = mathfjoyvecx;
                previousmathfjoyvecy = mathfjoyvecy;
                vectorpreviousmathfjoyvec = new Vector3(mathfjoyvecx, mathfjoyvecy);
            }
        }
    }
    public bool IsJoyVecStable()
    {
        return isJoyVecStable;
    }

    public void drop() // 화면에서 터치입력을 그만뒀을때 체크
    {
        CancelInvoke("checkjoystickon");
        checkjoystick = 0;
        DataController.Instance.touchjoy = 0;
        joyvec = Vector3.zero;
        bgstick.transform.position = joystickfirstposition;
        smallstick.transform.position = joystickfirstposition;
        ad = 0;
        SmartSecurity.SetInt("mathfjoyveindex", 0);
        joydrop();
        CancelInvoke("CheckJoyVecStability");
    }

    public void joydrop()
    {
        SmartSecurity.SetInt("ttsindex", 0);
        SmartSecurity.SetInt("ttsindex1", 0);
        SmartSecurity.SetInt("ttsindex2", 0);
        SmartSecurity.SetInt("ttsindex3", 0);
        SmartSecurity.SetInt("ttsindex4", 0);
        SmartSecurity.SetInt("ttsindex5", 0);
        SmartSecurity.SetInt("ttsindex6", 0);
        SmartSecurity.SetInt("ttsindex7", 0);
        
    }
    public int mathfjoyvecx;
    public int mathfjoyvecy;
    public void joyvecposition() //position tts compoment
    {
        joyvecpositionx();
        joyvecpositiony();
        if (SmartSecurity.GetInt("mathfjoyveindex") == 0)
        {
            if (mathfjoyvecx == 1 && mathfjoyvecy == 1) //1~2
            {
                DataController.Instance.joyposition = 0;
                SmartSecurity.SetInt("mathfjoyveindex", 1);
            }
            else if (mathfjoyvecx == 1 && mathfjoyvecy == 0) //R 3
            {
                DataController.Instance.joyposition = 1;
                SmartSecurity.SetInt("mathfjoyveindex", 2);
            }
            else if (mathfjoyvecx == 1 && mathfjoyvecy == -1) //4~5
            {
                DataController.Instance.joyposition = 2;
                SmartSecurity.SetInt("mathfjoyveindex", 3);
            }
            else if (mathfjoyvecx == 0 && mathfjoyvecy == -1) //D 6
            {
                DataController.Instance.joyposition = 3;
                SmartSecurity.SetInt("mathfjoyveindex", 4);
            }
            else if (mathfjoyvecx == -1 && mathfjoyvecy == -1) //7~8
            {
                DataController.Instance.joyposition = 4;
                SmartSecurity.SetInt("mathfjoyveindex", 5);
            }
            else if (mathfjoyvecx == -1 && mathfjoyvecy == 0) //L 9
            {
                DataController.Instance.joyposition = 5;
                SmartSecurity.SetInt("mathfjoyveindex", 6);
            }
            else if (mathfjoyvecx == -1 && mathfjoyvecy == 1)//10~11
            {
                DataController.Instance.joyposition = 6;
                SmartSecurity.SetInt("mathfjoyveindex", 7);
            }
            else if (mathfjoyvecx == 0 && mathfjoyvecy == 1) //U 12
            {
                DataController.Instance.joyposition = 7;
                SmartSecurity.SetInt("mathfjoyveindex", 8);
            }
            else if (mathfjoyvecx == 0 && mathfjoyvecy == 0) // 0
            {
                // print("0,0 0"); 
            }
            SmartSecurity.SetInt("mathfjoyvecxindex", mathfjoyvecx);
            SmartSecurity.SetInt("mathfjoyvecyindex", mathfjoyvecy);
        } 
        else
        {   
            if(SmartSecurity.GetInt("mathfjoyvecxindex")!= mathfjoyvecx || SmartSecurity.GetInt("mathfjoyvecyindex") != mathfjoyvecy)
            {
                SmartSecurity.SetInt("mathfjoyveindex", 0);
            }
        }
            //checkjoyvecposition();
            
    }
    public void joyvecpositionx()
    {
        if (joyvec.x > -0.5 && joyvec.x > -1)
        {
            mathfjoyvecx = Mathf.RoundToInt(joyvec.x);
        }
        else
        {
            mathfjoyvecx = Mathf.FloorToInt(joyvec.x);
        }
      
    }
    public void joyvecpositiony()
    {
        if (joyvec.y > -0.5 && joyvec.y > -1)
        {
            mathfjoyvecy = Mathf.RoundToInt(joyvec.y);
        }
        else
        {
            mathfjoyvecy = Mathf.FloorToInt(joyvec.y);
        }
     
    }
    public void checkjoytts()
    {
        if(SmartSecurity.GetInt("ttscontrolindex")==1)
        {
            SmartSecurity.GetInt("mathfjoyveindex");
            if (SmartSecurity.GetInt("mathfjoyveindex") == 1)
            {
                if (SmartSecurity.GetInt("checkttsindex")==0)
                {
                    JoyStickTTS.Instance.testttsTt("2시 방향");
                }
               
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 2)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt1("3시 방향");
                }
                
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 3)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt2("4시 방향");
                }
                
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 4)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt3("6시 방향");
                }
               
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 5)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt4("8시 방향");
                }
               
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 6)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt5("9시 방향");
                }
                
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 7)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt6("10시 방향"); 
                }
                
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex7", 0);
            }
            else if (SmartSecurity.GetInt("mathfjoyveindex") == 8)
            {
                if (SmartSecurity.GetInt("checkttsindex") == 0)
                {
                    JoyStickTTS.Instance.testttsTt7("12시 방향");
                }
                
                SmartSecurity.SetInt("ttsindex", 0);
                SmartSecurity.SetInt("ttsindex1", 0);
                SmartSecurity.SetInt("ttsindex2", 0);
                SmartSecurity.SetInt("ttsindex3", 0);
                SmartSecurity.SetInt("ttsindex4", 0);
                SmartSecurity.SetInt("ttsindex5", 0);
                SmartSecurity.SetInt("ttsindex6", 0);
            }
        }
        
    }

    // 추후 delete 필요없음 
    public RectTransform rectsmallstick;
    public RectTransform rectbgstick;   

    public void rag()
    {
        DataController.Instance.touchjoy = 1;
        rectbgstick.transform.position = Input.mousePosition;
        rectsmallstick.transform.position = Input.mousePosition;
        stickfirstposition = Input.mousePosition;
    }
    // 추후 delete 필요없음
}
