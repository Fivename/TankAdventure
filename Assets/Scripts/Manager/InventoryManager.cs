using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _Instance;
    private static List<Item> itemList;
    private ToolTip toolTip;
    private bool isShowToolTip=false;
    private Canvas canvas;
    public Vector2 vector2 = new Vector2(20,-20);
    public GameObject tp;
   

    public ItemUI PickedItem { get; set; }
    private bool isPickedItem = false;
    public bool IsPickedItem {
        get { return isPickedItem; }
    }
    public static InventoryManager Instance
    {
        get
        {
            if(_Instance == null)
                _Instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            return _Instance;
        }
    }
    private void Awake()
    {
        ParseItemsJSON();
    }

    private void Start()
    {
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        tp = toolTip.gameObject;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        PickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        PickedItem.HideItemUI();

    }

    private void Update()
    {
        if(tp.activeInHierarchy == true)
        {
            if (isPickedItem == true)
            {
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
                PickedItem.SetItemUIPosition(position + vector2);
            }
            //如果有显示预览 每桢随着鼠标移动改变位置
            if (isShowToolTip)
            {
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
                toolTip.SetToolTipPosition(position + vector2);
            }
            if (isPickedItem == true && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
            {
                isPickedItem = false;
                PickedItem.HideItemUI();
            }
        }
        //else
        //{
        //    toolTip = ObjectPool.Instance.GetObject(tp).GetComponent<ToolTip>();
        //}

    }

    private void ParseItemsJSON()
    {
        //List<Item> itemList = new List<Item>();
        itemList = new List<Item>();
        //获取Json中的文本。文本在unity中是textasset类型
        string itemFile = Resources.Load<TextAsset>("itemData").text;
        //string itemFilePath = Application.dataPath + @"//ItemTest2.json";
        //string itemFile = File.ReadAllText(itemFilePath, Encoding.UTF8);
        //把json文本转换为jsondata格式
        JsonData jsonData = JsonMapper.ToObject(itemFile);

        for (int i = 0; i < jsonData.Count; i++)
        {
            string itemType = (string)jsonData[i]["Type"].ToString();
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), itemType);
            int id = (int)jsonData[i]["Id"];  //公用物品解析
            string name = (string)jsonData[i]["Name"];
            //string Quality = temp["quality"].str;
            Item.ItemQuality quality =
            (Item.ItemQuality)System.Enum.Parse(typeof(Item.ItemQuality), jsonData[i]["Quality"].ToString());
            string description = jsonData[i]["Description"].ToString();
            int capacity = Int32.Parse(jsonData[i]["Capacity"].ToString());
            int buyPrice = Int32.Parse(jsonData[i]["BuyPrice"].ToString());
            int sellPrice = Int32.Parse(jsonData[i]["SellPrice"].ToString());
            string sprite = jsonData[i]["Sprite"].ToString();

            Item item = null;
            switch (type)
            {
                case Item.ItemType.Consumable:
                    int hp = Int32.Parse(jsonData[i]["Hp"].ToString());
                    int mp = Int32.Parse(jsonData[i]["Mp"].ToString());
                    List<BuffBase> ActiveBuffList = new List<BuffBase>();
                    int Length = jsonData[i]["ActiveBuffList"].Count;
                    for (int j = 0; j < Length; j++)
                    {
                        float m_num = (float)jsonData[i]["ActiveBuffList"][j]["m_Num"];
                        Buff.BuffKind m_BuffKind = (Buff.BuffKind)System.Enum.Parse(typeof(Buff.BuffKind), jsonData[i]["ActiveBuffList"][j]["m_BuffKind"].ToString());
                        Buff.BuffOverlap m_BuffOverlap = (Buff.BuffOverlap)System.Enum.Parse(typeof(Buff.BuffOverlap), jsonData[i]["ActiveBuffList"][j]["m_BuffOverlap"].ToString());
                        Buff.BuffShutDownType m_BuffShutDownType = (Buff.BuffShutDownType)System.Enum.Parse(typeof(Buff.BuffShutDownType), jsonData[i]["ActiveBuffList"][j]["m_BuffShutDownType"].ToString());
                        Buff.BuffCalculateType m_BuffCalculateType = (Buff.BuffCalculateType)System.Enum.Parse(typeof(Buff.BuffCalculateType), jsonData[i]["ActiveBuffList"][j]["m_BuffCalculateType"].ToString());
                        int m_Layer = (int)jsonData[i]["ActiveBuffList"][j]["m_Layer"];
                        float m_Length = (float)jsonData[i]["ActiveBuffList"][j]["m_Length"];
                        int timeLimit = (int)jsonData[i]["ActiveBuffList"][j]["timeLimit"];
                        float timer = (float)jsonData[i]["ActiveBuffList"][j]["timer"];
                        Buff buff = new Buff(m_BuffKind, m_BuffOverlap, m_BuffShutDownType, m_BuffCalculateType, m_Length, m_num);
                        ActiveBuffList.Add(buff);
                    }
                    item = new Consumable(id, name, type, quality, description, capacity,
         buyPrice, sellPrice, sprite, hp, mp, ActiveBuffList);
                    break;
                case Item.ItemType.Equipment:
                    int ehp = Int32.Parse(jsonData[i]["Hp"].ToString());
                    int emp = Int32.Parse(jsonData[i]["Mp"].ToString());
                    int defense = Int32.Parse(jsonData[i]["Defense"].ToString());
                    int speed = Int32.Parse(jsonData[i]["Speed"].ToString());
                    Equipment.EquipmentType equipType = (Equipment.EquipmentType)System.Enum.Parse(typeof(Equipment.EquipmentType), jsonData[i]["EquipmentType"].ToString());
                    item = new Equipment(id, name, type, quality, description, capacity,
         buyPrice, sellPrice, sprite, defense, speed, ehp, emp, equipType);
                    break;
                case Item.ItemType.Material:
                    item = new Material(id, name, type, quality, description, capacity,
         buyPrice, sellPrice, sprite);
                    break;
                case Item.ItemType.Weapon:
                    int damage = Int32.Parse(jsonData[i]["Damage"].ToString());
                    Weapon.WeaponType weaponType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), jsonData[i]["WeaponType"].ToString());
                    item = new Weapon(id, name, type, quality, description, capacity,
         buyPrice, sellPrice, sprite, damage, weaponType);
                    break;
            }
            itemList.Add(item);
        }
    }

    public Item GetItemID(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTipContent(string content)
    {
        isShowToolTip = true;
        toolTip.Show(content);
    }

    public void HideToolTipContent()
    {
        isShowToolTip = false;
        toolTip.Hide();
    }

    /// <summary>
    /// 捡起物品槽中指定数量物品
    /// </summary>
    /// <param name="item"></param>
    /// <param name="Amount"></param>
    public void PickedItemUI(Item item,int amount)
    {
        PickedItem.SetItem(item, amount);//将要拾取的物品信息赋值给PickedItem
        isPickedItem = true;//当前鼠标上有物体了
        PickedItem.ShowItemUI();//显示ItemUI
        toolTip.Hide();//隐藏预览
    }

    /// <summary>
    /// 减少鼠标上的物品信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="Amount"></param>
    public void ReducePickedItem(int amount=1)
    {
        PickedItem.ReduceAmount(amount);
        if(PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.HideItemUI();
        }
    }

    public void ReduceAllPickedItem()
    {
        PickedItem.ReduceAmount(PickedItem.Amount);
        isPickedItem = false;
        PickedItem.HideItemUI();
    }
}
