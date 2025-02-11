using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BackEnd;
using Photon.Pun;
using System.Linq;
using Apple.Accessibility;
using System.Runtime.InteropServices;

public class ObjPrf : MonoBehaviourPunCallbacks
{

    [DllImport("__Internal")]
    private static extern void _displayGraphicData(string data);

    public void DisplayGraphicData(string data)
    {
        _displayGraphicData(data);
    }

    string[] onedotimage = {
    "00000000000000000000008088888888888888888888888888880800000000000000000000000000002f22222222222e22222222222e22e2f000000000000000000000000000000f00000000000f00000000000f00f0f000000000000000000000000048440f00000000000f00000000000f00f07800000000e0325d555555555555151f11115155111f11111155151f11d10300000000302e01000000000000000f00000000000f00000080242322324208000000000f8024d9dddd9d42080f00000000000f004812d9dddd9d218470080000c04734feff7ff7ffeff099999999999999990ffeff7ff7ffeff0000f000010110173ffeffeff371011111111111111110173ffeffeff3710110100000000000010111101000000000000000000000000101111010000000000",
    "00000000000000e666666666666666666666666666666e0000000000000000000000000000f000000000000048840000000000000f0000000000000000000000000000f00000808888f811118f88880800000f0000000000000000000000000000f00000f08244a299992a44280f00000f0000000000000000000000000000f00000f0f0f0f888888f0f0f0f00000f0000000000000000000000000000f000f0f1b8cc44744744cc8b1f0f000f0000000000000000000000000000f000f030223e00000000e322030f000f0000000000000000000000000000f000f08888474cc44cc47488880f000f0000000000000000000000000000f000000070445555555544070000000f00000000000000000000000000007666666666666666666666666666666700000000000000",
    "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000884462aa262244880000000000004822222222224208000000000000801642882411420848126108000000f0aaaaaaaaaaaaaafa000000000080478824114208481242084872080000f000000000000000f00000000000f024114288ae1991ea481242080f0000f000000000000000f000000000c832111111f9998998999f111111630c00f000000000000000f00000000043442622110111222211103162444407007044444444444444740000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
    "000000008888888888888888880000888888888888888888880800000000000000000f0000ffffff0000f000f000000000000000000000f000000000000000000f0000ffffff0000f000f000000000c8cc08000000f00000000000000000430800ffffff00803400f000000070ffff7f000000f00000000000000000001042ffffff24010000f000000000989908000000f00000000000000000000000f399bf00000000f000000000abaa0e000000f0000000000000000000c092c9cccc99c20000f000000000abaa0e000000f00000000000000000000f302e2e2e3e000f00f000000000abaa0e000000f0000000000000000000617855555575680100f000000000232202000000f000000000000000000000102122221100000010222222222222222222221200000000",
    "000000000000000000004cc4c4cccccccc4c4cc400000000000000000000000000000000000000000ff0f0ffffffff0f0ff000000000000000000000000000000000000000000ff0f0ffffffff0f0ff0000000000000000000000000000000000000000021f4f0ffffffff0f4f120000000000000000000000000000000000000000000031ff7777ff130000000000000000000000000000000000000000000000004835aaaa53840000000000000000000000000000000000000000000080162cf244442fc261080000000000000000000000000000000000000000f0002d72888827d2000f00000000000000000000000000000000000000000043297288882792340000000000000000000000000000000000000000000000102122221201000000000000000000000000",
    "00000000000000000000000000000000000000000000000000000000000000000000000080eceece080000000000001f99911999f1001f999111190f0000000000003f333333f30000000000000ff8c007f8f0880f7cc007090f0000000080e89f330033f98e08000000000f8f700ccff1001fe3700c070f00000000f000f01011010f000f000000000ff0c007f8f0000f0fe0010f0f00004856555555555555555555658400000f1f700e1ff0000fe3100e080f00001f1111111111111111111111f100000ff3c007e7f4224f3cf009070f00000f0000000000000000000000f000000f3e300e7cf0000f0f800f0f0f00008f8888888888888888888888f800000f71000763f0000f077000030f000000000000000000000000000000000011111111111100111111111101",
    "00000000000000000000000088888888888800000000000000000000000000000000000000000088001e00e10ff01e00e100880000000000000000000000000000000000001031ffe2e3e33e3e2eff13010000000000000000000000000000000000808888fff8f8f88f8f8fff888808000000000000000000000000000000002123232f232323323232f2323212000000000000000000000000000000000000c0e30000000000003e0c00000000000000000000000000000000000000007c000f00000000f000c700000000000000000000000000000000000080cccfcccfccccccccfcccfccc080000000000000000000000000000008888f8888888888888888888888f880800000000000000000000000000202322232222222222222222222232223202000000000000",
    "00000000000000000000000080cccccccc0800000000000000000000000000000000000040cc54f71111f10f0000f01f11117f45cc040000000000000000404c541f11f100f00000f0000000000f00000f001f11f145c40400000000004f444f44f44474444c7444c44c4447c44447444f44f444f40000000000005f555f55755d755555755555555557555557d55755f555f50000000000c05755555d55474444444444444444444444447455d55555750c0000000080ff9999f99999999f99999f9999f99999f99999999f9999ff080000000040ff4444c44447444c74444c7447c44447c44474444c4444ff040000000020ff2a2232222e2223e22223e22e32222e3222e2222322a2ff020000000020333333333333333333333333333333333333333333333333020000",
    "0000000000000000000000888888888888888800000000000000000000000000000000c04444444444f5545d5555d5455f44444444440c0000000000000000000000f1b3b3bbb3f31111f11f11113f3bbb3b3b1f000000000000000000000000f0f0f0f0f0f0551d5dd5d1550f0f0f0f0f0f000000000000000000000000f0e0f0f0e0f0000f0ff0f0000f0e0f0f0e0f000000000000000000000000f0b8b8bab8f8aa8babbab8aa8f8bab8b8b0f000000000000000000000000f04c74c44447c4447447444c74444c47c40f000000000000000000000000e0232232222232222222222322222322320e0000000000000000f3dddddddddddddddddddd13111131dddddddddddddddddddd7d0000000033222222222222222222222322223222222222222222222222330000",
    "000000cccccccccccccccccccccccccccccccccccccccccccccccc000000000000000f000000f08088888888088088888888080f000000f000000000000000008f888888f8f088aaaa880ff088aaaa880f8f888888f800000000000000c44fc4cccccccc44444444444444444444cccccccc4cf44c000000000000000ff0004404f0804444444444444444080f4044000ff000000000000000000ff02222223201000000000000000010232222220ff000000000000000000ff00000000000000000000000000000000000000ff000000000000000000ff00000000000000000000000000000000000000ff000000000000000008ff80000000000000000000000000000000000008ff800000000000000004334000000000000000000000000000000000000433400000000",
    "00000000000000000000000000882cc28800000000000000000000000000000000000000000000000000f8000ff0008f00000000000000000000000000000000000000000000cc3300008118000033cc000000000000000000000000000000000080883f00e0224845548420e200f38808000000000000000000000000004847440480041e00002c22e5400840447484000000000000000000000000212e22021002a7442401007820012022e212000000000000000000000000001011cf007044212aa212440700fc1101000000000000000000000000000000000033cc880018810088663300000000000000000000000000000000000000000000f0000ff0000f000000000000000000000000000000000000000000000000001143341100000000000000000000000000",
    "000000000000000000000000008848848800000000000000000000000000000000000000000000000088f8000ff0008f880000000000000000000000000000000000000000882e4308008338008034e2880000000000000000000000000000000080880f10022648455484622001708808000000000000000000000000004847444480041e00002c22e5400844447484000000000000000000000000212e22221002a7442401007820012222e2120000000000000000000000000021220e800446212aa212200b42881f010000000000000000000000000000000011472c01001cc10084982e0300000000000000000000000000000000000000001161880ff08807000000000000000000000000000000000000000000000000000021120000000000000000000000000000"};

