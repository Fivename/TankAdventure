using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    protected Slot[] slots;
    [HideInInspector]
    public CanvasGroup canvasGroup;
    public float targetAlpha = 0;
    private GameObject toolTip;
   // private PlayerController playerController;
    public virtual void Start()
    {
        toolTip = GameObject.Find("ToolTip");
        slots = GetComponentsInChildren<Slot>();
        canvasGroup = GetComponent<CanvasGroup>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {

        //if (canvasGroup.alpha != targetAlpha)
        //{
        //    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smooth * Time.deltaTime);
        //    if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < .1f)
        //    {

        if (targetAlpha == 1 && toolTip.activeInHierarchy == false)
        {
            toolTip.SetActive(true);
        }


        canvasGroup.alpha = targetAlpha;
        //    }
        //}
    }

    //通过Id存储物品
    public bool StoreItem(int id)
    {
        //bool flag = InventoryManager.Instance.IsPickedItem;
        Item item = InventoryManager.Instance.GetItemID(id);
        return StoreItem(item);
    }
    public bool StoreItem(Item item)
    {
        if (item == null) return false;
        if (item.Capacity ==1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.Log("没有空的物品槽");
                return false;
            }
            else
            {
                //调用那个空位置的物品槽来存物品
                slot.CreateItem(item);
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(item);
            //如果有相同类型的物品 直接存进去 ps: BUG maybe
            if(slot != null)
            {
                slot.CreateItem(item);
            }
            else
            {
                Slot slot2 = FindEmptySlot();
                if(slot2 != null)
                {
                    slot2.CreateItem(item);
                }
                else
                {
                    Debug.Log("没有空的");
                    return false;
                }
            }
        }
        return true;
    }
    //寻找空的物品栏
    public Slot FindEmptySlot()
    {
        //如果有空的物品槽 就返回那个物品槽
        foreach(Slot slot in slots)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }
    //寻找类型相同的物品槽
    public Slot FindSameIdSlot(Item item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.transform.childCount >= 1 && slot.GetItemId() == item.Id && slot.isFilled() == false)
            {
                return slot;
            }
        }
        return null;
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        targetAlpha = 0;
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        targetAlpha = 1;
    }
    /// <summary>
    /// 物品的显示和隐藏
    /// </summary>
    public void ItemShowAndHide()
    {
        if (targetAlpha == 0)
        {
            Show();
            toolTip.SetActive(true);
        }
        else
        {
            toolTip.SetActive(false);
            Hide();
        }
    }
}
