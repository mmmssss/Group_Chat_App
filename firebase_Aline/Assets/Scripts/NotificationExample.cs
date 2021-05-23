using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Notifications.Android;

namespace Assets.Scripts
{
    public class NotificationExample : MonoBehaviour
    {
        //各種定義
        public InputField nameField;
        public HourScroll hourScroll;
        public MinuteScroll minuteScroll;
        public Toggle toggle;
        public Toggle noLoopToggle;
        public Toggle everydayToggle;
        public Toggle mondayToggle;
        public Toggle tuesdayToggle;
        public Toggle wednesdayToggle;
        public Toggle thursdayToggle;
        public Toggle fridayToggle;
        public Toggle satadayToggle;
        public Toggle sundayToggle;

        public AlarmList alarmlist;


        public Text timeField;
        public Text nameText;

        private const string schejuleKey = "_schejuleData_";
        private string m_channelId = "sharealarm";
        public SchejuleData schejuleData;


        void Awake()
        {
            originalElement.gameObject.SetActive(false);
        }

        void Start()
        {
            //キーに対する値を取得する
            string json = PlayerPrefs.GetString(schejuleKey, "");
            //jsonからオブジェクトに変換する
            //ジェネリックで型パラメータ渡す版
            schejuleData = JsonUtility.FromJson<SchejuleData>(json);
        }

        public void Save()
        {

            if (schejuleData == null)
            {
                schejuleData = new SchejuleData()
                {
                    dateList = new List<Date>(),
                };
            }

            int year, month, day;
            bool[] date = new bool[7];
            year = DateTime.Now.Year;
            month = DateTime.Now.Month;
            day = DateTime.Now.Day;
            //数字からなる文字列を数値に変換。
            //変換に成功した場合、true、失敗した場合、falseを返す
            //   if(int.TryParse(yearField.text,out year) && int.TryParse(monthField.text,out month) && int.TryParse(dayField.text,out day)){
            int hour = hourScroll.num;
            int minute = minuteScroll.num * 5;

            nameField = nameField.GetComponent<InputField>();
            string str = nameField.text;
            //DateTimeオブジェクトを作成
            DateTime SchejuleDateTime = new DateTime(year, month, day, hour, minute, 0);

            //数値を文字に変換→文字を数値に変換。
            int id = int.Parse((year % 100).ToString() + month.ToString() + day.ToString() + hour.ToString() + minute.ToString());
            int id2 = int.Parse((DateTime.Now.Year % 100).ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString());
            if (id < id2)
            {
                day += 1;
            }
            DateTime dt = new DateTime(year, month, day, hour, minute, 0);
            //曜日を取得する。
            DayOfWeek week = dt.DayOfWeek;
            //Int32型にキャストする。
            int weekNumber = (int)dt.DayOfWeek;

            if (sundayToggle.isOn)
            {
                date[0] = true;
            }
            if (mondayToggle.isOn)
            {
                date[1] = true;
            }
            if (tuesdayToggle.isOn)
            {
                date[2] = true;
            }
            if (wednesdayToggle.isOn)
            {
                date[3] = true;
            }
            if (thursdayToggle.isOn)
            {
                date[4] = true;
            }
            if (fridayToggle.isOn)
            {
                date[5] = true;
            }
            if (satadayToggle.isOn)
            {
                date[6] = true;
            }

            if (everydayToggle.isOn)
            {

            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    if (date[weekNumber % 7])
                    {
                        day += i;
                        break;
                    }
                    weekNumber++;
                }

            }


            Debug.Log(alarmlist.j);
            Debug.Log(alarmlist.clist);
            int lk = alarmlist.j;
            if (alarmlist.clist)
            {
                if (hour != 0)
                {
                    alarmlist.hour = hour;
                }
                if (minute != 0)
                {
                    alarmlist.minute = minute;
                }
                schejuleData.dateList.RemoveAt(lk);
                schejuleData.dateList.Insert(lk, new Date
                {
                    snooze = toggle.isOn,
                    nothing = noLoopToggle.isOn,
                    everyday = everydayToggle.isOn,
                    monday = mondayToggle.isOn,
                    tuesday = tuesdayToggle.isOn,
                    wednesday = wednesdayToggle.isOn,
                    thursday = thursdayToggle.isOn,
                    friday = fridayToggle.isOn,
                    sataday = satadayToggle.isOn,
                    sunday = sundayToggle.isOn,
                    id = id,
                    year = year,
                    month = month,
                    day = day,
                    hour = alarmlist.hour,
                    minute = alarmlist.minute,
                    second = 0,
                    namestr = str
                });
            }
            else
            {
                schejuleData.dateList.Add(new Date
                {
                    snooze = toggle.isOn,
                    nothing = noLoopToggle.isOn,
                    everyday = everydayToggle.isOn,
                    monday = mondayToggle.isOn,
                    tuesday = tuesdayToggle.isOn,
                    wednesday = wednesdayToggle.isOn,
                    thursday = thursdayToggle.isOn,
                    friday = fridayToggle.isOn,
                    sataday = satadayToggle.isOn,
                    sunday = sundayToggle.isOn,
                    id = id,
                    year = year,
                    month = month,
                    day = day,
                    hour = hour,
                    minute = minute,
                    second = 0,
                    namestr = str
                });
            }
            //オブジェクトをjsonに変換
            string json = JsonUtility.ToJson(schejuleData);
            PlayerPrefs.SetString(schejuleKey, json);
            SchejuleSimple(SchejuleDateTime, id, toggle.isOn);
        }



        public void SchejuleSimple(DateTime SchejuleDateTime, int id, bool snooze)
        {

            // 通知を送信する
            var n = new AndroidNotification
            {
                Title = "予定の時刻になりました。",
                Text = "タップしてアプリを起動してください。",
                SmallIcon = "icon_0",
                LargeIcon = "icon_1",
                FireTime = SchejuleDateTime, // 10 秒後に通知
            };

            int identifier = AndroidNotificationCenter.SendNotification(n, m_channelId);

            foreach (Date date in schejuleData.dateList) {
                if (date.id == id) {
                    id = identifier;
                }
            }
           
        }
        //class AlarmList

        [SerializeField] RectTransform content = null;
        [SerializeField] RectTransform originalElement = null;
        [SerializeField] Text elementOriginalText = null;
        [SerializeField] Text alarmDayText = null;

        public void listUpdate(int arrayNumber, int id, bool snooze)
        {
            schejuleData.dateList.RemoveAt(arrayNumber);
            string json = JsonUtility.ToJson(schejuleData);
            PlayerPrefs.SetString(schejuleKey, json);
            AndroidNotificationCenter.CancelScheduledNotification(id);
            AndroidNotificationCenter.CancelNotification(id);
            SceneManager.LoadScene("AlarmListScene");
        }


    }



}






