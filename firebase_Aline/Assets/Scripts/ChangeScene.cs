using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void ToCalenderScene()
    {
        SceneManager.LoadScene("CalenderScene");
    }

    public void ToShareSchejuleScene()
    {
        SceneManager.LoadScene("ShareSchejuleScene");
    }

    public void ToAlarmListScene()
    {
        SceneManager.LoadScene("AlarmListScene");
    }

    public void ToGroupScene()
    {
        SceneManager.LoadScene("GroupScene");
    }

    public void ToSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }

    public static void ToStaticGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public static void ToStaticSettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }

    public static void ToStaticCalenderScene()
    {
        SceneManager.LoadScene("CalenderScene");
    }

    public static void ToStaticGroupScene()
    {
        SceneManager.LoadScene("GroupScene");
    }

    public static void ToStaticShareSchejuleScene()
    {
        SceneManager.LoadScene("ShareSchejuleScene");
    }

    public static void ToStaticAlarmListScene()
    {
        SceneManager.LoadScene("AlarmListScene");
    }

}
