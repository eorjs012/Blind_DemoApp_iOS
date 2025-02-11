using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Http;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.Networking;
using LitJson;
using System.Web;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

public class JoyStickTTS : MonoBehaviour
{
    private static JoyStickTTS instance;

    public static JoyStickTTS Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<JoyStickTTS>();
            if (instance != null) return instance;
            var container = new GameObject("JoyStickTTS");
            instance = container.AddComponent<JoyStickTTS>();
            return instance;
        }
    }
    //string testurl_ = "https://texttospeech.googleapis.com/v1beta1/text:synthesize?key=AIzaSyCY59md0ekvhj0XShdlYphjOBGKA8FcB_g";
    string testurl_ = "https://texttospeech.googleapis.com/v1/text:synthesize?key=AIzaSyCY59md0ekvhj0XShdlYphjOBGKA8FcB_g";
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
        catch (WebException e)
        {
            Debug.Log(e);
        }

        return null;
    }
    private IEnumerator DestroyClipAfterPlay(AudioClip clip, AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(clip);
    }
    private async void allcreateaudioAsync()
    {
        var str = await testttspostAsync(tts);

        GetContent info = JsonUtility.FromJson<GetContent>(str);

        var bytes = Convert.FromBase64String(info.audioContent);
        var f = testConvertByteFloat(bytes);

        AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
        audioClip.SetData(f, 0);

        audioSource.PlayOneShot(audioClip);
        //Destroy(audioClip); //audio clip count 제거
        StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
        // DataController.Instance.ttsindex = 1;

        //audio clip count 제거완료 destroy.
        //sample sound memory 누적.
    }
    public void allpositioninit(string text)
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
        //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
        sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);

        sa.pitch = 1;
        sa.volumeGainDb = 0;
        tts.audioConfig = sa;
    }

    private async void testcreateaudioAsync() //2시방향
    {
        if (SmartSecurity.GetInt("ttsindex") == 1)
        {
            SmartSecurity.SetInt("ttsindex", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            //Destroy(audioClip); //audio clip count 제거
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            // DataController.Instance.ttsindex = 1;

            //audio clip count 제거완료 destroy.
            //sample sound memory 누적.
        }
    }

    private async void testcreateaudioAsync1() //3시방향
    {

        if (SmartSecurity.GetInt("ttsindex1") == 1)
        {
            SmartSecurity.SetInt("ttsindex1", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            // Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    private async void testcreateaudioAsync2() //4시방향
    {

        if (SmartSecurity.GetInt("ttsindex2") == 1)
        {
            SmartSecurity.SetInt("ttsindex2", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            //Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    private async void testcreateaudioAsync3() //6시방향
    {

        if (SmartSecurity.GetInt("ttsindex3") == 1)
        {
            SmartSecurity.SetInt("ttsindex3", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            // Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    private async void testcreateaudioAsync4() //7시방향
    {

        if (SmartSecurity.GetInt("ttsindex4") == 1)
        {
            SmartSecurity.SetInt("ttsindex4", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            // Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    private async void testcreateaudioAsync5() //9시방향
    {

        if (SmartSecurity.GetInt("ttsindex5") == 1)
        {
            SmartSecurity.SetInt("ttsindex5", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            //Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    private async void testcreateaudioAsync6() //10시방향
    {

        if (SmartSecurity.GetInt("ttsindex6") == 1)
        {
            SmartSecurity.SetInt("ttsindex6", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            //Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    private async void testcreateaudioAsync7() //12시방향
    {

        if (SmartSecurity.GetInt("ttsindex7") == 1)
        {
            SmartSecurity.SetInt("ttsindex7", 2);
            var str = await testttspostAsync(tts);

            GetContent info = JsonUtility.FromJson<GetContent>(str);

            var bytes = Convert.FromBase64String(info.audioContent);
            var f = testConvertByteFloat(bytes);

            AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
            audioClip.SetData(f, 0);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(DestroyClipAfterPlay(audioClip, audioSource));
            //Destroy(audioClip);
            // DataController.Instance.ttsindex = 1;

        }
    }

    public void testpositioninit(string text)
    {
        if (SmartSecurity.GetInt("ttsindex") == 0)
        {
            SmartSecurity.SetInt("ttsindex", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);

            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit1(string text)
    {
        if (SmartSecurity.GetInt("ttsindex1") == 0)
        {
            SmartSecurity.SetInt("ttsindex1", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit2(string text)
    {
        if (SmartSecurity.GetInt("ttsindex2") == 0)
        {
            SmartSecurity.SetInt("ttsindex2", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit3(string text)
    {
        if (SmartSecurity.GetInt("ttsindex3") == 0)
        {
            SmartSecurity.SetInt("ttsindex3", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit4(string text)
    {
        if (SmartSecurity.GetInt("ttsindex4") == 0)
        {
            SmartSecurity.SetInt("ttsindex4", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit5(string text)
    {
        if (SmartSecurity.GetInt("ttsindex5") == 0)
        {
            SmartSecurity.SetInt("ttsindex5", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit6(string text)
    {
        if (SmartSecurity.GetInt("ttsindex6") == 0)
        {
            SmartSecurity.SetInt("ttsindex6", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

    }
    public void testpositioninit7(string text)
    {
        if (SmartSecurity.GetInt("ttsindex7") == 0)
        {
            SmartSecurity.SetInt("ttsindex7", 1);
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
            //sa.speakingRate = SmartSecurity.GetFloat("ttsspeedfloat");;
            sa.speakingRate = Mathf.Clamp(SmartSecurity.GetFloat("ttsspeedfloat"), 0.5f, 2.0f);
            sa.pitch = 1;
            sa.volumeGainDb = 0;
            tts.audioConfig = sa;
        }

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
    public void testttsTt(string text)
    {
        testpositioninit(text);
        testcreateaudioAsync();
        text = "";
    }
    public void testttsTt1(string text)
    {
        testpositioninit1(text);
        testcreateaudioAsync1();
        text = "";
    }
    public void testttsTt2(string text)
    {
        testpositioninit2(text);
        testcreateaudioAsync2();
        text = "";
    }
    public void testttsTt3(string text)
    {
        testpositioninit3(text);
        testcreateaudioAsync3();
        text = "";
    }
    public void testttsTt4(string text)
    {
        testpositioninit4(text);
        testcreateaudioAsync4();
        text = "";
    }
    public void testttsTt5(string text)
    {
        testpositioninit5(text);
        testcreateaudioAsync5();
        text = "";
    }
    public void testttsTt6(string text)
    {
        testpositioninit6(text);
        testcreateaudioAsync6();
        text = "";
    }
    public void testttsTt7(string text)
    {
        testpositioninit7(text);
        testcreateaudioAsync7();
        text = "";
    }
    public void alltts(string text)
    {
        allpositioninit(text);
        allcreateaudioAsync();
        text = "";
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
