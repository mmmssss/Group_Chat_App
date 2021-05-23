using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CalenderManager : MonoBehaviour
{


    const string saveKey = "_UserData_";
    private UserData userData;
    private List<KeyAndValue> listData;

    private DateTime nowMonth;
    private DayElementScript[] elements = new DayElementScript[42];
    public GameObject dayElement, childObj;
    public Text yearText, monthText;
    private int placeOfDay = -1;

    void Awake()
    {
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        listData = userData.groupList;

        nowMonth = DateTime.Today;
        for (int i = 0; i < 42; i++)
        {
            GameObject obj = (GameObject)Instantiate(dayElement);
            obj.transform.SetParent(childObj.transform);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            elements[i] = obj.GetComponent<DayElementScript>();
        }
        SetDays();
    }

    void Start()
    {
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        listData = userData.groupList;
    }

    private void SetDays()
    {
        var firstDay = new DateTime(nowMonth.Year, nowMonth.Month, 1);
        yearText.text = firstDay.Year.ToString();
        yearText.color = new Color(0.9f, 0.9f, 0.9f);
        monthText.text = firstDay.Month.ToString() + "月";
        int first = (int)firstDay.DayOfWeek;
        for (int i = 0; i < 42; i++)
        {
            DateTime eleDay = firstDay.AddDays(i - first);
            elements[i].SetText(eleDay.Day.ToString());
            Color color;
            if (eleDay.Date == DateTime.Now.Date)
            {
                elements[i].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
                placeOfDay = i;
            }
            else if (placeOfDay == i)
            {
                elements[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
            if (i % 7 == 0)
            {
                color = Color.red;
            }
            else if (i % 7 == 6)
            {
                color = Color.blue;
            }
            else
            {
                color = new Color(0, 0, 0);
            }
            if (eleDay.Month != firstDay.Month)
            {
                elements[i].SetColor(new Color(color.r, color.g, color.b, 0.5f));
            }
            else
            {
                elements[i].SetColor(color);
            }
            elements[i].SetDate(eleDay);

            if (listData != null)
            {

                string[] strings = new string[] { "", "", "" };
                int count = 0;

                foreach (KeyAndValue keyAndValue in listData)
                {
                    if (count < 3)
                    {
                        if (keyAndValue.schejule == eleDay.ToString("yyyy年MM月dd日"))
                        {
                            if (keyAndValue.groupFlag)
                            {
                                strings[count] = keyAndValue.title;
                            }
                            else
                            {
                                strings[count] = keyAndValue.title;
                            }
                            count++;
                        }
                        
                    }
                    else {
                        break;
                    }
                }
                elements[i].SetSchejule(eleDay,strings);

            }
        }
    }

    public void OnClickNext()
    {
        nowMonth = nowMonth.AddMonths(1);
        SetDays();
    }

    public void OnClickPrev()
    {
        nowMonth = nowMonth.AddMonths(-1);
        SetDays();
    }
}
