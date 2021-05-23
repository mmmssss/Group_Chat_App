using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DayElementScript : MonoBehaviour
{

    public Text SchejuleText1;
    public Text SchejuleText2;
    public Text SchejuleText3;
    public Text dayText;
    private DateTime dateTime;
    // Use this for initialization

    const string saveKey = "_UserData_";
    private UserData userData;
    private List<KeyAndValue> listData;

    private void Start()
    {

        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        listData = userData.groupList;

    }

    public void OnClickButton()
    {
        SchejuleScene.date = dateTime.ToString("yyyyMMdd");

        if (listData != null)
        {
            List<string[]> detailList = new List<string[]>();
            foreach (KeyAndValue keyAndValue in listData)
            {
                if (keyAndValue.schejule == dateTime.ToString("yyyy年MM月dd日"))
                {
                    if (keyAndValue.groupFlag)
                    {
                        detailList.Add(new string[] { keyAndValue.hour.ToString() + "時" + keyAndValue.minute.ToString() + "分", keyAndValue.title + "(グループ)", keyAndValue.schejule });
                    }
                    else
                    {
                        detailList.Add(new string[] { keyAndValue.hour.ToString() + "時" + keyAndValue.minute.ToString() + "分", keyAndValue.title, keyAndValue.schejule });
                    }
                }
            }

            SchejuleScene.SetDetail(detailList);

        }
    }

    public void SetText(string s)
    {
        dayText.text = s;

    }

    public void SetColor(Color c)
    {
        dayText.color = c;
    }

    public void SetDate(DateTime dt)
    {
        dateTime = dt;
    }

    public void SetSchejule(DateTime eleDay,string[] strings)
    {
        SchejuleText1.text = strings[0];
        SchejuleText2.text = strings[1];
        SchejuleText3.text = strings[2];
    }
}
