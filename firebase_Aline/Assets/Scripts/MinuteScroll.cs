using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinuteScroll : MonoBehaviour, IEndDragHandler
{
    public int num = 0;
    public float pos = 0;
    public void OnEndDrag(PointerEventData data)
    {
        float max = 11f; //要素数-1
        pos = Mathf.Clamp(this.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
        num = Mathf.RoundToInt(max - max * pos);
        this.GetComponent<ScrollRect>().verticalNormalizedPosition = Mathf.RoundToInt(max * pos) / max;
    }
}
