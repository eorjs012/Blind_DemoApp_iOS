using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;
using System.Threading.Tasks;
using Apple.Accessibility;

public class testtts : MonoBehaviour
{

    private static testtts instance;

    public static testtts Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<testtts>();
            if (instance != null) return instance;
            var container = new GameObject("testtts");
            instance = container.AddComponent<testtts>();
            return instance;
        }
    }
    public AudioSource audioSource;
    public AudioClip eventclip;
    public AudioClip joyclickclip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        //조이스틱 인덱스 0으로 세팅
        SmartSecurity.SetInt("ttsindex", 0);
        SmartSecurity.SetInt("ttsindex1", 0);
        SmartSecurity.SetInt("ttsindex2", 0);
        SmartSecurity.SetInt("ttsindex3", 0);
        SmartSecurity.SetInt("ttsindex4", 0);
        SmartSecurity.SetInt("ttsindex5", 0);
        SmartSecurity.SetInt("ttsindex6", 0);
        SmartSecurity.SetInt("ttsindex7", 0);
        //StartCoroutine("st");
    }

    //v1beta1 update버전 안정성x , v1 안정성o
    //string testurl_ = "https://texttospeech.googleapis.com/v1beta1/text:synthesize?key=AIzaSyCY59md0ekvhj0XShdlYphjOBGKA8FcB_g";
    string testurl_ = "https://texttospeech.googleapis.com/v1/text:synthesize?key=AIzaSyCY59md0ekvhj0XShdlYphjOBGKA8FcB_g";
    
    
    public string apiKey;
    public double voicePitch;
    public double voiceRate;
    
    
    SetTextToSpeech tts = new SetTextToSpeech();
    public async Task<string> testttspostAsync(object data)
    {
        string str = JsonUtility.ToJson(data);
        var bytes = System.Text.Encoding.UTF8.GetBytes(str);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testurl_);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = bytes.Length;

        try
        {
            using (var stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
                await stream.FlushAsync();
            }

            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = await reader.ReadToEndAsync();

            return json;
        }
        catch (WebException e)//예외처리
        {
            Debug.Log(e);
        }
        finally //예외처리
        {

        }
        return null;
    }
    public void alltts(string text)
    {
        testpositioninitall(text);
        testcreateaudioAsyncall();
        text = "";
    }
    private async void testcreateaudioAsyncall() //전체  
    {
        var str = await testttspostAsync(tts);

        GetContent info = JsonUtility.FromJson<GetContent>(str);

        var bytes = Convert.FromBase64String(info.audioContent);
        var f = testConvertByteFloat(bytes);

        AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
        audioClip.SetData(f, 0);

        audioSource.PlayOneShot(audioClip);
        StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
        //Destroy(audioClip);
    }

   
    public void testpositioninitall(string text)
    {
        SetInput si = new SetInput();
        si.text = text;
        tts.input = si;

        SetVoice sv = new SetVoice();
        sv.languageCode = "ko-KR";
        sv.name = "ko-KR-Standard-A";
        sv.ssmlGender = "FEMALE";
        tts.voice = sv;

        SetAudioConfig sa = new SetAudioConfig();
        sa.audioEncoding = "LINEAR16";
        //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");
        sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
        sa.pitch = 1; //클라 0.6f set
        sa.volumeGainDb = 0;
        tts.audioConfig = sa;
    }
    private static float[] testConvertByteFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 2];

        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = BitConverter.ToInt16(array, i * 2) / 32768.0f;
        }

        return floatArr;
    }
   
  
    private IEnumerator DestroyClipAfterPlay(AudioClip clip, AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(clip);
    }

   

    [System.Serializable]
    public class SetTextToSpeech
    {
        public SetInput input;
        public SetVoice voice;
        public SetAudioConfig audioConfig;
    }
    [System.Serializable]
    public class SetInput
    {
        public string text;
        //public string ssml;
    }
    [System.Serializable]
    public class SetVoice
    {
        public string languageCode;
        public string name;
        public string ssmlGender;
    }
    [System.Serializable]
    public class SetAudioConfig
    {
        public string audioEncoding;
        public float speakingRate;
        public int pitch;
        public int volumeGainDb;
        //public int sampleRateHertz;
    }
    [System.Serializable]
    public class GetContent
    {
        public string audioContent;
    }
}
