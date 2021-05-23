using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Setting : MonoBehaviour
{
    public InputField inputNameField;
    public Text NameText;
    private static UserData userData;
    public static List<ListClass> memberList = new List<ListClass>();
    public static List<ListClass> messageList = new List<ListClass>();
    private const string saveKey = "_UserData_";
    private static string beforeName;
    private static string afterName;

    // Start is called before the first frame update



    void Awake()
    {

        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        beforeName = userData.name;
        NameText.text = userData.name;


        FirebaseDatabase.DefaultInstance.GetReference("members").GetValueAsync().ContinueWith(task =>
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

                    List<Dictionary<string, string>> members = new List<Dictionary<string, string>>();
                    for (int i = 0; i < (int)data.ChildrenCount; i++)
                    {
                        members.Add(new Dictionary<string, string> {
                          {"name", (string)data.Child(i.ToString()).Child("name").Value},
                          {"wakeup", (string)data.Child(i.ToString()).Child("wakeup").Value}
                      });

                    }
                    memberList.Add(new ListClass
                    {
                        roomIDPass = (string)data.Key,
                        list = members,
                    });
                }
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("messages").GetValueAsync().ContinueWith(task =>
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

                    List<Dictionary<string, string>> messages = new List<Dictionary<string, string>>();
                    for (int i = 0; i < (int)data.ChildrenCount; i++)
                    {
                        messages.Add(new Dictionary<string, string> {
                          {"name", (string)data.Child(i.ToString()).Child("name").Value},
                          {"message", (string)data.Child(i.ToString()).Child("message").Value}
                      });

                    }
                    messageList.Add(new ListClass
                    {
                        roomIDPass = (string)data.Key,
                        list = messages,
                    });
                }
            }
        });


    }

    public void OnChatsNameChange()
    {

        if (inputNameField.text != "")
        {
            afterName = inputNameField.text;
            NameText.text = afterName;

            foreach (ListClass groupmember in memberList)
            {
                foreach (KeyAndValue keyValue in userData.groupList)
                {
                    if (keyValue.roomIDPass == groupmember.roomIDPass)
                    {
                        foreach (Dictionary<string, string> member in groupmember.list)
                        {
                            if (member["name"] == beforeName)
                            {
                                member["name"] = afterName;
                            }
                        }
                        FirebaseScript.reference.Child("members").Child(groupmember.roomIDPass).SetValueAsync(groupmember.list);
                    }
                }
            }

            foreach (ListClass groupmember in messageList)
            {
                foreach (KeyAndValue keyValue in userData.groupList)
                {
                    if (keyValue.roomIDPass == groupmember.roomIDPass)
                    {
                        foreach (Dictionary<string, string> member in groupmember.list)
                        {
                            if (member["name"] == beforeName)
                            {
                                member["name"] = afterName;
                            }
                        }
                        FirebaseScript.reference.Child("messages").Child(groupmember.roomIDPass).SetValueAsync(groupmember.list);
                    }
                }
            }

            userData.name = afterName;
            PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
            ChangeScene.ToStaticCalenderScene();
        }
    }

    //  フリック操作

    public Vector3 touchStartPos;
    public Vector3 touchEndPos;
    public string Direction;

    void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z);
            GetDirection();
        }
    }

    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;
        //xがｙより絶対値が大きい時。
        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (30 < directionX)
            {
                //右向きにフリック
                Direction = "right";
            }
            else if (-30 > directionX)
            {
                //左向きにフリック
                Direction = "left";
            }
        }
        switch (Direction)
        {

            case "left":
                //左フリックされた時の処理
                ChangeScene.ToStaticCalenderScene();
                break;
        }
    }

}
