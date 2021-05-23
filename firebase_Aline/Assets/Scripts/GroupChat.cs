using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;

public class GroupChat : MonoBehaviour
{

    private string m_channelId = "sharealarm";
    public HourScroll hourScroll = null;
    public MinuteScroll minuteScroll = null;
    public static int identifier;
    public static int hour, minute;
    public int year, month, day;


    // 要素を追加するコンテント
    [SerializeField] RectTransform ChatContent = null;
    [SerializeField] RectTransform MemberContent = null;

    // 生成する要素
    [SerializeField] RectTransform ChatElement = null;
    [SerializeField] Text ChatText = null;
    [SerializeField] Text nameText = null;
    [SerializeField] Text titleNameText = null;
    [SerializeField] Text schejuleText = null;
    [SerializeField] Text alarmText = null;

    [SerializeField] RectTransform MemberElement = null;
    [SerializeField] Text MemberText = null;

    [SerializeField] RectTransform WakeupImage = null;

    // テキスト入力フィールド
    [SerializeField] InputField inputTextField = null;

    [SerializeField] GameObject MemberPanel = null;
    [SerializeField] GameObject MemberListOpen = null;
    [SerializeField] GameObject MemberListClose = null;

    [SerializeField] GameObject AlarmPanel = null;

    public Sprite _on;
    public Sprite _off;

    private const string saveKey = "_UserData_";
    private UserData userData;
    private DateTime dateTime;

    public static string RoomIDPass;
    private static string titleName = "";
    private static string schejule = "";
    private List<Dictionary<string, string>> members = new List<Dictionary<string, string>>();
    private List<Dictionary<string, string>> messages = new List<Dictionary<string, string>>();
    private List<Dictionary<string, string>> newMessages = new List<Dictionary<string, string>>();
    private bool messageFlag;
    private bool memberFlag;
    private DatabaseReference reference = FirebaseScript.GetFirebaseDatabaseReference();


