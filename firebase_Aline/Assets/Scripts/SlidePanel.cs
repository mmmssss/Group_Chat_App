using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidePanel : MonoBehaviour
{
    public RectTransform RightPanel;
    public RectTransform TopPanel;

    private static bool RightPanelOpenFlag = false;
    private static bool RightPanelCloseFlag = false;
    private int rightCount = 0;

    private static bool TopPanelOpenFlag = false;
    private static bool TopPanelCloseFlag = false;
    private int topCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (RightPanelOpenFlag)
        {
            RightPanel.localPosition = new Vector3(1080 - rightCount, 0, 0);
            rightCount = rightCount + 108;
            if (rightCount > 1080)
            {
                RightPanelOpenFlag = false;
                rightCount = 0;
            }
        }
        if (RightPanelCloseFlag)
        {
            RightPanel.localPosition = new Vector3(rightCount, 0, 0);
            rightCount = rightCount + 108;
            if (rightCount > 1080)
            {
                RightPanelCloseFlag = false;
                rightCount = 0;
            }
        }

        if (TopPanelOpenFlag)
        {
            TopPanel.localPosition = new Vector3(0, 1920 - topCount, 0);
            topCount = topCount + 192;
            if (topCount > 1920)
            {
                TopPanelOpenFlag = false;
                topCount = 0;
            }
        }
        if (TopPanelCloseFlag)
        {
            TopPanel.localPosition = new Vector3(0, topCount, 0);
            topCount = topCount + 192;
            if (topCount > 1920)
            {
                TopPanelCloseFlag = false;
                topCount = 0;
                ChangeScene.ToStaticCalenderScene();
            }
        }
    }

    public void OnOpenRightPanel()
    {
        RightPanelOpenFlag = true;
    }
    public void OnCloseRightPanel()
    {
        RightPanelCloseFlag = true;
    }

    public static void OnStaticOpenRightPanel()
    {
        RightPanelOpenFlag = true;
    }
    public static void OnStaticCloseRightPanel()
    {
        RightPanelCloseFlag = true;
    }


    public void OnOpenTopPanel()
    {
        TopPanelOpenFlag = true;
    }
    public void OnCloseTopPanel()
    {
        TopPanelCloseFlag = true;
    }

    public static void OnStaticOpenTopPanel()
    {
        TopPanelOpenFlag = true;
    }
    public static void OnStaticCloseTopPanel()
    {
        TopPanelCloseFlag = true;
    }
}
