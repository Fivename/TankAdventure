using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Consumable : Item
{
    public int Hp;
    public int Mp;
    public List<BuffBase> ActiveBuffList;

    public Consumable(int id, string name, ItemType type, ItemQuality quality, string description, int capacity,
        int buyPrice, int sellPrice, string sprite, int hp, int mp, List<BuffBase> activeBuffDic)
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Hp = hp;
        this.Mp = mp;
        this.ActiveBuffList = activeBuffDic;
    }

    public void AddBuff(CostumEntityLogic costumEntityLogic)
    {
        foreach(Buff activeBuffBase in ActiveBuffList)
        {
            Buff activebuff = new Buff(activeBuffBase.m_BuffKind, activeBuffBase.m_BuffOverlap,
                activeBuffBase.m_BuffShutDownType, activeBuffBase.m_BuffCalculateType, activeBuffBase.m_Length, activeBuffBase.m_Num);
            costumEntityLogic.AddBuff(activebuff);
        }
    }

}
