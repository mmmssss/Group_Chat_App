using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeNowScript : MonoBehaviour
{

    private Text ClockText;

    // Use this for initialization
    void Start()
    {
        ClockText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ClockText.text = DateTime.Now.ToLongTimeString();
       // ClockText.text = DateTime.Now.ToLongTimeString();
    }
}