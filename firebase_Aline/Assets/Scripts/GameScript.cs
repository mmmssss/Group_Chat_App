using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameScript : MonoBehaviour
{
    private static UserData userData;
    public static List<ListClass> memberList = new List<ListClass>();
    private const string saveKey = "_UserData_";

    // Start is called before the first frame update
    void Start()
    {
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));

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

    }

    public void Wakeup()
    {

        foreach (ListClass groupmember in memberList)
        {
            foreach (KeyAndValue keyValue in userData.groupList)
            {
                if (keyValue.roomIDPass == groupmember.roomIDPass)
                {
                    foreach (Dictionary<string, string> member in groupmember.list)
                    {
                        if (member["name"] == userData.name)
                        {
                            member["wakeup"] = "True";
                        }
                    }
                    FirebaseScript.reference.Child("members").Child(groupmember.roomIDPass).SetValueAsync(groupmember.list);
                }
            }
        }
    }

}
