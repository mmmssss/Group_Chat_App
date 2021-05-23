using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class AlarmCheck : MonoBehaviour
{

    const string saveKey = "_UserData_";
    private static UserData userData;

    private static int year, month, day, hour, minute;

    // Start is called before the first frame update


    public static void ToGameCheck()
    {
        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));

        Debug.Log("alarmCheck");

        if (userData != null)
        {
            if (userData.groupList != null)
            {
                if (userData.groupList.Count() > 0)
                {
                    //List<int> intList = new List<int>();
                    DateTime nowDateTime = DateTime.Now;

                    foreach (KeyAndValue keyAndValue in userData.groupList) {

                        if (keyAndValue.alarm)
                        {
                            int schejule = keyAndValue.id;

                            year = int.Parse(schejule.ToString().Substring(0, 2)) + 2000;
                            month = int.Parse(schejule.ToString().Substring(2, 2));
                            day = int.Parse(schejule.ToString().Substring(4, 2));
                            hour = int.Parse(schejule.ToString().Substring(6, 2));
                            minute = int.Parse(schejule.ToString().Substring(8, 2));

                            DateTime lastDateTime = new DateTime(year, month, day, hour, minute, 0);

                            if (nowDateTime > lastDateTime && nowDateTime - lastDateTime < TimeSpan.FromMinutes(30)) {
                                Debug.Log("ゲーム遷移");
                                keyAndValue.alarm = false;
                                //userData.groupList.RemoveAt(elementCount);
                                PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
                                ChangeScene.ToStaticGameScene();
                            }
                        }
                    }
                    //intList.Sort();

                    ////int elementCount = 0;

                    //foreach (KeyAndValue keyAndValue in userData.groupList)
                    //{
                    //    if (intList[0] == keyAndValue.id) {
                    //        schejule = intList[0];

                    //        break;
                    //    }
                    //    //elementCount++;
                    //}
                    

                    //year = int.Parse(schejule.ToString().Substring(0, 2)) + 2000;
                    //month = int.Parse(schejule.ToString().Substring(2, 2));
                    //day = int.Parse(schejule.ToString().Substring(4, 2));
                    //hour = int.Parse(schejule.ToString().Substring(6, 2));
                    //minute = int.Parse(schejule.ToString().Substring(8, 2));

                    //DateTime lastDateTime = new DateTime(year, month, day, hour, minute, 0);


                    //if (nowDateTime > lastDateTime && nowDateTime - lastDateTime < TimeSpan.FromMinutes(30))
                    //{
                    //    Debug.Log("ゲーム遷移");
                    //    //userData.groupList.RemoveAt(elementCount);
                    //    PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
                    //    ChangeScene.ToStaticGameScene();
                    //}
                }
            }
        }
    }




}
