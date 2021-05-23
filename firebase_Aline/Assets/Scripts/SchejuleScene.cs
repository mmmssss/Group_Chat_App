using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;


public class SchejuleScene : MonoBehaviour
{

    private string m_channelId = "sharealarm";
    public HourScroll hourScroll = null;
    public MinuteScroll minuteScroll = null;
    public int identifier;

    public GameObject saveButtonText;
    public InputField groupName;
    public InputField passWord;
    public Text roomIDtext;
    public Toggle toggle;
    public Text DateText;
    public Text DebugText;

    [SerializeField] GameObject GroupPanel = null;

    const string saveKey = "_UserData_";
    private UserData userData;
    private string roomID;

    [SerializeField] RectTransform DetailContent = null;
    [SerializeField] RectTransform DetailElement = null;
    [SerializeField] Text DetailSchejuleTime = null;
    [SerializeField] Text DetailSchejuleTitle = null;
    public Text DetailTopText;
    public RectTransform DetailPanel;
    public static string date = "";

    public static List<string[]> detailList;
    [SerializeField] Text DeleteButtonText = null;
    [SerializeField] Text DeleteCancelButtonText = null;
    private bool deleteFlag;

    public string year = "";
    public string month = "";
    public string day = "";

    public void reset()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Update()
    {

        if (toggle.isOn)
        {
            GroupPanel.SetActive(true);
            if (groupName.text != "" && passWord.text != "")
            {
                saveButtonText.GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);

            }
            else
            {
                saveButtonText.GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 150.0f / 255.0f);
            }
        }
        else
        {
            GroupPanel.SetActive(false);
            if (groupName.text != "")
            {
                saveButtonText.GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            }
            else
            {
                saveButtonText.GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 150.0f / 255.0f);
            }
        }

        if (date != "")
        {
            year = date.Substring(0, 4);
            month = date.Substring(4, 2);
            day = date.Substring(6, 2);
            DateText.text = "日時： " + year + " 年 " + month + " 月 " + day + " 日 ";
            date = "";
        }

        if (detailList != null)
        {

            bool oneplay = false;

            foreach (Transform element in DetailContent)
            {
                if (oneplay)
                {
                    GameObject.Destroy(element.gameObject);
                }
                else
                {
                    oneplay = true;
                }
            }

            if (detailList.Count > 0)
            {
                DetailPanel.gameObject.SetActive(true);
            }
            else
            {
                DetailPanel.gameObject.SetActive(false);
            }

            foreach (string[] strings in detailList)
            {

                DetailSchejuleTitle.text = strings[1];
                DetailSchejuleTime.text = strings[0];
                DetailTopText.text = strings[2];
                var element = GameObject.Instantiate<RectTransform>(DetailElement);
                element.GetComponent<Button>().onClick.AddListener(() => { DeleteSchejule(strings[1].TrimEnd('(','グ','ル','ー','プ',')') + strings[2]); });
                element.SetParent(DetailContent, false);
                element.SetAsLastSibling();
                element.gameObject.SetActive(true);
            }

            detailList = null;
        }
    }


    // Start is called before the first frame update

    void Start()
    {
        DetailElement.gameObject.SetActive(false);
        DetailPanel.gameObject.SetActive(false);

        date = DateTime.Now.ToString("yyyyMMdd");
        //日時の取得
        groupName.text = "";
        passWord.text = "";

        roomID = UnityEngine.Random.Range(10000, 99999).ToString();
        roomIDtext.text = "ルームID：" + roomID;
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));

    }

    public void DeleteSchejule(string str) {

        if (deleteFlag)
        {
            int elementCount = 0;

            foreach (KeyAndValue keyValue in userData.groupList)
            {

                if ((keyValue.title + keyValue.schejule) == str)
                {
                    userData.groupList.RemoveAt(elementCount);
                    PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
                    break;
                }
                elementCount++;

            }
            ChangeScene.ToStaticCalenderScene();

        }
    }


    public void OnDeleteButton()
    {
        deleteFlag = true;
        DeleteButtonText.text = "選択";
        DeleteCancelButtonText.GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
    }

    public void OnDeleteCancelButton()
    {
        deleteFlag = false;
        DeleteButtonText.text = "削除";
        DeleteCancelButtonText.GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 150.0f / 255.0f);
    }


    public void Save()
    {
        if (groupName.text != "")
        {
            if (toggle.isOn)
            {
                if (passWord.text != "")
                {
                    int hour = hourScroll.num;
                    int minute = minuteScroll.num * 5;

                    DateTime SchejuleDateTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), hour, minute, 0);
                    int id = int.Parse(year.Substring(2, 2) + SchejuleDateTime.ToString("MM" + "dd" + "HH" + "mm"));

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


                    GroupData groupData = new GroupData
                    {
                        roomIDPass = roomID + passWord.text,
                        chats = new Dictionary<string, string>(){
                            {"title",groupName.text},
                            {"schejule",year + "年" + month + "月" + day + "日"},
                        },
                        members = new List<Dictionary<string, string>>{
                            new Dictionary<string, string>(){
                                {"name",userData.name},
                                {"wakeup","False"},
                            }
                        },
                    };


                    userData.groupList.Add(new KeyAndValue
                    {
                        title = groupName.text,
                        schejule = year + "年" + month + "月" + day + "日",
                        roomIDPass = roomID + passWord.text,
                        identifier = identifier,
                        hour = hour,
                        minute = minute,
                        groupFlag = true,
                        id = id,

                    });

                    writeGroupData(groupData);
                    PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
                    SlidePanel.OnStaticCloseTopPanel();
                    TextClear();
                }
            }
            else
            {
                int hour = hourScroll.num;
                int minute = minuteScroll.num * 5;

                DateTime SchejuleDateTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), hour, minute, 0);
                int id = int.Parse(year.Substring(2, 2) + SchejuleDateTime.ToString("MM" + "dd" + "HH" + "mm"));

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

                userData.groupList.Add(new KeyAndValue
                {
                    title = groupName.text,
                    schejule = year + "年" + month + "月" + day + "日",
                    roomIDPass = roomID + passWord.text,
                    identifier = identifier,
                    hour = hour,
                    minute = minute,
                    groupFlag = false,
                    id = id,

                });

                PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
                SlidePanel.OnStaticCloseTopPanel();
                TextClear();
            }

        }
    }

    public void TextClear()
    {
        Start();
    }


    private void writeGroupData(GroupData groupData)
    {
        FirebaseScript.reference.Child("chats").Child(groupData.roomIDPass).SetValueAsync(groupData.chats);
        FirebaseScript.reference.Child("members").Child(groupData.roomIDPass).SetValueAsync(groupData.members);
        FirebaseScript.reference.Child("messages").Child(groupData.roomIDPass).SetValueAsync(groupData.messages);
    }

    public static void SetDetail(List<string[]> newDetailList)
    {
        detailList = newDetailList;
    }

    public void CloseDetailPanel()
    {
        DetailPanel.gameObject.SetActive(false);
    }

}
