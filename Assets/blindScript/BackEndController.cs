using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using LitJson;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.IO;
using Photon.Pun;

public class BackEndController : MonoBehaviour
{

    private static BackEndController instance;

    public static BackEndController Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<BackEndController>();
            if (instance != null) return instance;
            var container = new GameObject("BackEndController");
            return instance;
        }
    }
  
    public void quityes() //메인패널 데이터저장 후 앱종료
    {
        //리스트 초기화 objtts
        InsertPrivateData();
        InsertPublicItemData();
        InsertPublicLoginData();
        Application.Quit();
    }
    public void InsertPrivateData() //유저 데이터저장
    {
        Param param = new Param();

        var intData = "";

        if (SmartSecurity.GetString("int_smart_name") != "")
        {
            var int_Datas = (SmartSecurity.GetString("int_smart_name") + "ꂊ").Replace("》ꂊ", "").Replace("《", "")
                .Split('》');

            for (var i = 0; i < int_Datas.Length; i++)
            {
                intData += int_Datas[i] + "║" + SmartSecurity.GetInt(int_Datas[i]) + "◙";

            }
        }
        param.Add("intData", intData);

        var floatData = "";

        if (SmartSecurity.GetString("float_smart_name") != "")
        {
            var float_Datas = (SmartSecurity.GetString("float_smart_name") + "ꂊ").Replace("》ꂊ", "")
                .Replace("《", "")
                .Split('》');

            for (var i = 0; i < float_Datas.Length; i++)
            {
                floatData += float_Datas[i] + "║" + SmartSecurity.GetFloat(float_Datas[i]) + "◙";
            }
        }
        param.Add("floatData", floatData);

        var stringData = "";

        if (SmartSecurity.GetString("string_smart_name") != "")

        {
            var string_Datas = (SmartSecurity.GetString("string_smart_name") + "ꂊ").Replace("《inside_id》", "")
                .Replace("》ꂊ", "").Replace("《", "")
                .Split('》');

            for (var i = 0; i < string_Datas.Length; i++)
            {
                stringData += string_Datas[i] + "║" + SmartSecurity.GetString(string_Datas[i]) + "◙";
            }
        }
        param.Add("stringData", stringData);
        param.Add("myinsideid", SmartSecurity.GetString("insideid"));
        BackendReturnObject BRO = Backend.GameData.Insert("privatedata", param);
        SmartSecurity.SetString("myprivateindata", BRO.GetInDate());
    }
    public void InsertPublicItemData() //유저 아이템 정보 감정 머리 상의 하의
    {
        Param param = new Param();
        param.Add("wmsureindex", SmartSecurity.GetInt("wmsureindex"));  //성별체크.
        param.Add("itememotion", SmartSecurity.GetInt("itememotion"));
        param.Add("itemface", SmartSecurity.GetInt("itemface"));
        param.Add("itemtop", SmartSecurity.GetInt("itemtop"));
        param.Add("witememotion", SmartSecurity.GetInt("witememotion"));
        param.Add("witemtop", SmartSecurity.GetInt("witemtop"));
        param.Add("witemface", SmartSecurity.GetInt("witemface"));
        param.Add("iteminfoTt", SmartSecurity.GetString("iteminfoTt"));
        param.Add("mynickname", SmartSecurity.GetString("nickname"));
        param.Add("dotmcharimageindex", SmartSecurity.GetInt("dotmcharimageindex"));
        param.Add("dotwcharimageindex", SmartSecurity.GetInt("dotwcharimageindex"));
        BackendReturnObject bro = Backend.GameData.Insert("publicitemdata", param);
        SmartSecurity.SetString("mypublicitemdata", bro.GetInDate());
    }
    public void InsertPublicLoginData() //유저 아이디, 패스워드,닉네임
    {
        Param param = new Param();
        param.Add("loginid", SmartSecurity.GetString("loginid"));
        param.Add("loginpassword", SmartSecurity.GetString("loginpassword"));
        param.Add("loginnickname", SmartSecurity.GetString("loginnickname"));
        BackendReturnObject bro = Backend.GameData.Insert("publiclogindata", param);
        SmartSecurity.SetString("mypubliclogindata", bro.GetInDate());
    }
    public void Withdraw() //회원탈퇴.
    {
        BackendReturnObject bro = Backend.BMember.WithdrawAccount(); //즉시탈퇴,시간이 지나야 콘솔에서 유저정보 삭제됨.
        if (bro.IsSuccess())
        {
            //로그인 씬 이동.
            PlayerPrefs.DeleteAll(); //탈퇴시 데이터삭제.
            SceneManager.LoadScene(0);
            //탈퇴시 앱종료 1시간뒤 탈퇴확인 메세지.
        }
    }
    public void logout() //로그아웃.
    {
        BackendReturnObject bro = Backend.BMember.Logout();
        if(bro.IsSuccess())
        {
            SceneManager.LoadScene(0);
            SmartSecurity.SetInt("autoindex", 0);
            InsertPrivateData();
            InsertPublicItemData();
            InsertPublicLoginData();
        }
    }
}