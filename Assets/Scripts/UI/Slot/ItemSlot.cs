using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : Slot, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private new void Start()
    {
        base.Start();
        inventory = gameObject.transform.parent.parent.GetComponent<ItemPanel>();
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        ///右键拖动 道具/装备
        //手上有道具、手上没道具、栏里有道具、栏里没道具、道具是物品、道具是装备、左键还是右键
        //常用情况是玩家拖动、使用道具、先做手上没道具栏里有道具的左右操作

        //右键
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            
            //手上没东西
            if (InventoryManager.Instance.IsPickedItem == false)
            {
                //右键 槽里没东西 手上有 拿起来
                if (transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    InventoryManager.Instance.PickedItemUI(currentItemUI.item, currentItemUI.Amount);
                    Destroy(currentItemUI.gameObject);
                }
                //手槽都没 啥事没有
                else return;
            }
            //手上有东西
            else
            {
                //槽里有东西
                if(transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    //如果类别一样 一个个加
                    if (currentItemUI.item.Name == InventoryManager.Instance.PickedItem.item.Name)
                    {
                        if (currentItemUI.Amount < currentItemUI.item.Capacity)//如果容量大于物 并且 手上要有东西
                        {
                            InventoryManager.Instance.ReducePickedItem();
                            currentItemUI.AddAmount();
                        }
                        else return;
                    }
                    //类别不一样 交换
                    else
                    {
                        Item tempItem = currentItemUI.item;
                        int tempAmount = currentItemUI.Amount;
                        currentItemUI.SetItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                        InventoryManager.Instance.PickedItem.SetItem(tempItem, tempAmount);
                    }
                }
                //槽里没东西
                else
                {
                    CreateItem(InventoryManager.Instance.PickedItem.item);
                    InventoryManager.Instance.ReducePickedItem();
                }
            }
        }
        //左键
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //手上有东西
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                //槽里有东西
                if (transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    //类别一样 加满
                    if (InventoryManager.Instance.PickedItem.name == currentItemUI.item.Name)
                    {
                        //如果物品容量大于鼠标上和物品槽里的物品数之和则直接放下
                        if (currentItemUI.item.Capacity >= InventoryManager.Instance.PickedItem.Amount + currentItemUI.Amount)
                        {
                            currentItemUI.AddAmount(InventoryManager.Instance.PickedItem.Amount);
                            InventoryManager.Instance.ReduceAllPickedItem();
                        }
                        else//吧物品槽存满 然后鼠标上还有剩
                        {
                            int tempAmount = currentItemUI.item.Capacity - currentItemUI.Amount;
                            currentItemUI.AddAmount(tempAmount);
                            InventoryManager.Instance.ReducePickedItem(tempAmount);
                        }
                    }
                    //类别不一样 交换
                    else
                    {
                        Item tempItem = currentItemUI.item;
                        int tempAmount = currentItemUI.Amount;
                        currentItemUI.SetItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                        InventoryManager.Instance.PickedItem.SetItem(tempItem, tempAmount);
                    }
                }
                //槽里没东西
                else
                {
                    CreateItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.ReduceAllPickedItem();
                }
            }
            //手上没东西
            else
            {
                //槽里有东西 是消耗品就使用 是装备就装备
                if (transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    if (currentItemUI.item is Equipment || currentItemUI.item is Weapon)//如果当前物品为武器装备类型
                    {
                        CharacterPanel.Instance.PutOn(currentItemUI);//将物品放入装备槽中
                        InventoryManager.Instance.HideToolTipContent();//隐藏信息提示框
                        CharacterPanel.Instance.UpdatePropertyText();//更新人物属性
                    }
                    //使用消耗品
                    else if (currentItemUI.item is Consumable)
                    {
                        Consumable consumable = (Consumable)currentItemUI.item;
                        consumable.AddBuff(playerController);
                        currentItemUI.ReduceAmount();
                        int remainAmount = currentItemUI.Amount;
                        if (remainAmount <= 0)
                        {
                            Destroy(currentItemUI.gameObject);
                            InventoryManager.Instance.HideToolTipContent();
                        }
                        else
                        {
                            currentItemUI.SetAmount(remainAmount);
                        }
                    }
                }
                //手槽都没 return
                else
                {
                    return;
                }
            }
        }
    }

}