    string[] twodotimage = {
    "00000000000000000000c8444c444444c4448c00000000000000000000000000000000884c4c44547555575555557555574544444c8c0800000000000000000000230e1f5d55d5d15555555555d5d155551d0f2e0300000000000000000000000f0f8f88f8f08888888888f8f088880f0f0f0000000000000000000000ae0bafaaaaaaaaaaaaaaaaaaaaaaaaaaaa0fab0e00000000000000000000000f2f000000e02202000020220e0000200f0f0000000000000000000000000f4f000000f00000000000000f0000400f0f0000000000000000000000000f3f33e322222e22e222222e22e233330f0f00000000000000000000000087abaa7244444744744444474474a2aa8b07000000000000000000000000002362555555555555555555555565220300000000000000",
    "00808888888888888888888888888888888888888888888888888888080000f0000000008008002cc200000000004822840000000000c8ce08800f0000f0004888240110421881241142c8120000002184242144189324010f0000f03400002d420800300300000011000000000010000000b08300000f0000f00000f00000e100001e1e0000000000000000000000003094c2000f0000f0428898ca8c2400c0830f00000000000000000000000000f0300c0f0000f080f00ff0af4aa4ba7a88a4aa4a88a4aa4a88a4aa4a88a4aa5a890f0000f088b99d998998cb8898aa898898aa898898aa898898aa898898aa0f0000f000000000f000000000000f00000000f000000000000f000000000f00001011111111111111111111111111111111111111111111111111110100",
    "000000000000008044444444444444444444444444440800000000000000000000000080ac89888888888888888888888888888898ca0800000000000000000000dcdf99f9fd111f11f111111f11f1119f9f99f98900000000000000000000333f11f1f1000f00f000000f00f0001f1f11f11100000000000000000000000f00f0f0000f00f000000f00f0000f0f00f00000000000000000000000000f00f0f0000f00f000000f00f0000f0f00f00000000000000000000000000f00f0f0000f00f000000f00f0000f0f00f00000000000000000000000000f00f0f0000f00f000000f00f0000f0f00f0000000000000000000000000000000702a2222222222222222a207000000000000000000000000000000000000001022222273772322220100000000000000000000",
    "0000000000000000008888888888888888888888880000000000000000000000000000008024918888888888888888888888881942080000000000000000000000c09224010000000000000000000000001042290c000000000000000000008f3e00000000000000000000000000000000e3f8000000000000000000f0f90000000000000000000000000000000000009f0f0000000000000000fc0f000000000000000000000000000000000000f0cf0000000000000000f90f000000000000000000000000000000000000f09f0000000000000000f73046cccccccccccccccccccccccccccccccc64037f000000000000000010c32c22c2002c222e22222222e222c2002c22c23c0100000000000000000000312232223322232222222232223322232213000000000000",
    "00000000000000000000000000000000000000800c00000000000000000000000000000000000000000000000000884824810700000000000000000000000000000000000000482422221111008064452222222222e20000000000000000004824222212dddd1d210c00000008000000000048f200000000000000003c300100000010110000000000000f101111111100f80000000000000070880000000000000000000000004803404444444412f0000000000000000000311d111111008600000040120000000000000088f40000000000000000000030440800000011112242444488181111111100f000000000000000000000000010111111222222224244444444444444447400000000000000000000000000000000000000000000000000000000000000000000",
    "0000000000000000000000000048444c0800000000000000000000000000000000c022222222eeeeee2ee201000ff022eeeeee2e222222c200000000000000f0000000001011110010427445340010111100000000f000000000000000f0000000000000002e2efeffffce0000000000000000f00000000000000010420800008888001f1f3f77771300008088080000803400000000000000000010111101f0000f0f0f0000000000100e1111110100000000000000000000000000000f00230f0f000000000000c30000000000000000000000000000000000f0000000470f000000000000300c00000000000000000000000000000000b4000000000f000000000000000f0000000000000000000000000000000000212222224744444444442613010000000000000000",
    "00000000000000000000000000484444840000000000000000000000000000000000000000000000000000e122221e0000000000000000000000000000000000000000000000000000f000000f0000000000000000000000000000000000000000000000000000f000000f0000000000000000000000000000000000000000000000804824110800914284080000000000000000000000000000000000008024010000008fccfeeece184208000000000000000000000000000000001e0000000080ffffffffffcf00e9000000000000000000000000000000000f0000000010333389ffff17c0ff00000000000000000000000000000000218400000000c0feffffffee7f130000000000000000000000000000000000002142444474777777371300000000000000000000",
    "000000000000000000000000c4444444444c00000000000000000000000000000000000000000000000010f111111f0100000000000000000000000000000000000000000000000000bc0000cb000000000000000000000000000000000000000000000000c01a00111100a10c0000000000000000000000000000000000000000806c0110212222120110c60800000000000000000000000000000000008016000000e00080cccccc006308000000000000000000000000000000001e00000080ffccfcffffff6f00e1000000000000000000000000000000008700806c3333629c317337010078000000000000000000000000000000001063ff8f00000010010000c836010000000000000000000000000000000000001011333333333333110100000000000000000000",
    "0000000000000000000000000000488400000000000000000000000000000000000000000000000000008098ceec89080000000000000000000000000000888888000000000080e88988888888988e080000000000888888000000008f8800610800000021e22222222222222e1200000080160088f8000000000000210c3044441e55555555555555555555e1444403c012000000000000000000104244448700000000000000000000784444240100000000000000000000000000007088880000000000008888070000000000000000000000000000000000e01100004944444444940000110e00000000000000000000000000000000100e002e010000000010e200e0070000000000000000000000000000000030222232000000000000232232030000000000000000",
    "0000000000000000000000c44444444444444c0000000000000000000000000000000000000000000000210c0000c012000000000000000000000000000000000000000000000000001f1111f1000000000000000000000000000000000000000000000088484423222232448488000000000000000000000000000000000000c05248535953599535953584250c00000000000000000000000000000000f000404e00c8cc088008e0e4000f0000000000000000000000000000000000c30000807f7703733700003c00000000000000000000000000000000000000c300130000000000003c0000000000000000000000000000000000000000004b788007700887b40000000000000000000000000000000000000000003222233222222332222300000000000000000000",
    "000000000000000000000000000080888888888808000000000000000000000000000000000000000000002c49222222222294c20000000000000000000000000000000000000000e0c10300c0e20600101e0e0000000000000000000000a622420800008044123033333333333333034384000000000000000000000087008700c01200000000000000000000000010c200000000000000000000108600110f0000000000000000000000000000000f000000000000000000003084000f0000000000000000000000000000000f00000000000000000000000011318400000000000000000000000080340000000000000000000000000000001022840800000000000000882412000000000000000000000000000000000000001033333333333333000000000000000000",
    "000000000000000000c8444444080000000000000000000000000000000000000000000000323322220c00c3000000888888888888080000000000000000000000000000000000c3001011113d2222222211110000000000000000000000000000000000000011e922b2840000000000000000000000000000000000000000000000000000c7034378000000000000000000000000000000000000000000000000000000e11e00000000000000000000000000000000000000000000000000000000f00f00000000000000000000000000000000000000000000000000000000f00f00000000000000000000000000000000000000000000000000888888f88f8888880000000000000000000000000000000000000000001022222222222222220100000000000000000000"};


