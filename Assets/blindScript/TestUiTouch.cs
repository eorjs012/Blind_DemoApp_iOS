using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //UI클릭시 터치이벤트 발생방지.
using UnityEngine.UI;

public class TestUiTouch : MonoBehaviour, IPointerDownHandler//, IBeginDragHandler //, IDragHandler, IPointerClickHandler
{
    private static TestUiTouch instance;

    public static TestUiTouch Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<TestUiTouch>();
            if (instance != null) return instance;
            var container = new GameObject("TestUiTouch");
            return instance;
        }
    }
    public GameObject testuipanel;
    public ScrollRect sr;
    public RectTransform srcontent;
    
    //public GameObject testuibotpanel;
    void Start()
    {
        sr.verticalNormalizedPosition = 1; //1(panel),0(botpanel).
        DataController.Instance.touchjoy = 0;
        //lastouchtime = Time.time
    }
    
    public void touchscroll() //하단 메뉴바.
    {
        if(srcontent.anchoredPosition.y ==100)
        {
            print("src100"); //터치불가.
        }
        else
        {
            print("src0"); //터치가능(botpanel).
        }
    }
    public GameObject testpanel;
    //조이스틱 이동중일떄, 인덱스 1 일떄 봇세팅패널로 터치 감지됬을때 tts(터치불가영역).
    public void checktouchevent() //miss
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                print("touch0");
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    print("touch1");
                    DataController.Instance.touchjoy = 1;
                    TextController.Instance.touchjoypanel.SetActive(true);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    print("touch2");
                    DataController.Instance.touchjoy = 0;
                    TextController.Instance.touchjoypanel.SetActive(false);

                }
            }
        }

    }
    public Vector2 touchbeganpos;
    public Vector2 touchendedpos;
    public Vector2 touchdif;
    private float swipeSensitivity;
    public Vector3 touchedpos;
 
    public void OnPointerDown(PointerEventData eventData) //오브젝트 눌렀을때. 버튼을 클릭/터치하는 순간 실행됨.
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //UI터치.
                DataController.Instance.touchjoy = 0;

            }
            else
            {
                //화면터치(조이스틱패널).

            }
        }
    }
    public float interval = 0.25f;
    public float doubleclickedtime = -1.0f;
    float currenttimeclick;
    public bool isdoubleclicked = false;
    public void doubletap() //더블탭 조이스틱 분리.
    {
        //Touch touch = Input.GetTouch(0);
        if (DataController.Instance.eventtouchindex ==1) //eventtouchindex 오브젝트감지.
        {
            if (Input.touchCount > 1)
            {
                if ((Time.time - doubleclickedtime) < interval)
                {
                    isdoubleclicked = true;
                    doubleclickedtime = -1.0f;
                    //testpanel.transform.position = new Vector3(910, 0, 0);
                    Debug.Log("double click");
                    DataController.Instance.doubletouch++;
                    if (DataController.Instance.doubletouch >= 2)
                    {
                        testuipanel.SetActive(true);
                    }
                }
                else
                {
                    isdoubleclicked = false;
                    doubleclickedtime = Time.time;
                    
                }
            }
            else if(Input.touchCount==1)
            {
                rag();
            }
        }        
        else
        {
            if(Input.touchCount ==1)
            {
                rag();
            }
            
        }

    }
  
    public Vector3 first;
    public void joypanelrag()
    {
        TextController.Instance.joypanel.SetActive(false);
        TextController.Instance.touchjoypanel.SetActive(true);
        print("joypanelrag");
        //rag();
    }
    public void startrag()
    {
        TextController.Instance.touchjoypanel.SetActive(true);
        DataController.Instance.touchjoy = 1;
        TestJoyStick.Instance.joystickfirstposition = new Vector3(132.6f, 238, 0);
    }
  
    public GameObject botsetting;
    public void startdrag(BaseEventData baseEventData)
    {
        if(DataController.Instance.touchjoy ==0)
        {
            botsetting.transform.position = new Vector3(0, 750, 0);
        }
    }
    
    public void rag()
    {
        DataController.Instance.touchjoy = 1;
        TestJoyStick.Instance.rectbgstick.transform.position = Input.mousePosition;
        TestJoyStick.Instance.rectsmallstick.transform.position = Input.mousePosition;
        TestJoyStick.Instance.stickfirstposition = Input.mousePosition;
        //joyon(); //invok 체크안됨.  조이스틱터치하고있으면 스몰조이스틱만나옴.
    }
    public void drag(BaseEventData baseEventData) 
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 dragposition = pointerEventData.position;
        TestJoyStick.Instance.joyvec = (dragposition - TestJoyStick.Instance.stickfirstposition).normalized;

        float stickdistance = Vector3.Distance(dragposition, TestJoyStick.Instance.stickfirstposition);
        if (stickdistance < TestJoyStick.Instance.stickradius)
        {
            TestJoyStick.Instance.rectsmallstick.transform.position = TestJoyStick.Instance.stickfirstposition + TestJoyStick.Instance.joyvec * stickdistance;
        }
        else
        {
            TestJoyStick.Instance.rectsmallstick.transform.position = TestJoyStick.Instance.stickfirstposition + TestJoyStick.Instance.joyvec * TestJoyStick.Instance.stickradius;
        }
    }
    public void drop() 
    {

        if (DataController.Instance.one == 1)
        { //chat panel open.
            TestJoyStick.Instance.joystickfirstposition = new Vector3(132.6f, 545.3f, 0);
        }
        else
        {//chat panel close.
            TestJoyStick.Instance.joystickfirstposition = new Vector3(132.6f, 238, 0); 
        }
        DataController.Instance.touchjoy = 0;
        TestJoyStick.Instance.joyvec = Vector3.zero;//애니메이터 미끄럼방지.
        TestJoyStick.Instance.rectbgstick.transform.position = TestJoyStick.Instance.joystickfirstposition;
        TestJoyStick.Instance.rectsmallstick.transform.position = TestJoyStick.Instance.joystickfirstposition;
    }
    public void exituipanel()
    {
        DataController.Instance.doubletouch = 0;
    }
    public void joyon() 
    {
        Invoke("joyoff", 5f);
        print("joyoff");
    }
    public void joyoff()
    {
        if(DataController.Instance.touchjoy == 1) //joypanel touch.
        {
            if(TestJoyStick.Instance.joyvec.x == 0 || TestJoyStick.Instance.joyvec.y == 0)
            {
                TestJoyStick.Instance.bgstick.transform.position = TestJoyStick.Instance.joystickfirstposition;
                TestJoyStick.Instance.smallstick.transform.position = TestJoyStick.Instance.joystickfirstposition;
                DataController.Instance.touchjoy = 0;
            }
        }
    }
}
/*public void OnPointerClick(PointerEventData eventData)
{
    //throw new System.NotImplementedException();
    /*if(Input.touchCount<2)
    {
        if ((Time.time - doubleclickedtime) < interval)
        {
            isdoubleclicked = true;
            doubleclickedtime = -1.0f;
            //testpanel.transform.position = new Vector3(910, 0, 0);
            Debug.Log("double click");
            DataController.Instance.doubletouch++;
            if (DataController.Instance.doubletouch <= 2)
            {
                testuipanel.SetActive(true);
            }
        }
    }
    else
    {
        isdoubleclicked = false;
        doubleclickedtime = Time.time;
        joypanelrag();
        print("onetouch");
   }
}*/



