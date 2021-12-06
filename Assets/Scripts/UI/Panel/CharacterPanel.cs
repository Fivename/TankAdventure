using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : Inventory
{
    private static CharacterPanel _instance;
    private Text tex;
    //private PlayerController playerController;
    private Player player;

    public override void Start()
    {
        base.Start();
        player = Player.getInstance();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        tex = transform.Find("PropertyPanel/Text").GetComponent<Text>();
        tex.text = "<size=13>生命:10\n魔法:10\n速度:10\n防御:10\n攻击:10\n</size>"; 
    }

    public static CharacterPanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("CharacterPanel").GetComponent<CharacterPanel>();
            return _instance;
        }
    }

    /// <summary>
    /// 将相同类型的装备/武器放入对应的装备/武器物品槽中
    /// </summary>
    /// <param name="item"></param>
    public void PutOn(ItemUI itemUI)
    {
        foreach (Slot slot in slots)
        {
            EquipmentSlot equipmentSlot = (EquipmentSlot)slot;
            if (equipmentSlot.IsRightItem(itemUI.item))
            {
                if (equipmentSlot.transform.childCount > 0)
                {
                    Item tempItem;
                    ItemUI currentItem = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    tempItem = currentItem.item;
                    currentItem.SetItem(itemUI.item);
                    DestroyImmediate(itemUI.gameObject);
                    KnapsackPanel.Instance.StoreItem(tempItem);
                    return;
                }
                else
                {
                    equipmentSlot.CreateItem(itemUI.item);
                    DestroyImmediate(itemUI.gameObject);
                    return;
                }
                //break;
            }
        }
    }

    /// <summary>
    /// 将装备/武器放入背包的物品槽中
    /// </summary>
    /// <param name="item"></param>
    public void PutOff(Item item)
    {
        KnapsackPanel.Instance.StoreItem(item);
    }

    /// <summary>
    /// 更新人物属性
    /// </summary>
    public void UpdatePropertyText()
    {
        int defense = 0,hp = 0,mp = 0,damage= 0;
        float speed = 0f;
        foreach (EquipmentSlot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().item;
                if (item is Equipment)
                {
                    Equipment e = (Equipment)item;
                    speed += e.Speed;
                    defense += e.Defense;
                    hp += e.Hp;
                    mp += e.Mp;
                }
                else if (item is Weapon)
                {
                    Weapon w = (Weapon)item;
                    damage += w.Damage;
                }
            }
        }
        speed += player.RunSpeed;
        defense += player.Defens;
        hp += player.Health;
        mp += player.MagicPoint;
        damage += player.Damage;
        tex.text = string.Format("<size=13>生命:{0}\n魔法:{1}\n速度:{2}\n防御:{3}\n攻击:{4}\n</size>"
            , hp, mp, speed, defense, damage);
    }
}
