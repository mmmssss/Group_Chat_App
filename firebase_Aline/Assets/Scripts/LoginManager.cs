using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;

public class LoginManager : MonoBehaviour
{

    public InputField mailAddressInputField1 = null;
    public InputField passWordField1 = null;
    public InputField mailAddressInputField2 = null;
    public InputField passWordField2 = null;
    private UserData userData;

    private static Firebase.Auth.FirebaseAuth auth;
    private const string saveKey = "_UserData_";
    // Start is called before the first frame update
    void Start()
    {
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://sharealarm-f1da1.firebaseio.com/");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        userData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString(saveKey));
        if (userData != null)
        {
            if (userData.email != null && userData.passWord != null)
            {
                LoginUser(userData.email, userData.passWord);
            }
        }
        
    }

    public void OnCreateUserButton()
    {
        string mailAddress = mailAddressInputField1.text;
        string passWord = passWordField1.text;
        LoginManager.CreateUser(mailAddress, passWord);
    }

    public void OnLoginUserButton()
    {
        string mailAddress = mailAddressInputField2.text;
        string passWord = passWordField2.text;

        LoginManager.LoginUser(mailAddress, passWord);
    }

    public static void CreateUser(string email, string passWord)
    {

        auth.CreateUserWithEmailAndPasswordAsync(email, passWord).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
        });

        LoginManager.UserInit("ゲスト", email, passWord);
        ChangeScene.ToStaticSettingScene();
    }

    public static void LoginUser(string email, string passWord)
    {
        auth.SignInWithEmailAndPasswordAsync(email, passWord).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
        ChangeScene.ToStaticCalenderScene();
    }

    public static void UserInit(string name, string email, string passWord)
    {
        UserData userData = new UserData()
        {
            name = name,
            email = email,
            passWord = passWord,
        };
        PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(userData));
    }

}
