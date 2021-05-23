using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class ShareSchejuleScene : MonoBehaviour
{

    private const string saveKey = "_UserData_";

    [SerializeField] RectTransform ShareContent = null;
    [SerializeField] RectTransform ShareElement = null;
    [SerializeField] Text ShareGroupNameText = null;
    [SerializeField] Text ShareSchejuleText = null;
    [SerializeField] Text ShareRoomIDText = null;

    [SerializeField] GameObject JoinGroupPanel = null;
    [SerializeField] GameObject JoinGroupButton = null;
    [SerializeField] Text DeleteButtonText = null;
    [SerializeField] Text DeleteCancelButtonText = null;
    public InputField idField;
    public InputField passField;

    public Vector3 touchStartPos;
    public Vector3 touchEndPos;
    public string Direction;
    private UserData userData;
    private List<KeyAndValue> listData;
    private List<Dictionary<string, string>> members = new List<Dictionary<string, string>>();
    private bool oneplay;
    private string titleName = "";
    private string schejule = "";
    private string roomIDPass;
    private bool deleteFlag;

    public Sprite _on;
    public Sprite _off;


    // Use this for initialization

    void Update()
    {

        if (oneplay && titleName != "" && schejule != "")
        {
            members.Add(new Dictionary<string, string>(){
                  {"name",userData.name},
                  {"wakeup","False"},
                });
            FirebaseScript.reference.Child("members").Child(roomIDPass).SetValueAsync(members);
            Save(titleName, schejule);
            oneplay = false;
            titleName = "";
            ChangeScene.ToStaticShareSchejuleScene();
        }

    }

    void Start()
    {
        ShareElement.gameObject.SetActive(false);
        JoinGroupPanel.SetActive(false);
        deleteFlag = false;
        oneplay = false;
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        listData = userData.groupList;

        for (int i = 0; i < listData.Count; i++)
        {
            if (listData[i].groupFlag)
            {
                ShareGroupNameText.text = listData[i].title;
                ShareSchejuleText.text = listData[i].schejule;
                ShareRoomIDText.text = "ID：" + listData[i].roomIDPass.Substring(0, 5);
                string number = listData[i].roomIDPass;
                int identifier = listData[i].identifier;
                int hour = listData[i].hour;
                int minute = listData[i].minute;
                var element = GameObject.Instantiate<RectTransform>(ShareElement);
                element.GetComponent<Button>().onClick.AddListener(() => { OnClickButton(number, identifier, hour, minute); });
                element.SetParent(ShareContent, false);
                element.SetAsLastSibling();
                element.gameObject.SetActive(true);
            }
        }
    }

    public void Save(string groupTitle, string date)
    {

        roomIDPass = idField.text + passField.text;

        if (userData.groupList != null)
        {
            userData.groupList.Add(new KeyAndValue
            {
                title = groupTitle,
                schejule = date,
                roomIDPass = roomIDPass,
                identifier = 0,
                hour = 0,
                minute = 0,
                groupFlag = true,

            });
        }
        else
        {
            userData.groupList = new List<KeyAndValue>{
        new KeyAndValue{
          title = groupTitle,
          roomIDPass = roomIDPass,
          schejule = date,
          identifier = 0,
          hour = 0,
          minute = 0,
          groupFlag = true,
          },
      };
        }
        PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
    }

    public void OnClickButton(string number, int identifier, int hour, int minute)
    {
        if (deleteFlag)
        {
            int elementCount = 0;

            foreach (KeyAndValue keyValue in userData.groupList)
            {

                if (keyValue.roomIDPass == number)
                {
                    userData.groupList.RemoveAt(elementCount);
                    PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
                    break;
                }
                elementCount++;

            }
            ChangeScene.ToStaticShareSchejuleScene();

        }
        else
        {
            GroupChat.RoomIDPass = number;
            GroupChat.identifier = identifier;
            GroupChat.hour = hour;
            GroupChat.minute = minute;
            ChangeScene.ToStaticGroupScene();
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

    //グループに参加

    public void OnJoinGroupPanel()
    {

        if (JoinGroupPanel.activeInHierarchy)
        {
            JoinGroupPanel.SetActive(false);
            JoinGroupButton.GetComponent<Image>().sprite = _off;
        }
        else
        {
            JoinGroupPanel.SetActive(true);
            JoinGroupButton.GetComponent<Image>().sprite = _on;
        }
    }

    public void OnJoinGruop()
    {

        roomIDPass = idField.text + passField.text;

        FirebaseDatabase.DefaultInstance.GetReference("chats").Child(roomIDPass).Child("title").GetValueAsync().ContinueWith(task =>
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

        FirebaseDatabase.DefaultInstance.GetReference("chats").Child(roomIDPass).Child("schejule").GetValueAsync().ContinueWith(task =>
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

        FirebaseDatabase.DefaultInstance.GetReference("members").Child(roomIDPass).GetValueAsync().ContinueWith(task =>
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
                oneplay = true;
            }
        });

    }
}
