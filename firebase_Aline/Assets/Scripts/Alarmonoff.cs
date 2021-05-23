/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alameonoff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Alarmonoff : MonoBehaviour
{

    public Sprite spriteMae;
    public Sprite spriteAto;

    private bool onoffFlg = false;

    public void changeSprite()
    {

        if (!onoffFlg)
        {
            this.gameObject.GetComponent<Image>().sprite = spriteAto;
            onoffFlg = true;
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = spriteMae;
            onoffFlg = false;
        }

    }

}