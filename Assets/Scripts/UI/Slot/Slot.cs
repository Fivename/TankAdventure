using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public GameObject ItemPrefab;
    public PlayerController playerController;
    public Inventory inventory; 
    //private ToolTip toolTip = GameObject.FindObjectOfType<ToolTip>();
    //存储Item在物品槽中
    protected void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventory = gameObject.transform.parent.parent.GetComponent<KnapsackPanel>();
    }

    protected void Update()
    {
        
    }
    /// <summary>
    /// 把Item放在自身下面，如果自身一下已经有Item了，amount++，
    /// 如果没有，根据ItemPrefab 去实例化一个Item,放在下面
    /// </summary>
    /// <param name="item"></param>
    public void CreateItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemPrefabs = GameObject.Instantiate(ItemPrefab);
            itemPrefabs.transform.SetParent(transform);
            itemPrefabs.transform.localPosition = Vector3.zero;
            itemPrefabs.transform.localScale = Vector3.one;
            transform.GetChild(0).GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }

    public void CreateItem(Item item, int amount)
    {
        if (transform.childCount == 0)
        {
            GameObject itemPrefabs = GameObject.Instantiate(ItemPrefab);
            itemPrefabs.transform.SetParent(transform);
            itemPrefabs.transform.localPosition = Vector3.zero;
            itemPrefabs.transform.localScale = Vector3.one;
            transform.GetChild(0).GetComponent<ItemUI>().SetItem(item, amount);
        }
        else
        {
            Debug.LogWarning("有孩子了");
            return;
        }
    }
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().item.Id;
    }

    public bool isFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.item.Capacity;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            if(transform.childCount > 0 && inventory.canvasGroup.alpha == 1)
            {
                string content = transform.GetChild(0).GetComponent<ItemUI>().item.GetToolTipContent();
                InventoryManager.Instance.ShowToolTipContent(content);
            }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
            if(transform.childCount > 0)
            {
                InventoryManager.Instance.HideToolTipContent();
            }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)//点击鼠标右键
        {
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                //如果手上有东西 且槽有东西
                if (transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    //如果两个物体id相同
                    if (InventoryManager.Instance.PickedItem.item.Name == currentItemUI.item.Name)
                    {
                        if (currentItemUI.Amount < currentItemUI.item.Capacity)//如果容量大于物品 手上也要有东西
                        {
                            InventoryManager.Instance.ReducePickedItem();
                            currentItemUI.AddAmount();
                        }
                        else return;
                    }
                    //不同 交换物品
                    else
                    {
                        Item tempItem = currentItemUI.item;
                        int tempAmount = currentItemUI.Amount;
                        currentItemUI.SetItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                        InventoryManager.Instance.PickedItem.SetItem(tempItem, tempAmount);
                    }
                }
                //槽里没有 手上有
                else
                {

                    CreateItem(InventoryManager.Instance.PickedItem.item);
                    InventoryManager.Instance.ReducePickedItem();
                }

            }
            else 
            {
                //手上没东西 但是槽里有
                //装备
                if (transform.childCount > 0)
                {
                    ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                    if (currentItemUI.item is Equipment || currentItemUI.item is Weapon)//如果当前物品为武器装备类型
                    {
                        CharacterPanel.Instance.PutOn(currentItemUI);//将物品放入装备槽中
                        InventoryManager.Instance.HideToolTipContent();//隐藏信息提示框
                        CharacterPanel.Instance.UpdatePropertyText();//更新人物属性
                    }
                    else
                    {
                        InventoryManager.Instance.PickedItemUI(currentItemUI.item, currentItemUI.Amount);
                        Destroy(currentItemUI.gameObject);
                    }
                }
                //手上槽里都没 返回
                else
                {
                    return;
                }
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 1.自身是空的
        //1.1  鼠标上有物体(即 IsPickedItem = true)
        //1.11  按下Ctrl         放置当前鼠标上物品的一个
        //1.12  没有按下Ctrl     放置当前鼠标上物品的全部

        //1.2  鼠标上没有有物体(即 IsPickedItem = false)
        //1.21  返回，不处理

        //2.自身不是空的
        //2.1  鼠标上有物体(即 IsPickedItem = true)
        //2.11  自身ID 等于 鼠标物品ID
        //按下Ctrl          物品一个一个放入物品槽中
        //没有按下Ctrl      物品全部放入物品槽中（判断Capacity是否够放  只能放一部分和全部都能你放下
        //2.12  自身ID 不等于 鼠标物品ID
        //PickedItem和当前物品交换

        //2.2  鼠标上没有物体 (即 IsPickedItem = false)
        //2.21  按下Ctrl         取得当前物品槽中物品的一半
        //2.22  没有按下Ctrl     把当前物品槽里面的物品全部放到鼠标上

        //槽不为空(有itemUI)
        if (transform.childCount > 0)
        {
            ItemUI CurrentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
            //由清单管理的单例来查看是否有物体被鼠标选中
            if (InventoryManager.Instance.IsPickedItem == false)
            {
                //如果按下了左ctrl
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    //把物品的一半抓在鼠标上（至少一个
                    int tempAmount = (CurrentItemUI.Amount + 1) / 2;
                    InventoryManager.Instance.PickedItemUI(CurrentItemUI.item, tempAmount);
                    int remainAmount = CurrentItemUI.Amount - tempAmount;
                    //如果剩余数量小于等于0 删除物品
                    if (remainAmount <= 0)
                    {
                        Destroy(CurrentItemUI.gameObject);
                    }
                    else
                    {
                        CurrentItemUI.SetAmount(remainAmount);
                    }
                }
                //如果没按
                else
                {
                    //UI替换为被捡起的物品UI  销毁原来的UI
                    InventoryManager.Instance.PickedItemUI(CurrentItemUI.item, CurrentItemUI.Amount);
                    Destroy(CurrentItemUI.gameObject);
                }
            }
            //如果鼠标上有物品
            else
            {
                //如果两个物体id相同
                if (InventoryManager.Instance.PickedItem.item.Name == CurrentItemUI.item.Name)
                {
                    //如果按了左ctrl 一个个加
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (CurrentItemUI.Amount < CurrentItemUI.item.Capacity)//如果容量大于物 并且 手上要有东西
                        {
                            InventoryManager.Instance.ReducePickedItem();
                            CurrentItemUI.AddAmount();
                        }
                        else return;
                    }
                    else//没按
                    {
                        //如果物品容量大于鼠标上和物品槽里的物品数之和则直接放下
                        if (CurrentItemUI.item.Capacity >= InventoryManager.Instance.PickedItem.Amount + CurrentItemUI.Amount)
                        {
                            CurrentItemUI.AddAmount(InventoryManager.Instance.PickedItem.Amount);
                            InventoryManager.Instance.ReduceAllPickedItem();
                        }
                        else//吧物品槽存满 然后鼠标上还有剩
                        {
                            int tempAmount = CurrentItemUI.item.Capacity - CurrentItemUI.Amount;
                            CurrentItemUI.AddAmount(tempAmount);
                            InventoryManager.Instance.ReducePickedItem(tempAmount);
                        }
                    }
                }
                //交换物品
                else
                {
                    Item tempItem = CurrentItemUI.item;
                    int tempAmount = CurrentItemUI.Amount;
                    CurrentItemUI.SetItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(tempItem, tempAmount);
                }
            }
        }
        //槽为空
        // 1.自身是空的
        //1.1  鼠标上有物体(即 IsPickedItem = true)
        //1.11  按下Ctrl         放置当前鼠标上物品的一个
        //1.12  没有按下Ctrl     放置当前鼠标上物品的全部
        //1.2  鼠标上没有有物体(即 IsPickedItem = false)
        //1.21  返回，不处理
        else
        {
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    CreateItem(InventoryManager.Instance.PickedItem.item);
                    InventoryManager.Instance.ReducePickedItem();
                }
                else
                {
                    CreateItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.ReduceAllPickedItem();
                }
            }
            else
                return;
        }
    }
}
