using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class AlarmList : MonoBehaviour
    {
        // Start is called before the first frame update

        [SerializeField] RectTransform content = null;
        [SerializeField] RectTransform originalElement = null;
        [SerializeField] Text elementOriginalText = null;
        [SerializeField] Text alarmDayText = null;
        [SerializeField] Text nametext = null;
        [SerializeField] HourScroll hourscroll = null;
        [SerializeField] MinuteScroll minuteScroll = null;
        [SerializeField] InputField nameField = null;
        [SerializeField] Toggle noLoopToggle = null;
        [SerializeField] Toggle everydayToggle = null;
        [SerializeField] Toggle mondayToggle = null;
        [SerializeField] Toggle tuesdayToggle = null;
        [SerializeField] Toggle wednesdayToggle = null;
        [SerializeField] Toggle thursdayToggle = null;
        [SerializeField] Toggle fridayToggle = null;
        [SerializeField] Toggle satadayToggle = null;
        [SerializeField] Toggle sundayToggle = null;


        private const string schejuleKey = "_schejuleData_";
        private SchejuleData schejuleData;

        public int j;
        public int hour;
        public int minute;
        public bool clist;


        void Awake()
        {
            originalElement.gameObject.SetActive(false);
        }

        void Start()
        {
            UpdateAlarmList();
        }

        public void UpdateAlarmList()
        {
            //キーに対する値を取得する
            string json = PlayerPrefs.GetString(schejuleKey);
            //jsonからオブジェクトに変換する
            //ジェネリックで型パラメータ渡す版
            schejuleData = JsonUtility.FromJson<SchejuleData>(json);

            if (schejuleData == null)
            {
                schejuleData = new SchejuleData()
                {
                    dateList = new List<Date>(),
                };
            }

            if (schejuleData.dateList != null)
            {
                for (int i = 0; i < schejuleData.dateList.Count; i++)
                {
                    Date date = schejuleData.dateList[i];
                    DateTime dateTime = new DateTime(date.year, date.month, date.day, date.hour, date.minute, date.second);
                    int arrayNumber = i;
                    int id = date.id;
                    bool snooze = date.snooze;
                    nametext.text = date.namestr;
                    if (date.everyday)
                    {
                        alarmDayText.text = "毎日";
                    }
                    else if (!date.everyday)
                    {
                        alarmDayText.text = "";
                        ; if (date.monday)
                        {
                            alarmDayText.text = "月";
                        }
                        if (date.tuesday)
                        {
                            alarmDayText.text += "火";
                        }
                        if (date.wednesday)
                        {
                            alarmDayText.text += "水";
                        }
                        if (date.thursday)
                        {
                            alarmDayText.text += "木";
                        }
                        if (date.friday)
                        {
                            alarmDayText.text += "金";
                        }
                        if (date.sataday)
                        {
                            alarmDayText.text += "土";
                        }
                        if (date.sunday)
                        {
                            alarmDayText.text += "日";
                        }
                    }
                    else
                    {
                        alarmDayText.text = "";
                    }
                    // elementOriginalText.text = dateTime.ToString("yyyy年MM月dd日 HH時mm分ss秒");
                    elementOriginalText.text = dateTime.ToString("HH時mm分");
                    var element = GameObject.Instantiate<RectTransform>(originalElement);
                    element.GetComponent<Button>().onClick.AddListener(() => { EditList(arrayNumber, id, snooze); });
                    element.SetParent(content, false);
                    element.SetAsLastSibling();//一番最後の順番
                    element.gameObject.SetActive(true);

                }
            }
        }

        public void DeleteList()
        {
            int id = schejuleData.dateList[j].id;
            schejuleData.dateList.RemoveAt(j);
            string json = JsonUtility.ToJson(schejuleData);
            PlayerPrefs.SetString(schejuleKey, json);
            PlayerPrefs.Save();
            //NotificationManager.Cancel(id);
            SceneManager.LoadScene("AlarmListScene");
            j = -1;
            clist = false;
        }





        public void EditList(int arrayNumber, int id, bool snooze)
        {
            //schejuleData.dateList.RemoveAt(arrayNumber);
            //string json = JsonUtility.ToJson( schejuleData );
            //PlayerPrefs.SetString(schejuleKey, json);
            //PlayerPrefs.Save();
            //for(int i=0; i < 5; i++){
            //  NotificationManager.Cancel(id);
            //  id += 30;
            //}
            j = arrayNumber;
            clist = true;
            hour = schejuleData.dateList[arrayNumber].hour;
            minute = schejuleData.dateList[arrayNumber].minute;
            ScrollRect scrollRect = hourscroll.GetComponent<ScrollRect>();
            scrollRect.GetComponent<ScrollRect>().verticalNormalizedPosition = Mathf.RoundToInt(23f - schejuleData.dateList[arrayNumber].hour) / 23f;
            ScrollRect scrollRect2 = minuteScroll.GetComponent<ScrollRect>();
            scrollRect2.GetComponent<ScrollRect>().verticalNormalizedPosition = Mathf.RoundToInt(11f - schejuleData.dateList[arrayNumber].minute / 5) / 11f;
            nameField = nameField.GetComponent<InputField>();
            nameField.text = schejuleData.dateList[arrayNumber].namestr;
            noLoopToggle.isOn = false;
            everydayToggle.isOn = false;
            mondayToggle.isOn = false;
            tuesdayToggle.isOn = false;
            wednesdayToggle.isOn = false;
            tuesdayToggle.isOn = false;
            fridayToggle.isOn = false;
            satadayToggle.isOn = false;
            sundayToggle.isOn = false;
            if (schejuleData.dateList[arrayNumber].nothing)
            {
                noLoopToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].everyday)
            {
                everydayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].monday)
            {
                mondayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].tuesday)
            {
                tuesdayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].wednesday)
            {
                wednesdayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].thursday)
            {
                thursdayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].friday)
            {
                fridayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].sataday)
            {
                satadayToggle.isOn = true;
            }
            if (schejuleData.dateList[arrayNumber].sunday)
            {
                sundayToggle.isOn = true;
            }

            SlidePanel slidePanel = new SlidePanel();
            slidePanel.OnOpenRightPanel();
            //SceneManager.LoadScene("AlarmScene");
        }

        public void CanceltoAlarmScene()
        {
            j = -1;
            clist = false;
        }

    }
}
