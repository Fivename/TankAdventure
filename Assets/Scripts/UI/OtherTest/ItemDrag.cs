using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject StartFather;//一开始的父物体
    public GameObject InDragFather;//在抓的时候的父物体

    private void Start()
    {
        //初始化设置父物体为背景 否则抓取时物体会被遮挡（后生成的物体会遮挡先生成的）
        InDragFather = GameObject.Find("Canvas/PlayerKnapsack/Knapsack");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartFather = transform.parent.gameObject;
        //设置拖拽的物体是否能被射线检测
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetParent(InDragFather.transform);
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log("OnEndDray");
        Debug.Log(go);
        //目标位置是空格子 放入
        if (go != null && go.transform.tag == "Slot" && go.transform.childCount == 0)
        {
            transform.SetParent(go.transform);
            transform.position = go.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        //目标位置是物品 交换物品位置
        else if(go != null && go.transform.tag != "Slot" && go.transform.parent.tag == "ItemTemplate")
        {
            transform.SetParent(go.transform.parent.parent.transform);
            transform.position = go.transform.parent.parent.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            go.transform.parent.transform.SetParent(StartFather.transform);
            go.transform.parent.transform.position = StartFather.transform.position;
        }
        else
        {
            //目标位置检测不在正确位置 回原位
            transform.SetParent(StartFather.transform);
            transform.position = StartFather.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
