using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System;

public class LoginTTS : MonoBehaviour
{
    private static LoginTTS instance;

    public static LoginTTS Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<LoginTTS>();
            if (instance != null) return instance;
            var container = new GameObject("LoginTTS");
            instance = container.AddComponent<LoginTTS>();
            return instance;
        }
    }
    public AudioSource audioSource;
    string testurl_ = "https://texttospeech.googleapis.com/v1/text:synthesize?key=AIzaSyCY59md0ekvhj0XShdlYphjOBGKA8FcB_g";
    SetTextToSpeech tts = new SetTextToSpeech();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


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
    private async void createaudioAsyncall() //전체  
    {
        var str = await testttspostAsync(tts);

        GetContent info = JsonUtility.FromJson<GetContent>(str);

        var bytes = Convert.FromBase64String(info.audioContent);
        var f = ConvertByteFloat(bytes);

        AudioClip audioClip = AudioClip.Create("audioContent", f.Length, 1, 44100, false);
        audioClip.SetData(f, 0);

        audioSource.PlayOneShot(audioClip);
        Destroy(audioClip); //profiler audio clip count 제거
    }
    public void positioninitall(string text)
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
        sa.speakingRate = 1.4f; //SmartSecurity.GetFloat("ttsspeedfloat");
        sa.pitch = 1;
        sa.volumeGainDb = 0;
        tts.audioConfig = sa;
    }
    private static float[] ConvertByteFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 2];

        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = BitConverter.ToInt16(array, i * 2) / 32768.0f;
        }

        return floatArr;
    }
    public  void logintts(string text)
    {
        positioninitall(text);
        createaudioAsyncall();
        text = "";
        SmartSecurity.SetInt("boxindex", 0);
    }

    public void alltts(string text)
    {
        positioninitall(text);
        createaudioAsyncall();
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
