using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInputField : MonoBehaviour
{
    public InputField nameField;
    public Text nametext;
    // Start is called before the first frame update
    void Start()
    {
        nameField = nameField.GetComponent<InputField>();
        nametext = nametext.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputText()
    {
        //テキストにinputFieldの内容を反映
        nametext.text = nameField.text;

    }
}