    void Start()
    {

        // 通知用のチャンネルを作成する
        var c = new AndroidNotificationChannel
        {
            Id = m_channelId,
            Name = "予定の時刻になりました。",
            Importance = Importance.High,
            Description = "タップしてアプリを起動してください。",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);

        ChatElement.gameObject.SetActive(false);
        MemberListClose.SetActive(false);
        MemberPanel.SetActive(false);
        AlarmPanel.SetActive(false);

        MemberElement.gameObject.SetActive(false);
        messageFlag = false;
        memberFlag = false;

        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));

        FirebaseDatabase.DefaultInstance.GetReference("chats").Child(RoomIDPass).Child("title").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("not found users");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                titleName = (string)snapShot.Value;
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("chats").Child(RoomIDPass).Child("schejule").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("not found users");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapShot = task.Result;
                schejule = (string)snapShot.Value;
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("members").Child(RoomIDPass).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("not found users");
            }
            else if (task.IsCompleted)
            {

                IEnumerator<DataSnapshot> result = task.Result.Children.GetEnumerator();
                while (result.MoveNext())
                {
                    DataSnapshot data = result.Current;

                    members.Add(new Dictionary<string, string>(){
                        {"name",(string)data.Child("name").Value},
                        {"wakeup",(string)data.Child("wakeup").Value},
                    });
                }
                memberFlag = true;
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("messages").Child(RoomIDPass).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("not found users");
            }
            else if (task.IsCompleted)
            {

                IEnumerator<DataSnapshot> result = task.Result.Children.GetEnumerator();
                while (result.MoveNext())
                {
                    DataSnapshot data = result.Current;

                    messages.Add(new Dictionary<string, string>(){
                        {"message",(string)data.Child("message").Value},
                        {"name",(string)data.Child("name").Value},
                    });
                }
                messageFlag = true;
            }
        });

    }

    private void MessageUpdate()
    {
        FirebaseDatabase.DefaultInstance.GetReference("messages").Child(RoomIDPass).ChildAdded += (sender, e) =>
        {
            newMessages.Add(new Dictionary<string, string>(){
                {"message",(string)e.Snapshot.Child("message").Value},
                {"name",(string)e.Snapshot.Child("name").Value},
            });
        };
    }




    void Update()
    {

        if (titleName != "" && schejule != "")
        {
            year = int.Parse(schejule.Substring(0, 4));
            month = int.Parse(schejule.Substring(5, 2));
            day = int.Parse(schejule.Substring(8, 2));
            dateTime = new DateTime(year, month, day, hour, minute, 0);

            titleNameText.text = titleName;
            schejuleText.text = schejule;
            alarmText.text = dateTime.ToString("HH時mm分");

            titleName = "";
            schejule = "";
        }
        if (messageFlag && memberFlag)
        {
            Debug.Log("AddMember");
            foreach (Dictionary<string, string> message in messages)
            {
                CreateTextBox(message["message"], message["name"]);
            }
            foreach (Dictionary<string, string> member in members)
            {
                AddMember(member["name"], member["wakeup"]);
            }
            MessageUpdate();
            messageFlag = false;
            memberFlag = false;
        }
        if (newMessages.Count > messages.Count)
        {
            CreateTextBox(newMessages[newMessages.Count - 1]["message"], newMessages[newMessages.Count - 1]["name"]);
            messages.Add(new Dictionary<string, string>(){
                {"message",newMessages[newMessages.Count-1] ["message"]},
                {"name",newMessages[newMessages.Count-1]["name"]},
            });
        }
    }

    public void OnSendButton()
    {
        // 入力が空ではない場合処理
        if (!string.IsNullOrEmpty(inputTextField.text.Trim()) && inputTextField.text != "")
        {
            messages.Add(
                new Dictionary<string, string>(){
                        {"name",userData.name},
                        {"message",inputTextField.text},
                }
            );
            reference.Child("messages").Child(RoomIDPass).SetValueAsync(messages);
            CreateTextBox(inputTextField.text, userData.name);
            inputTextField.text = string.Empty;
        }
    }

    //アラームスクリプト

    public void OnAlarmPanel()
    {
        if (AlarmPanel.activeInHierarchy)
        {
            AlarmPanel.SetActive(false);
        }
        else
        {
            AlarmPanel.SetActive(true);
            MemberPanel.SetActive(false);
            MemberListOpen.SetActive(true);
            MemberListClose.SetActive(false);
        }
    }

    // ボタンが押されたら呼び出される関数
    public void OnClickButton()
    {

        int hour = hourScroll.num;
        int minute = minuteScroll.num * 5;

        int id = int.Parse((year % 100).ToString() + dateTime.ToString("MM" + "dd" + "HH" + "mm"));

        dateTime = new DateTime(year, month, day, hour, minute, 0);
        int newId = int.Parse((year % 100).ToString() + dateTime.ToString("MM" + "dd" + "HH" + "mm"));

        for (int i =0; i < userData.groupList.Count() ; i++) {
            if (userData.groupList[i].id == id) {
                userData.groupList[i].id = newId;
            }
        }

        alarmText.text = dateTime.ToString("HH時mm分");

        // 通知を送信する
        var n = new AndroidNotification
        {
            Title = "予定の時刻になりました。",
            Text = "タップしてアプリを起動してください。",
            SmallIcon = "icon_0",
            LargeIcon = "icon_1",
            FireTime = dateTime, // 10 秒後に通知
        };
  

        AlarmCancel(identifier);

        identifier = AndroidNotificationCenter.SendNotification(n, m_channelId);

        AlarmUpdate(identifier, hour, minute);

        AlarmPanel.SetActive(false);

    }

    public void AlarmUpdate(int newIdentifier, int newHour, int newMinute)
    {
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        List<KeyAndValue> listData = userData.groupList;

        for (int i = 0; i < listData.Count; i++)
        {
            if (RoomIDPass == listData[i].roomIDPass)
            {
                userData.groupList[i].hour = newHour;
                userData.groupList[i].minute = newMinute;
                userData.groupList[i].identifier = newIdentifier;
                userData.groupList[i].groupFlag = true;
            }
        }

        PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
    }

    public void AlarmCancel(int number)
    {
        CancelNotification(number);
    }

    public void CancelNotification(int notificationId)
    {
        AndroidNotificationCenter.CancelScheduledNotification(notificationId);
        AndroidNotificationCenter.CancelNotification(notificationId);
    }

    //チャットスクリプト

    public void CreateTextBox(string textMessage, string name)
    {


        // 入力フィールドは初期化する
        ChatText.text = textMessage;
        nameText.text = name;

        // content以下にoriginalElementを複製
        var element1 = GameObject.Instantiate<RectTransform>(ChatElement);
        if (userData.name == name)
        {
            element1.GetComponent<Image>().color = new Color(165.0f / 255.0f, 255.0f / 255.0f, 165.0f / 255.0f, 255.0f / 255.0f);
        }
        else
        {
            element1.GetComponent<Image>().color = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 255.0f / 255.0f);
        }
        element1.SetParent(ChatContent, false);
        element1.SetAsLastSibling();
        element1.gameObject.SetActive(true);

    }

    //　メンバースクリプト

    public void OnMemberPanel()
    {
        if (MemberPanel.activeInHierarchy)
        {

            MemberPanel.SetActive(false);
            MemberListOpen.SetActive(true);
            MemberListClose.SetActive(false);
        }
        else
        {
            AlarmPanel.SetActive(false);
            MemberPanel.SetActive(true);
            MemberListOpen.SetActive(false);
            MemberListClose.SetActive(true);
        }
    }

    public void AddMember(string name, string wakeup)
    {

        // 入力フィールドは初期化する
        MemberText.text = name;
        if (wakeup == "False")
        {
            WakeupImage.GetComponent<Image>().sprite = _off;
        }
        else
        {
            WakeupImage.GetComponent<Image>().sprite = _on;
        }
        // content以下にoriginalElementを複製
        var element2 = GameObject.Instantiate<RectTransform>(MemberElement);

        element2.SetParent(MemberContent, false);
        element2.SetAsLastSibling();
        element2.gameObject.SetActive(true);

    }
}
