using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    private static FootStep instance;

    public static FootStep Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<FootStep>();
            if (instance != null) return instance;
            var container = new GameObject("FootStep");
            return instance;
        }
    }

    public AudioSource foot;
    public AudioClip footclip;

    // Start is called before the first frame update
    void Start()
    {
        foot = GetComponent<AudioSource>();
        foot.clip = footclip;
        foot.loop = false;
    }

}
