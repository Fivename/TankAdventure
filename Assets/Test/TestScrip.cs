using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Text;
using System.IO;
using System;

public class TestScrip : MonoBehaviour
{
    public Item consumable;
    public List<Item> itemList;
    private void Awake()
    {
        //List<BuffBase> dic = new List<BuffBase>();
        //BuffBase buff1 = new BuffBase(BuffBase.BuffKind.Heal, BuffBase.BuffOverlap.StackedTime, BuffBase.BuffShutDownType.All, BuffBase.BuffCalculateType.Once, 3, 5);
        //BuffBase buff2 = new BuffBase(BuffBase.BuffKind.Heal, BuffBase.BuffOverlap.StackedTime, BuffBase.BuffShutDownType.All, BuffBase.BuffCalculateType.Once, 4, 6);
        //dic.Add(buff1);
        ////dic.Add(buff2);
        //consumable = new Consumable(1, "测试药水", Item.ItemType.Consumable, Item.ItemQuality.Artifact, "这是一个药水", 10, 50, 25, "/测试路径", 10, 0, dic);
    }

    private void OnEnable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //ParseItemsJSON();

        //string json = JsonMapper.ToJson(consumable);
        //string savePath = Application.dataPath + @"/Test/ItemTest.json";
        //File.WriteAllText(savePath, json, Encoding.UTF8);        
        //string json = JsonMapper.ToJson(consumable);
        //string savePath = Application.dataPath + @"/Test/ItemTest2.json";
        //File.WriteAllText(savePath, json, Encoding.UTF8);

    }
    #region Json测试
    private void ParseItemsJSON()
    {
        //List<Item> itemList = new List<Item>();
        itemList = new List<Item>();
        //获取Json中的文本。文本在unity中是textasset类型
        //TextAsset itemText = Resources.Load<TextAsset>("itemData");
        string itemFilePath = Application.dataPath + @"/Test/ItemTest2.json";

        string itemFile = File.ReadAllText(itemFilePath, Encoding.UTF8);
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
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
