using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Apple.Accessibility;


public class CheckInputField : MonoBehaviour,IPointerDownHandler
{
    public InputField input;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            print("select");
            if (eventData.selectedObject.CompareTag("LoginIDInput"))
            {
                checkidinput();
            }
            else if (eventData.selectedObject.CompareTag("LoginPasswordInput"))
            {
                checkpasswordinput();
            }
            else if (eventData.selectedObject.CompareTag("SignUpIDInput"))
            {
                checkidinput();
            }
            else if (eventData.selectedObject.CompareTag("SignUpPasswordInput"))
            {
                checkpasswordinput();
            }
            else if (eventData.selectedObject.CompareTag("SignUpVerifyPasswordInput"))
            {
                checkverifypasswordinput();
            }
            else if (eventData.selectedObject.CompareTag("NickNameInput"))
            {
                checknicknameinput();
            }
        }
    }

    public void checkidinput()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (input.text != "")
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    LoginTTS.Instance.alltts("아이디" + input.text);
                }
            }
            else
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    LoginTTS.Instance.alltts("아이디를 입력해주세요");
                }
            }
        }
    }

    public void checkpasswordinput()
    {

        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (input.text != "")
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    int checkpassword = input.text.Length;
                    var password = new string('*', checkpassword);
                    LoginTTS.Instance.alltts("패스워드" + password);
                }
            }
            else
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    LoginTTS.Instance.alltts("패스워드를 입력해주세요");
                }
            }
        }
    }

    public void checkverifypasswordinput()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (input.text != "")
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    int checkpassword = input.text.Length;
                    var password = new string('*', checkpassword);
                    LoginTTS.Instance.alltts("verify패스워드" + password);
                }
            }
            else
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    LoginTTS.Instance.alltts("verify패스워드를 입력해주세요");
                }
            }
        }
       
    }
    public void checknicknameinput()
    {
        if (AccessibilitySettings.IsVoiceOverRunning == false)
        {
            if (input.text != "")
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    LoginTTS.Instance.alltts("닉네임" + input.text);
                }
            }
            else
            {
                if (SmartSecurity.GetInt("ttscontrolindex") == 1)
                {
                    LoginTTS.Instance.audioSource.Stop();
                    LoginTTS.Instance.alltts("닉네임을 입력해주세요");
                }

            }
        }
    }
}
