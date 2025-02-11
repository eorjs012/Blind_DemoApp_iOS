using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SmartSecurity : MonoBehaviour
{
    private static string privateKey = "2TOsh20fasSdD2gF0Sas7";

    // Add some values to this array before using SmartSecurity
    public static string[] keys =
    {
        "SFhsf08sf", "H2uf58smS", "ssf208kfn", "wnsdydWkd1", "gimoti252w"
    };

    public static string Md5(string strToEncrypt)
    {
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public static void SaveEncryption(string key, string type, string value)
    {
        int keyIndex = (int)Mathf.Floor(Random.value * keys.Length);
        string secretKey = keys[keyIndex];
        string check = Md5(type + "_" + privateKey + "_" + secretKey + "_" + value);
        PlayerPrefs.SetString(key + "_encryption_check", check);
        PlayerPrefs.SetInt(key + "_used_key", keyIndex);
    }

    public static bool CheckEncryption(string key, string type, string value)
    {
        int keyIndex = PlayerPrefs.GetInt(key + "_used_key");
        string secretKey = keys[keyIndex];
        string check = Md5(type + "_" + privateKey + "_" + secretKey + "_" + value);
        // if (!PlayerPrefs.HasKey(key + "_encryption_check")) return false;
        string storedCheck = PlayerPrefs.GetString(key + "_encryption_check");
        return storedCheck == check;
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        SavePoint("int", key);
        // SaveEncryption(key, "int", value.ToString());
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        SavePoint("float", key);
        // SaveEncryption(key, "float", Mathf.Floor(value * 1000).ToString());
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        SavePoint("string", key);
        // SaveEncryption(key, "string", value);
    }

    public static void SavePoint(string type, string hash)
    {
        var save_hash = PlayerPrefs.GetString(type + "_smart_name");
        if (!save_hash.Contains("《" + hash + "》"))
        {
            PlayerPrefs.SetString(type + "_smart_name", save_hash + "《" + hash + "》");
        }
    }
    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }
     
    public static float GetFloat(string key)
    {
        return GetFloat(key, 0f);
    }

    public static string GetString(string key)
    {
        return GetString(key, "");
    }

    public static int GetInt(string key, int defaultValue)
    {
        int value = PlayerPrefs.GetInt(key, defaultValue);
        // if (!key.Equals("skill_3_duration"))
        // {
        //     if (!CheckEncryption(key, "int", value.ToString()))
        //     {
        //         return defaultValue;
        //     }
        // }

        return value;
    }

    public static float GetFloat(string key, float defaultValue)
    {
        float value = PlayerPrefs.GetFloat(key, defaultValue);
        // if (!CheckEncryption(key, "float", Mathf.Floor(value * 1000).ToString())) return defaultValue;

        return value;
    }

    public static string GetString(string key, string defaultValue)
    {
        string value = PlayerPrefs.GetString(key, defaultValue);
        // if (!CheckEncryption(key, "string", value)) return defaultValue;

        return value;
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey(key + "_encryption_check");
        PlayerPrefs.DeleteKey(key + "_used_key");
    }
    
}