using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveandLoad : MonoBehaviour
{
    

    // Update is called once per frame
    private void OnApplicationFocus(bool hasFocus){
        if(hasFocus){
            AlarmCheck.ToGameCheck();
        }else{
            Debug.Log("Saved");
            PlayerPrefs.Save();
        }
    }

    private void OnApplicationPause(bool pauseStatus){
        if(pauseStatus){
            AlarmCheck.ToGameCheck();
        }
        else
        {
            Debug.Log("Saved");
            PlayerPrefs.Save();
        }
    }

    private void OnApplicationQuit(){
        Debug.Log("Saved");
        PlayerPrefs.Save();
    }
}