    private static ObjPrf instance;

    public static ObjPrf Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<ObjPrf>();
            if (instance != null) return instance;
            var container = new GameObject("ObjPrf");
            return instance;
        }
    }


    public GameObject panel; //감지된 오브젝트 패널.
    private int i;
    public int pi; //playerprf index.
    public int oi; //objprf index. 
    public string names;
    public GameObject listenpanel; //전시물정보패널
    public GameObject listpanel; //안씀
    public GameObject listttspanel; //캐릭터정보패널ㅡ


    #region nameset
    public void nameset()
    {
        if (pi == 0)
        {
            names = "A";
        }
        else if (pi == 1)
        {
            names = "B";
        }
        else if (pi == 2)
        {
            names = "C";
        }
        else if (pi == 3)
        {
            names = "D";
        }
        else if (pi == 4)
        {
            names = "E";
        }
        else if (pi == 5)
        {
            names = "F";
        }
        else if (pi == 6)
        {
            names = "G";
        }
        else if (pi == 7)
        {
            names = "H";
        }
        else if (pi == 8)
        {
            names = "I";
        }
        else if (pi == 9)
        {
            names = "J";
        }
        else if (pi == 10)
        {
            names = "K";
        }
        else if (pi == 11)
        {
            names = "L";
        }
        else if (pi == 12)
        {
            names = "M";
        }

    }
    #endregion

   

    public void panelopen()
    {
        pi = 0;
        oi = 0;
    }
    public GameObject[] checkindex = new GameObject[12];
    public Text[] infoTt = new Text[12];
    public GameObject[] infoTtobj = new GameObject[12];

    public Text listTt;  // 주변이벤트 오브젝트패널 상단바 오브젝트 이름
    public GameObject listTtobj; //보이스오버 정보가져오기 오브젝트라벨 오브젝트 이름

    public Text listenTt; // 삭제예정 유저캐릭터정보 네임정보

    public Text showusernameTt; //캐릭터네임 
    public string[] showusername = new string[20];
    public void exit()  //list 초기화
    {
        for (i = 0; i < checkindex.Length; i++)
        {
            checkindex[i].SetActive(false);
            SmartSecurity.SetInt("checklisten" + i, 0);
        }
    }
    public void ttsexitbtn() //유저 주변이벤트 닫기버튼
    {
        SmartSecurity.SetInt("checkuserevent", 0);
    }

    private List<Collider2D> nearbyColliders = new List<Collider2D>();
    public void testindex()
    {
       
        if (TestPlayer.Instance.PV.IsMine)
        {
            //ismine인 콜라이더 allcol 를 가져ㅑ와야함.
            JoyStickTTS.Instance.audioSource.Stop();
            exit();
            TestPlayer.Instance.allcol = Physics2D.OverlapBoxAll(TestPlayer.Instance.ts.transform.position, TestPlayer.Instance.size, 0, TestPlayer.Instance.alllayer);
            // 리스트 비우기
            nearbyColliders.Clear();

            // 주변의 Collider들 리스트에 추가
            nearbyColliders.AddRange(TestPlayer.Instance.allcol);

            // 가까운 순으로 정렬
            nearbyColliders.Sort((c1, c2) =>
            {
                float dist1 = Vector2.Distance(TestPlayer.Instance.ts.transform.position, c1.transform.position);
                float dist2 = Vector2.Distance(TestPlayer.Instance.ts.transform.position, c2.transform.position);
                return dist1.CompareTo(dist2);
            });

            // 정렬된 Collider들에 대한 작업 수행
            for (int i = 0; i < nearbyColliders.Count; i++)
            {
                Collider2D collider = nearbyColliders[i];
                // 여기에 필요한 작업 수행
                Debug.Log(collider.gameObject.name);
                if (i >= 7)
                {
                    break;
                }
                else
                {
                    nameset();
                    if (nearbyColliders[i].CompareTag("Player"))
                    {
                        //더블탭시 유저정보 닉네임 저장.
                        showusername[i] = nearbyColliders[i].GetComponent<TestPlayer>().tt.text;
                        SmartSecurity.SetString("username" + i, showusername[i]);
                        print(SmartSecurity.GetString("username" + i));
                        infoTt[i].text = "유저 캐릭터 " + names;
                        SmartSecurity.SetString("listname" + i, "유저 캐릭터 " + names);
                        SmartSecurity.SetInt("checklisten" + i, 1);  // 유저 일때 로컬데이터 checklisten 1로 저장
                        var showname = nearbyColliders[i].GetComponent<TestPlayer>().tt.text;
                        SmartSecurity.SetString("listplayerindex" + i, showname);
                        pi++;
                    }
                    else
                    {
                        infoTt[i].text = nearbyColliders[i].name;   // 감지된 오브젝트 거리순으로 name 순서 
                        SmartSecurity.SetString("listname" + i, nearbyColliders[i].name);  // 감지된 오브젝트 거리순으로 name 순서 로컬저장
                        SmartSecurity.SetInt("checklisten" + i, 2);    // 전시물 일때 로컬데이터 checklisten 2로 저장
                        var objTt = nearbyColliders[i].GetComponent<ObjectLabel>().objTt.text;
                        var objindex = nearbyColliders[i].GetComponent<ObjectLabel>().objint;
                        SmartSecurity.SetInt("listobjindex" + i, objindex);  
                        SmartSecurity.SetString("objtts" + i, objTt);
                    }
                    checkindex[i].SetActive(true);  //  감지된 오브젝트 개수만큼 리스트 True 
                }

            }
            if (TestPlayer.Instance.allcol.Length > 0)
            {
                if (usercheck == 0)
                {
                    usercheck = 1;
                    if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                    {
                        testtts.Instance.audioSource.Stop();
                        testtts.Instance.alltts("유저 주변 이벤트");
                    }
                    Invoke("listuitts", 1f);
                }
                Invoke("usercheckon", 1f);
            }
        }

    }
    public int usercheck;
    public void listuitts()
    {
        panel.SetActive(true);
        if (SmartSecurity.GetInt("ttscontrolindex") == 1)
        {
            testtts.Instance.audioSource.Stop();
            testtts.Instance.alltts("유저 주변 이벤트 안내 입니다");
        }
    }
    public void usercheckon()
    {
        usercheck = 0;
    }



    public void checkprfindex()
    {
        var name = SmartSecurity.GetString("listname");
        if (SmartSecurity.GetInt("checklisten") == 1) //player  유저 캐릭터정보 화면으로 이동
        {
            showusernameTt.text = SmartSecurity.GetString("listplayerindex");
            listenTt.text = name; //필요없음 delete
            if (SmartSecurity.GetInt("getwmsureindex") == 0)
            {//여성일때.
                TextController.Instance.shwocharemotionimage.sprite = TextController.Instance.avataemotionsprite[SmartSecurity.GetInt("getwitememotion")];
                TextController.Instance.showchartopimage.sprite = TextController.Instance.wavatatopsprite[SmartSecurity.GetInt("getwitemtop")];
                TextController.Instance.showcharfaceimage.sprite = TextController.Instance.wavatafacesprite[SmartSecurity.GetInt("getwitemface")];
                TextController.Instance.checkcharhair();
            }
            else
            {//남성일때.
                TextController.Instance.shwocharemotionimage.sprite = TextController.Instance.avataemotionsprite[SmartSecurity.GetInt("getitememotion")];
                TextController.Instance.showchartopimage.sprite = TextController.Instance.avatatopsprite[SmartSecurity.GetInt("getitemtop")];
                TextController.Instance.showcharfaceimage.sprite = TextController.Instance.avatafacesprite[SmartSecurity.GetInt("getitemface")];
                TextController.Instance.checkshowcharhairimage.SetActive(false);
            }

            getcharinfo(SmartSecurity.GetString("listplayerindex"));
           // panel.SetActive(false);
            TextController.Instance.blindcharinfopanel.SetActive(true);
            listttspanel.SetActive(true);
            panel.SetActive(false);
        }
        else if (SmartSecurity.GetInt("checklisten") == 2) //obj 오브젝트정보 화면으로 이동 -오브젝트이름으로 표기
        {
            listTt.text = name;
            listTtobj.GetComponent<AccessibilityNode>().AccessibilityLabel = listTt.text;
            //panel.SetActive(false);
            TextController.Instance.objimage.sprite = TextController.Instance.objlistimage[SmartSecurity.GetInt("listobjindex")];
            TextController.Instance.showobjimage.SetActive(true);
            listenpanel.SetActive(true);
            panel.SetActive(false);

            if (SmartSecurity.GetInt("bleconnectindex") == 1)
            {
                if (SmartSecurity.GetInt("photonroomindex") == 1) //월드1층
                {

                    DisplayGraphicData(onedotimage[SmartSecurity.GetInt("listobjindex" + DataController.Instance.listobjindex)]);
                }
                else if (SmartSecurity.GetInt("photonroomindex") == 2) //월드2층
                {
                    DisplayGraphicData(twodotimage[SmartSecurity.GetInt("listobjindex" + DataController.Instance.listobjindex)]);
                }
            }
        }
    }

    public void getcharinfo(string st) //유저아이템 데이터 저장.
    {
        Where where = new Where();
        var bro = Backend.Social.GetUserInfoByNickName(st);
        if (bro.IsSuccess())
        {
            var usernickname = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            where.Equal("mynickname", usernickname);
            BackendReturnObject bro1 = Backend.GameData.Get("publicitemdata", where, 1);
            if (bro1.IsSuccess())
            {
                //남 여 인덱스 저장.
                string wmsureindex = bro1.GetReturnValuetoJSON()["rows"][0]["wmsureindex"]["N"].ToString();
                SmartSecurity.SetInt("getwmsureindex", int.Parse(wmsureindex));

                //남성 캐릭터 인덱스 저장.
                string itememotion = bro1.GetReturnValuetoJSON()["rows"][0]["itememotion"]["N"].ToString();
                string itemface = bro1.GetReturnValuetoJSON()["rows"][0]["itemface"]["N"].ToString();
                string itemtop = bro1.GetReturnValuetoJSON()["rows"][0]["itemtop"]["N"].ToString();
                SmartSecurity.SetInt("getitememotion", int.Parse(itememotion));
                SmartSecurity.SetInt("getitemface", int.Parse(itemface));
                SmartSecurity.SetInt("getitemtop", int.Parse(itemtop));

                //여성 캐릭터 인덱스 저장.
                string witememotion = bro1.GetReturnValuetoJSON()["rows"][0]["witememotion"]["N"].ToString();
                string witemface = bro1.GetReturnValuetoJSON()["rows"][0]["witemface"]["N"].ToString();
                string witemtop = bro1.GetReturnValuetoJSON()["rows"][0]["witemtop"]["N"].ToString();
                SmartSecurity.SetInt("getwitememotion", int.Parse(witememotion));
                SmartSecurity.SetInt("getwitemface", int.Parse(witemface));
                SmartSecurity.SetInt("getwitemtop", int.Parse(witemtop));

                string mcharimageindex = bro1.GetReturnValuetoJSON()["rows"][0]["dotmcharimageindex"]["N"].ToString();
                string wcharimageindex = bro1.GetReturnValuetoJSON()["rows"][0]["dotwcharimageindex"]["N"].ToString();
                SmartSecurity.SetInt("getdotmcharimageindex", int.Parse(mcharimageindex));
                SmartSecurity.SetInt("getdotwcharimageindex", int.Parse(wcharimageindex));

                //아이템설명 텍스트 저장.
                string iteminfoTt = bro1.GetReturnValuetoJSON()["rows"][0]["iteminfoTt"]["S"].ToString();
                SmartSecurity.SetString("getiteminfoTt", iteminfoTt);
            }
        }
    }
    public void listenbackbtn() 
    {
        listenpanel.SetActive(false);
        panel.SetActive(true);
    }
    
    public void listttsbackbtn() //유저 캐릭터정보 뒤로가기버튼 추후delete
    {
        listttspanel.SetActive(false);
        //panel.SetActive(true);
    }
    public void listttsexitbtn() //유저 캐릭터정보 닫기버튼 유저주변이벤트 화면으로 이동
    {
        SmartSecurity.SetInt("checkuserevent", 0);
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                testtts.Instance.audioSource.Stop();
                testtts.Instance.alltts("유저 캐릭터 정보 닫기 버튼 유저 캐릭터 정보 화면을 닫습니다");
            }
        }
        listttspanel.SetActive(false);
        panel.SetActive(true);
    }
    //int[] checkpanelindex = new int[12];
    public void checklistenpanel()
    {
        if (SmartSecurity.GetInt("checklisten") == 1)
        {
            panel.SetActive(false);
            listenpanel.SetActive(true);
        }
        else if (SmartSecurity.GetInt("checklisten") == 2)
        {
            panel.SetActive(false);
            listttspanel.SetActive(true);
        }
    }
    public void objexit() //전시물설명화면 닫기버튼  
    {
       // SmartSecurity.SetInt("checkuserevent", 0);
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (SmartSecurity.GetInt("ttscontrolindex") == 1)
            {
                testtts.Instance.audioSource.Stop();
                testtts.Instance.alltts("전시물 설명화면을 닫습니다");
            }
        }
        TextController.Instance.blindobjinfopanel.SetActive(true);
        TextController.Instance.blindshowimagepanel.SetActive(true);
        panel.SetActive(true);
        listenpanel.SetActive(false);

    }
    public void objback() //전시물설명화면 뒤로가기버튼
    {
        if (SmartSecurity.GetInt("ttscontrolindex") == 1)
        {
            testtts.Instance.audioSource.Stop();
            testtts.Instance.alltts("전시물 설명 화면 뒤로가기 버튼 유저 주변 이벤트 화면으로 이동합니다");
        }
    }
    public void charexitallbtn()  //캐릭터 설명하기 모든 닫기버튼 컴포넌트
    {
        //TestJoyStick.Instance.joypanel.GetComponent<AccessibilityNode>().IsAccessibilityElement = true;
    }
   
}