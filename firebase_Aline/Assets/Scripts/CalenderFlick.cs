﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalenderFlick : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Flick();
    }

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
            case "right":
                //右フリックされた時の処理
                ChangeScene.ToStaticAlarmListScene();
                break;
            case "left":

                //左フリックされた時の処理
                ChangeScene.ToStaticShareSchejuleScene();
                break;
        }
    }
}
