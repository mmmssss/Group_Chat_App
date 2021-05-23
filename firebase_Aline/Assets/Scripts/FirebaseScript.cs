using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseScript : MonoBehaviour
{

    //public DatabaseReference groupMessagesDatabase;
    public static DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    public static DatabaseReference GetFirebaseDatabaseReference() {
        return reference;
    }

}
